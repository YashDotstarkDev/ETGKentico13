using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Models.Pages
{
    public class PrimaryLandingPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }

        public List<PrimaryLandingItemModel> Items {get;set;}
    }
}
