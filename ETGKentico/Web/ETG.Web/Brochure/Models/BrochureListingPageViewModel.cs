using Castle.Core.Internal;
using ETG.Web.Models;
using ETG.Web.Models.PageTypes;
using System.Collections.Generic;
using ETG.Web.Models.Base;

namespace ETG.Web.Brochure.Models
{
    public class BrochureListingPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public string OrderedBrochureGuids { get; set; }
        public List<BrochureViewModel> Brochures { get; set; }
        public int OrderedBrochureCount { get
            {
                if (OrderedBrochureGuids.IsNullOrEmpty())
                {
                    return 0;
                }

                return OrderedBrochureGuids.Split(',').Length;
            }
        }
    }
}
