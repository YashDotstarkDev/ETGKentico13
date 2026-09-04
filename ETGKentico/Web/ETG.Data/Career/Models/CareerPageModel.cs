using Devotion.Automapper.Common;
using ETG.Data.Models.PageTypes;
using System.Collections.Generic;
using ETG.Data.Models.Base;

namespace ETG.Data.Career.Models
{
    public class CareerPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public List<CareerRoleBasicInfoModel> Roles { get; set; }
    }
}
