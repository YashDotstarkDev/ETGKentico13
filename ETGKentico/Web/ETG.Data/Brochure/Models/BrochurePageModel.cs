using Devotion.Automapper.Common;
using ETG.Data.Models.PageTypes;
using System.Collections.Generic;
using ETG.Data.Models.Base;

namespace ETG.Data.Brochure.Models
{
    public class BrochurePageModel : BasePageModel, IDataModel
    {
        public BrochureModel Brochure { get; set; }
    }
}
