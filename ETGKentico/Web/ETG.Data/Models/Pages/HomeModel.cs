using ETG.Data.Article.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;
using ETG.Data.Tour.Models;
using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Experience.Models;

namespace ETG.Data.Models.Pages
{
    public class HomeModel : IDataModel
    {
        public HomepageModel Page { get; set; }
        public List<ImageModel> HeroSliderImages { get; set; }
        public ProofPointComponentModel ProofPointsComponent { get; set; }
        public DestinationComponentModel DestinationComponent { get; set; }
        public TourListingModel FeatureTours { get; set; }
        public TourListingModel FeatureTours2 { get; set; }
        public TourSummaryInfoModel ExpertFavouriteTour { get; set; }
        public ArticleListingModel TravelBlogsComponent { get; set; }
        public IEnumerable<ExperienceSummaryModel> Experiences { get; set; }
        public string TourIndexName { get; set; }
        public List<FAQModel> FAQs { get; set; }
    }
}