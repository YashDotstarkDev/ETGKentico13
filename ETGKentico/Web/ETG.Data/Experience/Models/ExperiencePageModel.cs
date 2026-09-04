using ETG.Data.Article.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Models;
using System.Collections.Generic;
using Devotion.Automapper.Common;

namespace ETG.Data.Experience.Models
{
    public class ExperiencePageModel : BasePageModel, IDataModel
    {
        public PageHeroModel Hero { get; set; }
        public ExperienceModel Page { get; set; }

        public TourListingModel RelatedTours { get; set; }
        public List<ArticleModel> RelatedArticles { get; set; }
        public TourListingModel FeatureTours { get; set; }
    }
}
