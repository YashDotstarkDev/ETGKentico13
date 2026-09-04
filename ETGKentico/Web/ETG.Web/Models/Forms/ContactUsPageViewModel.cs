using System.Web.Mvc;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.Models.Forms
{
    public class ContactUsPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public ContactUsFormViewModel Form { get; set; }
        public GenericSideContactViewModel ContactDetails { get; set; }
    }
}