using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Models.PageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Web.Models.Pages
{
    public class PrimaryLandingPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public List<PrimaryLandingItemViewModel> Items {get;set;}
    }
}
