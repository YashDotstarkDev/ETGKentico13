using System;
using System.Web.UI.WebControls;
using CMS.Base.Web.UI;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;

namespace ETG.Module.Interface.Extenders.PageExtenders
{
    public class EnquiryListingPageExtender : PageExtender<CMSPage>
    {
        public override void OnInit()
        {
            Page.Load += Page_Load;
        }
        
        private CurrentUserInfo CurrentUser
        {
            get { return MembershipContext.AuthenticatedUser; }
        }
        
        private void Page_Load(object sender, EventArgs e)
        {
            var literal = new Literal
            {
                Text =
                    $"<div style=\"display:none\" class=\"data-presentation-url\" >{SiteContext.CurrentSite.SitePresentationURL}</div>"
            };

            Page.Controls.Add(literal);
        }
    }
}