using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Article.Models;
using ETG.Data.Brochure.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Models;

namespace ETG.Data.DestinationExpertTeam.Models
{
    public class DestinationExpertTeamPageModel : BasePageModel, IDataModel
    {
        public DestinationExpertTeamModel DestinationExpert { get; set; }
        public TourSummaryInfoModel FavouriteTour { get; set; }
        public List<ArticleModel> RelatedArticles { get; set; }
        public TourListingModel FeatureTours { get; set; }
        public List<MapItemModel> RegionMapItems { get; set; }
        public BrochureModel Brochure { get; set; }

    }
}
