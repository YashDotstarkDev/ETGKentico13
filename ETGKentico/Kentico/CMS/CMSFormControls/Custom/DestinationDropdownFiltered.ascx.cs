using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.Core.Internal;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.SiteProvider;
using CommonServiceLocator;
using ETG.Core.Forms;
using ETG.Core.Kentico;
using ETG.Data.Destination.Services;
using ETG.Module.Data;
using ETG.Module.Interface.Services;

namespace CMSApp.CMSFormControls.Custom
{
    public partial class DestinationDropdownFiltered : FormEngineUserControl
    {

        #region "Properties"
        private CurrentUserInfo CurrentUser
        {
            get { return MembershipContext.AuthenticatedUser; }
        }
        /// <summary>
        /// Gets or sets the enabled state of the control.
        /// </summary>
        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                ddlDestination.Enabled = value;
            }
        }


        /// <summary>
        /// Gets or sets field value.
        /// </summary>
        public override object Value
        {
            get { return ddlDestination.SelectedValue; }
            set
            {
                try
                {

                    ddlDestination.SelectedValue = (string)value;
                }
                catch
                {

                }
            }
        }


        /// <summary>
        /// Gets ClientID of the textbox with emailinput.
        /// </summary>
        public override string ValueElementID
        {
            get { return ddlDestination.ClientID; }
        }

        #endregion


        #region "Methods"

        private string GetCurrenctSelected()
        {
            var itemid = ValidationHelper.GetInteger(Request["objectid"], 0);

            if (itemid == 0)
            {
                return string.Empty;
            }
            var item = BizFormItemProvider.GetItem<EnquireItem>(itemid);

            return item?.PreferredDestination;
        }

        /// <summary>
        /// Page load.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var destinations = GetDestinations();

                ddlDestination.Items.Add(new ListItem("Select", string.Empty));
                foreach (var destination in destinations)
                {
                    ddlDestination.Items.Add(new ListItem(destination, destination));
                }
                try
                {
                    ddlDestination.SelectedValue = GetCurrenctSelected();
                }
                catch
                {

                }
            }
        }

        private List<string> GetDestinations()
        {
            //var etgUserService = new ETGUserService(new KenticoSiteContext());
            //if (CurrentUser.IsInRole(ETGUserRole.MANAGER, SiteContext.CurrentSiteName) ||
            //    CurrentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
            //{
            //    var destinationService = ServiceLocator.Current.GetInstance<IDestinationService>();

            //    return destinationService.GetMainDestinations(true).Select(a => a.Name).ToList();
            //}
            //return etgUserService.GetCurrentLoginDestinations(CurrentUser);

            var destinationService = ServiceLocator.Current.GetInstance<IDestinationService>();
            return destinationService.GetMainDestinations(true).Select(a => a.Name).ToList();
        }


        /// <summary>
        /// Returns <c>true</c> if email input is empty or has correct format.
        /// </summary>
        /// <remarks>Also returns <c>true</c> if email input contains macro and is not used on live site.</remarks>>
        public override bool IsValid()
        {
            return true;
            /*
            if (CurrentUser.IsInRole(ETGUserRole.MANAGER, SiteContext.CurrentSiteName) ||
                CurrentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
            {
                return true;
            }
            var destination = ValidationHelper.GetString(Value, String.Empty);
            var isValid = !destination.IsNullOrEmpty();

            if (!isValid)
            {
                ValidationError = "Destination is required";
            }

            return isValid;*/
        }



        #endregion
    }
}