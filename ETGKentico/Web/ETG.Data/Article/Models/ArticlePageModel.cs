using Devotion.Automapper.Common;
using System.Collections.Generic;
using ETG.Data.Models.Base;
using ETG.Data.Tour.Models;

namespace ETG.Data.Article.Models
{
    public class ArticlePageModel : BasePageModel, IDataModel
    {
        public ArticleModel Article { get; set; }
        public TourListingModel FeatureTours { get; set; }
        public List<ArticleModel> RelatedArticles { get; set; }
    }
}
