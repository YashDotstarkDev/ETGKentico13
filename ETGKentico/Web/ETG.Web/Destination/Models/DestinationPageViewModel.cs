using ETG.Web.Article.Models;
using ETG.Web.DestinationExpertTeam.Models;
using ETG.Web.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Tour.Models;
using System.Collections.Generic;
using ETG.Web.Brochure.Models;
using ETG.Data.Destination.Models;

namespace ETG.Web.Destination.Models
{
    public class DestinationPageViewModel : BasePageViewModel, IViewModel
    {
        public PageHeroViewModel Hero { get; set; }
        public DestinationViewModel Destination { get; set; }
        public DestinationSummaryViewModel MainDestination { get; set; }

        public DestinationExpertTeamSummaryViewModel DestinationExpert { get; set; }
        public List<AccordionItemViewModel> HelpfulInformation { get; set; }
        public List<ArticleViewModel> RelatedArticles { get; set; }
        public List<MapItemViewModel> RegionMapItems { get; set; }
        public List<RegionMapItemViewModel> MainDestinationMapItems { get; set; }
        public TourListingViewModel FeatureTours { get; set; }
        public BrochureViewModel Brochure { get; set; }
        public FeatureTilesComponentViewModel FeatureTilesComponent { get; set; }
        public string TourSearchIndex { get; set; }

        public ConnectWithUsViewModel ConnectWithUs { get; set; }

        public List<QuickLinkItemViewModel> QuickLinkItems { get; set; }

        public DestinationStickyModel DestinationStickyModel { get; set; }
    }
}
