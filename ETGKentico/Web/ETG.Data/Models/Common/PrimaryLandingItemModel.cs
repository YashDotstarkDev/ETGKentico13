using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class PrimaryLandingItemModel :IDataModel
    {
        public string Heading { get; set; }
        public string Image { get; set; }
        public string Summary { get; set; }
        public string Path { get; set; }
    }
}
