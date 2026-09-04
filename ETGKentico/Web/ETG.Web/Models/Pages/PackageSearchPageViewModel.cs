
using System.Collections.Generic;
using ETG.Web.Models.Base;
using ETG.Web.Models.Menu;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.Models.Pages
{
    public class PackageSearchPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public string TourIndexName { get; set; }
        public List<MenuGroup> DestinationMenus { get; set; }
        public bool CurrentCurrencyAppliesDiscount { get; set; }
    }
}
