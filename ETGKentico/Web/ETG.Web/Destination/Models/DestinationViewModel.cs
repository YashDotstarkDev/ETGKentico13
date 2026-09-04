using ETG.Web.Models;
using ETG.Web.Models.Base;

namespace ETG.Web.Destination.Models
{
    public class DestinationViewModel : PageNodeViewModel
    {
        public DestinationSummaryViewModel BasicInfo { get; set; }
        public string JsonSchema { get; set; }
        public string SVGMap { get; set; }
        public string Detail { get; set; }
        public string WhenToVisit { get; set; }
        
        public bool HideGoogleReviews { get; set; }
        
        public string ViewPackagesLabel { get; set; }
        public string FeatureTourCodes { get; set; }
        public string RelatedArticlesHeader { get; set; }
    }
}
