using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CMS.Base.Web.UI;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;

namespace ETG.Module.Interface.Extenders.PageExtenders
{
    public class EnquiryFormPageExtender : PageExtender<CMSPage>
    {
        public override void OnInit()
        {
            Page.Load += Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            var literal = new Literal();
            literal.Text =
                $"<link rel=\"stylesheet\" href=\"/custom/styles/enquiry.css\">\n";
           
            Page.Controls.Add(literal);
            Page.ClientScript.RegisterClientScriptInclude( "enquiryscript", "/custom/scripts/enquiryscripts.js");
        }
    }
}