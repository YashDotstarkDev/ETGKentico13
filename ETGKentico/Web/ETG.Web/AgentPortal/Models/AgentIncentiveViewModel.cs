using ETG.Web.Models;
using ETG.Web.Models.Base;

namespace ETG.Web.AgentPortal.Models
{
    public class AgentIncentiveViewModel : PageNodeViewModel{
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Destination { get; set; }
        public string HeroImage { get; set; }
        public string HeroImageCaption { get; set; }
        public string Url { get; set; }
    }
}
