using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class RegionMapItemModel : IDataModel
    {
        public string ID { get; set;}
        public string RegionName { get; set; }
        public string RegionUrl { get; set; }
    }
}
