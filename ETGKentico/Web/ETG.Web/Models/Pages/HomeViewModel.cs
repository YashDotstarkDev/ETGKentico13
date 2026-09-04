using ETG.Web.Article.Models;
using ETG.Web.DestinationExpertTeam.Models;
using ETG.Web.Models.Common;
using ETG.Web.Tour.Models;
using System.Collections.Generic;
using ETG.Web.Experience.Models;
using ETG.Web.Models.Menu;
using ETG.Web.Models.PageTypes;
using ETG.Web.Models.Widgets.FAQWidget;

namespace ETG.Web.Models.Pages
{
    public class HomeViewModel : IViewModel
    {
        public HomepageViewModel Page { get; set; }
        public List<ImageViewModel> HeroSliderImages { get; set; }

        public ProofPointComponentViewModel ProofPointsComponent { get; set; }

        public DestinationComponentViewModel DestinationComponent { get; set; }

        public TourListingViewModel FeatureTours { get; set; }
        
        public TourListingViewModel FeatureTours2 { get; set; }
        public TourSummaryInfoViewModel ExpertFavouriteTour { get; set; }
        public ArticleListingViewModel TravelBlogsComponent { get; set; }
        
        public IEnumerable<ExperienceSummaryViewModel> Experiences { get; set; }
        public string TourIndexName { get; set; }
        public List<MenuGroup> DestinationsMenu { get; set; }
        
        public List<FAQViewModel> FAQs { get; set; }

        public string FAQJsonSchema { get; set; }
        public string SearchAppId { get; set; }
        public string SearchApiKey { get; set; }
        public string SearchIndexName { get; set; }
    }
}