using ETG.Web.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.PageTypes;
using System.Collections.Generic;

namespace ETG.Web.Career.Models
{
    public class CareerPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public List<CareerRoleBasicInfoViewModel> Roles { get; set; }
    }
}
