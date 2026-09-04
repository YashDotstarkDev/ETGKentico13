using ETG.Web.Article.Models;
using ETG.Web.Models.Common;
using ETG.Web.Tour.Models;
using System.Collections.Generic;
using ETG.Web.Models.Base;

namespace ETG.Web.Models.Pages
{
    public class PageNotFoundPageViewModel : BasePageViewModel, IViewModel
    {

        public ContactViewModel Contact { get; set; }
        public TourListingViewModel Tours { get; set; }
        public List<ArticleViewModel> Articles { get; set; }
    }
}
