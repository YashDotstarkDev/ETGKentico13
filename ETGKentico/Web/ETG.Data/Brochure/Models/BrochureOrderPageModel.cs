using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Brochure.Models
{
    public class BrochureOrderPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public BrochureOrderFormModel Form { get; set; }
    }
}
