using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.Core.Internal;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using ETG.Core.Kentico;
using ETG.Module.Interface.Services;

namespace CMSApp.Custom.EnquiryManagement
{
    public partial class ConsultantFilter : FormEngineUserControl
    {/// <summary>
        /// Gets or sets the value selected within the filter.
        /// </summary>
        public override object Value
        {
            get
            {
                return ddlConsultant.SelectedValue;
            }
            set
            {
                ddlConsultant.SelectedValue = ValidationHelper.GetString(value, "");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Only initializes the culture options on the first page load
            if (IsPostBack == false)
            {
                var etgUserService = new ETGUserService(new KenticoSiteContext());

                var consultants = etgUserService.GetAssignees();

                ddlConsultant.DataValueField = "UserGuid";
                ddlConsultant.DataTextField = "FullName";
                ddlConsultant.DataSource = consultants;
                ddlConsultant.DataBind();
                ddlConsultant.Items.Insert(0, new ListItem("Select", ""));
            }
        }

        public override string GetWhereCondition()
        {
            var filterConsultantGuid = Value as string;

            // Returns an empty condition if the special (any) option is selected in the filter
            if (filterConsultantGuid.IsNullOrEmpty())
            {
                return string.Empty;
            }

            // Returns a condition for loading users whose preferred content culture matches the filter's value.
            return "(AssignedUserGuid = N'" + filterConsultantGuid + "')";
        }
    }
}