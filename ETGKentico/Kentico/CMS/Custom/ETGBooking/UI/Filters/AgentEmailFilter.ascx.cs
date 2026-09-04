using System;
using System.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;

namespace CMSApp.Custom.ETGBooking.UI.Filters
{
    public partial class AgentEmailFilter : FormEngineUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override object Value
        {
            get => txtEmail.Text;
            set => txtEmail.Text = ValidationHelper.GetString(value, string.Empty);
        }

        public override string GetWhereCondition()
        {
            if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))

            {
                return $"(AgentLogoEmails LIKE '%{txtEmail.Text.Replace("'","''")}%' OR AgentLogoID in (select AgentEmailAgentLogoID from ETG_AgentEmailLogo where AgentEmailEmailAddress LIKE '%{txtEmail.Text.Replace("'","''")}%'))";
            }

            return string.Empty;
        }
    }
}