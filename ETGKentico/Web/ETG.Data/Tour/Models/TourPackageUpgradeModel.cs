using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class TourPackageUpgradeModel : IDataModel
    {
        public string UpgradeLabel { get; set; }
        public string IconClass { get; set; }
        
        public TourSummaryInfoModel Tour { get; set; }
    }
}