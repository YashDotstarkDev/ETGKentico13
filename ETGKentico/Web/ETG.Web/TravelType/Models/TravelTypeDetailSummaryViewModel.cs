using ETG.Web.Models;

namespace ETG.Web.TravelType.Models
{
    public class TravelTypeDetailSummaryViewModel:IViewModel
    {
        public string HeroImage { get; set; }
        public string HeroAltText { get; set; }
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public string Path { get; set; }
        public string PageAlias { get; set; }
        public bool HideGoogleReviews { get; set; }
    }
}