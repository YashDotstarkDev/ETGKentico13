using Devotion.Automapper.Common;

namespace ETG.Data.TravelType.Models
{
    public class TravelTypeDetailSummaryModel : IDataModel
    {
        public string HeroImage { get; set; }
        public string HeroAltText { get; set; }
        public string HeroIconClass { get; set; }
        public string HeroIconSvg { get; set; }
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public string Path { get; set; }
        public string PageAlias { get; set; }

        public string ThemedPackagesHeading { get; set; }
        public string ThemedPackagesUrl { get; set; }

        public bool HideGoogleReviews { get; set; }
    }
}