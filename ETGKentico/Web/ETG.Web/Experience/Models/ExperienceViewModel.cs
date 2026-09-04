using ETG.Web.Models.Base;

namespace ETG.Web.Experience.Models
{
    public class ExperienceViewModel : PageNodeViewModel
    {
        public ExperienceSummaryViewModel SummaryInfo { get; set; }
        public string Heading { get; set; }

        public bool HideGoogleReviews { get; set; }

        public string HeroIconImage { get; set; }
        public string HeroImageAccreditation { get; set; }
        public string FeatureTourCodes { get; set; }
        public string JsonSchema { get; set; }
    }
}
