using ETG.Web.Models;

namespace ETG.Web.Tour.Models
{
    public class TourRoomUpgradeViewModel : IViewModel
    {
        public string Title { get; set; }
        public string Image { get; set; }

        public string Description
        {
            get; set;
        }
        
        public string PriceStatements{ get; set; }

    }
}