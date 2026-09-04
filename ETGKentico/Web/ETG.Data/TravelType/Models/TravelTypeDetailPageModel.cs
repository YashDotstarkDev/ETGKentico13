using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Article.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.TravelType.Models
{
    public class TravelTypeDetailPageModel: BasePageModel, IDataModel
    {
        public PageHeroModel Hero { get; set; }
        public TravelTypeDetailModel Detail { get; set; }
        public ThemedPackagesListingModel ThemedPackagesInfo { get; set; }
        public ArticleListingModel RelatedArticles { get; set; }

    }
}