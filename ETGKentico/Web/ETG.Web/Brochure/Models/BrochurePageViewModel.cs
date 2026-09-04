using ETG.Data.Brochure.Models;
using ETG.Data.Models.PageTypes;
using ETG.Web.Models;
using ETG.Web.Models.Base;

namespace ETG.Web.Brochure.Models
{
    public class BrochurePageViewModel : BasePageViewModel, IViewModel
    {
        public BrochureViewModel Brochure { get; set; }
    }
}
