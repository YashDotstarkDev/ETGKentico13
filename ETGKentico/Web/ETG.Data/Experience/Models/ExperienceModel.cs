using ETG.Data.Models.Base;

namespace ETG.Data.Experience.Models
{
    public class ExperienceModel : PageNodeModel
    {
        public ExperienceSummaryModel SummaryInfo { get; set; }
        public string Heading { get; set; }

        public string HeroIconImage { get; set; }
        public string HeroIconSVG { get; set; }
        public string HeroImageAccreditation { get; set; }
        public string FeatureTourCodes { get; set; }
        public string FeatureCruiseCodes { get; set; }
        public bool HideGoogleReviews { get; set; }
        public string JsonSchema { get; set; }
    }
}
