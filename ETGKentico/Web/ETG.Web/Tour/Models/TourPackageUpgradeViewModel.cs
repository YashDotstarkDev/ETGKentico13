using ETG.Web.Models;

namespace ETG.Web.Tour.Models
{
    public class TourPackageUpgradeViewModel : IViewModel
    {
        public string UpgradeLabel { get; set; }
        public string IconClass { get; set; }
        public TourSummaryInfoViewModel Tour { get; set; }
    }
}