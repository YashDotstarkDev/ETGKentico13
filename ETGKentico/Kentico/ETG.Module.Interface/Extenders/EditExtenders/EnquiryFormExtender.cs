using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;
using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.UIControls;
using ETG.Core.Kentico;
using ETG.Module.Data;
using ETG.Module.Interface.Services;

namespace ETG.Module.Interface.Extenders.GridExtenders
{
    public class EnquiryFormExtender : ControlExtender<UIForm>
    {
        private IETGUserService _etgUserService;

        public EnquiryFormExtender()
        {
            _etgUserService = new ETGUserService(new KenticoSiteContext());
        }
        private CurrentUserInfo CurrentUser => MembershipContext.AuthenticatedUser;

        public override void OnInit()
        {
            Control.OnAfterDataLoad += Control_OnAfterDataLoad;
            Control.OnAfterSave += Control_OnAfterSave;
            
        }

        private void Control_OnAfterSave(object sender, EventArgs e)
        {
            SetConfirmedStatusFields();
        }

        private string GetValue( string fieldName)
        {
            for (int i = 0; i < Control.Fields.Count; i++)
            {

                if (Control.Fields[i] == fieldName)
                {
                    return ValidationHelper.GetString(Control.FieldControls[Control.Fields[i]].Value, string.Empty);
                }
            }

            return string.Empty;
        }

        private string AddCssClass( string fieldName, string cssClass)
        {
            for (int i = 0; i < Control.Fields.Count; i++)
            {

                if (Control.Fields[i] == fieldName)
                {
                    return Control.FieldControls[Control.Fields[i]].CssClass += " " + cssClass;
                }
            }

            return string.Empty;
        }

        private void SetConfirmedStatusFields()
        {
            
            /*var status = GetValue("Status");
            if (status != "Confirmed")
            {
                //Hide some fields when status is not confirmed
                for (int i = 0; i < Control.Fields.Count; i++)
                {
                    if (ETGStatusConfirmed.FieldNames.Any(a => a == Control.Fields[i]))
                    {
                        Control.FieldLabels[Control.Fields[i]].AddCssClass("hide");
                        Control.FieldControls[Control.Fields[i]].CssClass += " hide";
                    }
                }
            }*/
        }
        private void Control_OnAfterDataLoad(object sender, EventArgs e)
        {
            AddCssClass("Status", "form-select-status");
            SetConfirmedStatusFields();
        }

    }
}