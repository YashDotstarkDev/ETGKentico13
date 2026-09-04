using ETG.Data.Article.Models;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Models;
using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Core.PageTypes;
using ETG.Data.Brochure.Models;

namespace ETG.Data.Destination.Models
{
    public class DestinationPageModel : BasePageModel, IDataModel
    {
        public PageHeroModel Hero { get; set; }
        public DestinationModel Destination { get; set; }
        public DestinationSummaryModel MainDestination { get; set; }
        public List<ArticleModel> RelatedArticles { get; set; }
        public List<AccordionItemModel> HelpfulInformation { get; set; }
        public DestinationExpertTeamSummaryModel DestinationExpert { get; set; }
        public List<MapItemModel> RegionMapItems { get; set; }
        public List<RegionMapItemModel> MainDestinationMapItems { get; set; }
        public TourListingModel FeatureTours { get; set; }
        public BrochureModel Brochure { get; set; }
        public FeatureTilesComponentModel FeatureTilesComponent { get; set; }
        public List<QuickLinkItemModel> QuickLinkItems { get; set; }
        public DestinationStickyModel DestinationStickyModel { get; set; }
    }
}
