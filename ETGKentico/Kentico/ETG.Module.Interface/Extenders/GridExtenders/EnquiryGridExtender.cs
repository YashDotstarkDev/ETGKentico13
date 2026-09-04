using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Castle.Core.Internal;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.ImportExport;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.SiteProvider;
using CMS.UIControls;
using ETG.Core.Forms;
using ETG.Core.Kentico;
using ETG.Module.Data;
using ETG.Module.Interface.Services;

namespace ETG.Module.Interface.Extenders.GridExtenders
{
    public class EnquiryGridExtender : ControlExtender<UniGrid>
    {
        private IETGUserService _etgUserService;

        private CurrentUserInfo CurrentUser
        {
            get { return MembershipContext.AuthenticatedUser; }
        }

        public EnquiryGridExtender()
        {
            _etgUserService = new ETGUserService(new KenticoSiteContext());
        }
        public override void OnInit()
        {
            Control.OnExternalDataBound += Control_OnExternalDataBound;
            Control.OnBeforeDataReload += Control_OnBeforeDataReload;
            Control.OnLoadColumns += Control_OnLoadColumns;
            Control.FilterIsSet = true;
            Control.JavaScriptModule = "/custom/scripts/enquiry.js";
            Control.HeaderActions.ActionPerformed += HeaderActions_ActionPerformed;
            Control.HeaderActions.AddAction(new HeaderAction
            {
                ButtonStyle = ButtonStyle.Default,
                Text = "Export",
                CommandName = "export",
            });
        }
        
        private void HeaderActions_ActionPerformed(object sender, CommandEventArgs args)
        {
            switch (args.CommandName.ToLowerCSafe())
            {
                default:
                    break;

                case "export":
                    var items = BizFormItemProvider.GetItems<EnquireItem>().OrderBy("FormInserted");
                    var response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearHeaders();
                    var dataExportHelper = new DataExportHelper(items);
                    dataExportHelper.ExportToCSV(items, 0, response.OutputStream, false);
                    response.ContentType = DataExportHelper.GetDataExportFormatContentType(DataExportFormatEnum.CSV);
                    HTTPHelper.AddContentDispositionHeader(response, true, "etgenquiries.csv");
                    response.OutputStream.Flush();
                    response.OutputStream.Close();
                    response.End();
                    break;
            }
        }

        private void Control_OnLoadColumns()
        {
            var currentSiteName = SiteContext.CurrentSiteName;
        
            if (CurrentUser.IsInRole(ETGUserRole.MANAGER, currentSiteName) ||
                CurrentUser.IsInRole(ETGUserRole.MARKETING, currentSiteName) ||
                CurrentUser.IsInRole(ETGUserRole.ENQUIRY_MANAGEMENT_USER, currentSiteName) ||
                CurrentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
            {
                return;
            }

            var utmColumns = Control.GridColumns.Columns.Where(a => a.Source.Contains("UTM"));
            
            foreach (var utmColumn in utmColumns)
            {
                utmColumn.Visible = false;
            }
        }

        private void Control_OnBeforeDataReload()
        {
            if (CurrentUser.IsInRole(ETGUserRole.MANAGER, SiteContext.CurrentSiteName))
            {
                return;
            }

            if (CurrentUser.IsInRole(ETGUserRole.CONSULTANT, SiteContext.CurrentSiteName) ||
                CurrentUser.IsInRole(ETGUserRole.TEAM_LEADER, SiteContext.CurrentSiteName))
            {
                var destinations = _etgUserService.GetCurrentLoginDestinations(CurrentUser);

                if (destinations.IsNullOrEmpty())
                {
                    //consultant not allowed to view any destination 
                    Control.WhereClause = "1=2";
                }
                else
                {
                    if (Control.WhereClause.IsNullOrEmpty())
                    {
                        Control.WhereClause = GetDestinationWhere(destinations);
                    }
                    else
                    {
                        Control.WhereClause += " AND (" + GetDestinationWhere(destinations) + ")";
                    }
                }
            }

            Control.OrderBy = "FormInserted DESC";
        }

        private string GetDestinationWhere(List<string> destinations)
        {
            var where = new StringBuilder();

            if (destinations.IsNullOrEmpty())
            {
                return string.Empty;
            }

            for (int i = 0; i < destinations.Count; i++)
            {
                if (i > 0)
                {
                    where.Append(" Or ");
                }

                where.Append($" PreferredDestination LIKE '{destinations[i].Replace("'", "''''")}'");
            }

            return where.ToString();
        }

        private int currentItemID = 0;
        private string currentDestination = string.Empty;
        
        private object Control_OnExternalDataBound(object sender, string sourceName, object parameter)
        {
            var dropdownHtml = new StringBuilder();
            switch (sourceName)
            {
                default:
                    return parameter;

                case "delete":
                    if (!CurrentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin) &&
                        !CurrentUser.IsInRole(ETGUserRole.MANAGER, SiteContext.CurrentSiteName) &&
                        !CurrentUser.IsInRole(ETGUserRole.TEAM_LEADER, SiteContext.CurrentSiteName))
                    {
                        var button = sender as CMSGridActionButton;
                        button.Visible = false;
                    }

                    return parameter;
                
                case "itemid":
                    currentItemID = ValidationHelper.GetInteger(parameter, 0);
                    return parameter;
                
                case "destination":
                    currentDestination = ValidationHelper.GetString(parameter, string.Empty);
                    return parameter;
                
                case "status":
                    var currentStatus = ValidationHelper.GetString(parameter, "");
                    
                    dropdownHtml.Append($"<select class=\"select-status\" data-id=\"{currentItemID}\" >");
                    foreach (var status in ETGEnquireStatus.Statuses)
                    {
                        dropdownHtml.Append(
                            $"<option {(currentStatus == status ? "selected" : "")} value=\"{status}\">{status}</option>");
                    }
                    
                    dropdownHtml.Append("</select");

                    return dropdownHtml.ToString();
                
                case "assignee":
                    var assignees = _etgUserService.GetAssignees(currentDestination);
                    dropdownHtml.Append($"<select class=\"select-assignee\" data-id=\"{currentItemID}\" class=\"form-control\">");
                    dropdownHtml.Append("<option value=\"\">Select</option>");
                    foreach (var assignee in assignees)
                    {
                        dropdownHtml.Append(
                            $"<option {(ValidationHelper.GetGuid(parameter, Guid.Empty) == assignee.UserGUID ? "selected" : "")} value=\"{assignee.UserGUID}\">{assignee.FullName}</option>");
                    }
                    dropdownHtml.Append("</select");

                    return dropdownHtml.ToString();

                case "dateinserted":
                case "dateupdated":
                    return ValidationHelper.GetDateTime(parameter, DateTime.MinValue).ToString("dd/MM/yyyy h:mm:ss tt");
            }
        }
    }
}