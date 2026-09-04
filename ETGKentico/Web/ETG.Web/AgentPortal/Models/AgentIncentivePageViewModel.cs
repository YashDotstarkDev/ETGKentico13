using ETG.Web.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;

namespace ETG.Web.AgentPortal.Models
{
    public class AgentIncentivePageViewModel : BasePageViewModel, IViewModel
    {
        public AgentIncentiveViewModel Page { get; set; }
        public PageHeroViewModel Hero
        {
            get
            {
                if (Page == null)
                {
                    return null;
                }
                return new PageHeroViewModel
                {
                    Heading = Page.Name,
                    HeroImage = Page.HeroImage
                };
            }
        }
    }
}
