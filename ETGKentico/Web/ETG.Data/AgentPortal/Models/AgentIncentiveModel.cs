using ETG.Data.Models.Base;

namespace ETG.Data.AgentPortal.Models
{
    public class AgentIncentiveModel : PageNodeModel
    {
        public  string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Destination { get; set; }
        public string HeroImage { get; set; }
        public string HeroImageCaption { get; set; }
        public string Url { get; set; }
    }
}
