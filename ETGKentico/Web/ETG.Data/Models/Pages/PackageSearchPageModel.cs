using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Models.Pages
{
    public class PackageSearchPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public string TourIndexName { get; set; }
    }
}
