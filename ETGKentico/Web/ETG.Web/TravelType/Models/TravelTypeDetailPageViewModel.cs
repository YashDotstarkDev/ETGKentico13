using System.Collections.Generic;
using ETG.Data.Models.PageTypes;
using ETG.Web.Article.Models;
using ETG.Web.Brochure.Models;
using ETG.Web.Destination.Models;
using ETG.Web.DestinationExpertTeam.Models;
using ETG.Web.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Tour.Models;

namespace ETG.Web.TravelType.Models
{
    public class TravelTypeDetailPageViewModel: BasePageViewModel, IViewModel
    {
        public PageHeroViewModel Hero { get; set; }
        public TravelTypeDetailViewModel Detail { get; set; }
        public ThemedPackagesListingViewModel ThemedPackagesInfo { get; set; }
        
        public ConnectWithUsViewModel ConnectWithUs { get; set; }

        public ArticleListingViewModel RelatedArticles { get; set; }
    }
}
