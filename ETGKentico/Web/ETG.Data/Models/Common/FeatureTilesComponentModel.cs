using Devotion.Automapper.Common;
using System.Collections.Generic;

namespace ETG.Data.Models.Common
{
    public class FeatureTilesComponentModel : IDataModel
    {
        public string Title { get; set; }
        public List<FeatureTileModel> FeatureTiles { get; set; }
    }
}