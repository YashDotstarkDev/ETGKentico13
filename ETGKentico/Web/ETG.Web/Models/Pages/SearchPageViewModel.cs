using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.Models.Pages
{
    public class SearchPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public string TourIndexName { get; set; }

        public string ArticleIndexName { get; set; }

        public string ContentsIndexName { get; set; }
        public Dictionary<string,string> TourTypes { get; set; }

        public Dictionary<string, string> CruiseTypes { get; set; }

        public Dictionary<string, string> Experiences { get; set; }
        public List<IconSVGViewModel> PriceInclusions { get; set; }
    }
}
