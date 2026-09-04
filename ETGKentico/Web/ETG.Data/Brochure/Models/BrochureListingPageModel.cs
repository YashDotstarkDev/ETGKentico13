using Devotion.Automapper.Common;
using ETG.Data.Models.PageTypes;
using System.Collections.Generic;
using ETG.Data.Models.Base;

namespace ETG.Data.Brochure.Models
{
    public class BrochureListingPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }

        public string OrderedBrochureGuids { get; set; }
        public List<BrochureModel> Brochures { get; set; }
    }
}
