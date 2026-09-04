using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.Forms;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Models.Pages
{
    public class FAQsPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public GenericSideContactModel ContactDetails { get; set; }
    }
}