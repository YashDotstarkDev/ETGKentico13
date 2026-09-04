using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.Common
{
    public class FeatureTilesComponentViewModel : IViewModel
    {
        public string Title { get; set; }
        public List<FeatureTileViewModel> FeatureTiles { get; set; }
    }
}