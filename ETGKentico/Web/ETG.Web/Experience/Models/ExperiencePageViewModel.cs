using ETG.Web.Article.Models;
using ETG.Web.Models;
using ETG.Web.Models.Common;
using ETG.Web.Models.PageTypes;
using ETG.Web.Tour.Models;
using System.Collections.Generic;
using ETG.Web.Models.Base;

namespace ETG.Web.Experience.Models
{
    public class ExperiencePageViewModel : BasePageViewModel, IViewModel
    {
        public PageHeroViewModel Hero { get; set; }
        public ExperienceViewModel Page { get; set; }
        public List<ArticleViewModel> RelatedArticles { get; set; }
        public TourListingViewModel FeatureTours { get; set; }
        
        public ConnectWithUsViewModel ConnectWithUs { get; set; }
    }
}
