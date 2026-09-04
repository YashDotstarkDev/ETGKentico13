using Devotion.Automapper.Common;
using System;
using ETG.Data.Models.Base;

namespace ETG.Data.Brochure.Models
{
    public class BrochureModel : PageNodeModel
    {
        public Guid BrochureNodeGuid { get; set; }
        public string Name { get; set; }
        public string CoverImage { get; set; }
        public string FilePath { get; set; }
        public string IssuuLink { get; set; }
        public string FlipStackID { get; set; }
        public string AgentPurchaseLink { get; set; }
        public string DestinationGuids { get; set; }
        public bool IsComingSoon { get; set; }
    }
}
