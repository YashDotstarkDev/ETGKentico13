using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Models.Forms
{
    public class ContactUsPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public ContactUsFormModel Form { get; set; }
        public GenericSideContactModel ContactDetails { get; set; }
    }
}