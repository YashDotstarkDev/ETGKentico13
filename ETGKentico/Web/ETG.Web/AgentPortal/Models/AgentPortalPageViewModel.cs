using ETG.Web.Models;
using System.Collections.Generic;
using ETG.Web.Brochure.Models;
using ETG.Web.Models.Base;

namespace ETG.Web.AgentPortal.Models
{
    public class AgentPortalPageViewModel : BasePageViewModel, IPagedViewModel<AgentPortalPageViewModel>, IViewModel
    {
        public AgentPortalMainViewModel Page { get; set; }
        public List<AgentToolkitViewModel> AgentToolkits { get; set; }
        public List<AgentIncentiveViewModel> AgentIncentives { get; set; }
        public List<AgentWebinarViewModel> AgentWebinars { get; set; }
        public List<BrochureViewModel> Brochures { get; set; }

        public bool BookingOptionsEnabled => !string.IsNullOrWhiteSpace(Page.BookingOptionsHeading) &&
                                             !string.IsNullOrWhiteSpace(Page.BookingOptionsDescription);
    }
}
