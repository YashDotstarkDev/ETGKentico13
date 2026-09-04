using ETG.Web.Models;
using ETG.Web.Models.Common;
using ETG.Web.Models.PageTypes;
using System.Collections.Generic;
using ETG.Web.Models.Base;

namespace ETG.Web.Article.Models
{
    public class ArticleLandingPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public string ArticleIndexName { get; set; }
    }
}
