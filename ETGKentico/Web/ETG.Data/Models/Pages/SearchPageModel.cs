using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Models.Pages
{
    public class SearchPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public string TourIndexName { get; set; }

        public string ArticleIndexName { get; set; }

        public string ContentsIndexName { get; set; }
        public Dictionary<string,string> TourTypes { get; set; }

        public Dictionary<string, string> CruiseTypes { get; set; }

        public Dictionary<string, string> Experiences { get; set; }
        public List<IconSVGModel> PriceInclusions { get; set; }
    }
}
