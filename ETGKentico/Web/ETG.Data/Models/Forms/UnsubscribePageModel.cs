using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Models.Forms
{
    public class UnsubscribePageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
    }
}
