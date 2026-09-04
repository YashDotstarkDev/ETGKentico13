using Devotion.Automapper.Common;
using System.Collections.Generic;

namespace ETG.Data.Article.Models
{
    public class ArticleListingModel : IDataModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ArticleModel> Articles { get; set; }
        public string ViewAllUrl { get; set; }
    }
}
