using System;
using System.Web.UI;

using CMS.Base.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;

namespace CMSApp.CMSFormControls.Custom
{
    public partial class IconSelector : FormEngineUserControl, ICallbackEventHandler
    {
        #region "Variables"
        
        private bool mDisplayClearButton = true;

        #endregion


        #region "Public properties"

        /// <summary>
        /// Gets or sets field value.
        /// </summary>
        public override object Value
        {
            get
            {
                return txtIconClass.Text;
            }
            set
            {
                txtIconClass.Text = (string)value;
            }
        }


       
        /// <summary>
        /// If true display button for clear font.
        /// </summary>
        public bool DisplayClearButton
        {
            get
            {
                return mDisplayClearButton;
            }
            set
            {
                mDisplayClearButton = value;
            }
        }


        /// <summary>
        /// ClientId of icon type text box.
        /// </summary>
        public string IconTextBoxClientId
        {
            get
            {
                return txtIconClass.ClientID;
            }
        }

        #endregion


        #region "Control events"

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (hfValue.Value != String.Empty)
            {
                txtIconClass.Text = hfValue.Value;
            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            btnChangeIcon.OnClientClick = Page.ClientScript.GetCallbackEventReference(this, "document.getElementById('" + txtIconClass.ClientID + "').value", "selectIcon", null) + ";return false;";

            btnClearIcon.Visible = DisplayClearButton;

            RegisterScripts();
        }


        protected void btnClearIcon_Click(object sender, EventArgs e)
        {
            // Clear value in selector
            txtIconClass.Text = String.Empty;
            hfValue.Value = String.Empty;
        }

        #endregion


        #region "Methods"

        private void RegisterScripts()
        {
            // Register dialog script
            ScriptHelper.RegisterDialogScript(Page);

            // Create script for open dialog, get parameters and refresh
            string script = @" 
function selectIcon(queryParams) {
    modalDialog('" + ResolveUrl("~/CMSFormControls/Custom/IconSelectorDialog.aspx") + @"' + queryParams, 'CustomIconSelector', 500, 470);
}
function setParameters(val,hf,tb) {
    document.getElementById(hf).value = val;
    document.getElementById(tb).value = val;
}";

            ScriptHelper.RegisterClientScriptBlock(this, typeof(String), "CustomIconSelectorScripts", script, true);
        }


        /// <summary>
        /// Sets "onchange" javascript function to control's input.
        /// </summary>
        /// <param name="fnction"></param>
        public void SetOnChangeAttribute(string fnction)
        {
            txtIconClass.Attributes["onchange"] = fnction;
        }

        #endregion


        #region "Callback handlers"

        private string callBackArg;


        public string GetCallbackResult()
        {
            // Add font parameters for selector dialog
            string value = (String.IsNullOrEmpty(callBackArg)) ? "" : callBackArg;
            WindowHelper.Add(hfValue.ClientID, value);

            return String.Format("?hiddenId={0}&iconClassId={1}&group={2}", hfValue.ClientID, txtIconClass.ClientID, GetValue("Group"));
        }


        public void RaiseCallbackEvent(string eventArgument)
        {
            callBackArg = eventArgument;
        }

        #endregion
    }
}