using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Career.Models
{
    public class CareerApplyPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public CareerRoleModel Role { get;set; }
        public CareerApplyFormModel Form { get; set; }
    }
}
