using System.Collections.Generic;
using ETG.Web.Models;

namespace ETG.Web.Article.Models
{
    public class ArticleListingViewModel : IViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ArticleViewModel> Articles { get; set; }
        public string ViewAllUrl { get; set; }
    }
}