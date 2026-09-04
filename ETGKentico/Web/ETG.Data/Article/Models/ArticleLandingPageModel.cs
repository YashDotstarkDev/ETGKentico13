using Devotion.Automapper.Common;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;
using System.Collections.Generic;
using ETG.Data.Models.Base;

namespace ETG.Data.Article.Models
{
    public class ArticleLandingPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public string ArticleIndexName { get; set; }
    }
}
