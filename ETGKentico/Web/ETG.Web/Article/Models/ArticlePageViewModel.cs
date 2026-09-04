using ETG.Web.Models;
using System.Collections.Generic;
using ETG.Web.Models.Base;
using ETG.Web.Tour.Models;

namespace ETG.Web.Article.Models
{
    public class ArticlePageViewModel: BasePageViewModel, IViewModel
    {
        public ArticleViewModel Article { get; set; }
        public string JsonSchema { get; set; }
        public TourListingViewModel FeatureTours { get; set; }
        public List<ArticleViewModel> RelatedArticles { get; set; }
    }
}
