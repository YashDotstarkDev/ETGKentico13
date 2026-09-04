using Devotion.Automapper.Common;
using ETG.Data.Models.Common;
using System.Collections.Generic;

namespace ETG.Data.Models.Base
{
    public class BasePageModel 
    {
        public virtual List<SimpleLinkModel> BreadCrumbs { get; set; }
    }
}