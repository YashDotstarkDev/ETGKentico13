using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.Models.Pages
{
    public class FAQsPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public GenericSideContactViewModel ContactDetails { get; set; }
    }
}