using Castle.Core.Internal;
using ETG.Data.Models.Base;

namespace ETG.Data.Destination.Models
{
    public class DestinationModel : PageNodeModel
    {
        public DestinationSummaryModel BasicInfo { get; set; }
        public string JsonSchema { get; set; }
        public string SVGMap { get; set; }
        public string Detail { get; set; }
        public string WhenToVisit { get; set; }
        public string ViewPackagesLabel { get; set; }
        public string FeatureTourCodes { get; set; }

        public string FeatureCruiseCodes { get; set; }

        public bool HideGoogleReviews { get; set; }

        public bool IsSubRegion
        {
            get
            {
                if (PageAliasPath.IsNullOrEmpty())
                {
                    return false;
                }

                return (PageAliasPath.Split('/').Length == 4);
            }
        }

        public string RelatedArticlesHeader { get; set; }
        public DestinationStickyModel DestinationStickyModel { get; set; } = new DestinationStickyModel();
    }
}
