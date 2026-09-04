using System.Collections.Generic;
using ETG.Data.Tour.Models;
using ETG.Web.Article.Models;
using ETG.Web.Brochure.Models;
using ETG.Web.DestinationExpertTeam.Models;
using ETG.Web.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Tour.Models;

namespace ETG.Web.DestinationExpertTeam.Models
{
    public class DestinationExpertTeamPageViewModel : BasePageViewModel, IViewModel
    {
        public DestinationExpertTeamViewModel DestinationExpert { get; set; }

        public TourSummaryInfoViewModel FavouriteTour { get; set; }
        public TourListingViewModel FeatureTours { get; set; }
        public List<MapItemViewModel> RegionMapItems { get; set; }
        public List<ArticleViewModel> RelatedArticles { get; set; }
        public BrochureViewModel Brochure { get; set; }
    }
}
