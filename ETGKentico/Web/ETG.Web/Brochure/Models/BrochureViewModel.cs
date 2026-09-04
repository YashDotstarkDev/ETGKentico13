using ETG.Web.Models.Base;
using System;
using ETG.Core.Constants;

namespace ETG.Web.Brochure.Models
{
    public class BrochureViewModel : PageNodeViewModel
    {
        public Guid BrochureNodeGuid { get; set; }
        public string Name { get; set; }
        public string CoverImage { get; set; }
        public string FilePath { get; set; }
        public string IssuuLink { get; set; }
        public string FlipStackID { get; set; }

        public string BrochureLink => PathConstants.GetBrochureLink(PageAlias);

        public string AgentPurchaseLink { get; set; }
        public string DestinationGuids { get; set; }
        public bool IsComingSoon { get; set; }
    }
}
