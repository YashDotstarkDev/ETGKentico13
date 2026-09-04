using Castle.Core.Internal;
using ETG.Web.Models;
using ETG.Web.Models.PageTypes;
using System.Collections.Generic;
using System.Linq;
using ETG.Web.Models.Base;

namespace ETG.Web.Brochure.Models
{
    public class BrochureOrderPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public BrochureOrderFormViewModel Form { get; set; }
        

    }
}
