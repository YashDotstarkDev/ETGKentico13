using ETG.Data.Career.Models;
using ETG.Web.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.Career.Models
{
    public class CareerApplyPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }

        public CareerRoleViewModel Role { get; set; }
        public CareerApplyFormViewModel Form { get; set; }
    }
}
