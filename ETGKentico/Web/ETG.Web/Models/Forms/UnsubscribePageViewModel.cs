using ETG.Data.Models.Forms;
using ETG.Web.Models.Base;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.Models.Forms
{
    public class UnsubscribePageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public UnsubscribeFromViewModel Form { get; set; } = new UnsubscribeFromViewModel();
    }
}