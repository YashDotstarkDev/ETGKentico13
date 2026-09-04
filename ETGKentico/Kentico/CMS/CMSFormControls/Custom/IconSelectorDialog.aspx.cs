using System;
using System.Drawing;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.UIControls;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CMSApp.CMSFormControls.Custom
{
    public partial class IconSelectorDialog : CMSModalPage
    {
        #region "Variables"

        private string mHiddenFieldId;
        private string mIconClassId;
        private string mIconClass;
        #endregion


        #region "Page events"

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Save += btnOk_Click;

            mHiddenFieldId = Request.QueryString["hiddenId"];
            mIconClassId = Request.QueryString["iconClassId"];

            mIconClass = WindowHelper.GetItem(mHiddenFieldId) as string;

            if (!IsPostBack)
            {
                hidSelectedIcon.Value = mIconClass;
            }
        }

        public List<string> GetIcons()
        {
            var path = Server.MapPath("~/cmsformcontrols/custom/icons.txt");

            var iconString = File.ReadAllText(path);

            return iconString.Split('\n').Select(a => a.Trim()).ToList();

        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            repIcons.DataSource = GetIcons();
            repIcons.DataBind();
            RegisterScriptCode();
            SetSaveJavascript("return true");

            WindowHelper.Remove(mHiddenFieldId);
        }

        protected string IsSelected(string icon)
        {
            if (!string.IsNullOrWhiteSpace(mIconClass) && icon == mIconClass)
            {
                return "selected";
            }

            return string.Empty;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            if (true)
            {
                
                string submitScript = String.Format("wopener.setParameters('{0}',{1},{2}); CloseDialog();", hidSelectedIcon.Value, ScriptHelper.GetString(mHiddenFieldId), ScriptHelper.GetString(mIconClassId));
                ScriptHelper.RegisterStartupScript(Page, typeof(String), "SubmitScript", submitScript, true);
            }
        }

        #endregion


        #region "Methods"
        
        private void RegisterScriptCode()
        {
            ScriptHelper.RegisterWOpenerScript(Page);

           /* var scripts = "$('.icon-list a').click(function (){\r\n" +
                "$('.icon-list a').removeClass('selected')\r\n" +
                "$(this).addClass('selected')\r\n" +

                "})";*/
            ScriptHelper.RegisterClientScriptBlock(this, typeof(String), "CustomIconSelectorDialogScripts", "", true);
        }

        #endregion
    }
}