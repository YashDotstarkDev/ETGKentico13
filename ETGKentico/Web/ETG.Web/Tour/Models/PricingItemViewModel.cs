using ETG.Web.Models;

namespace ETG.Web.Tour.Models
{
    public class PricingItemViewModel : IViewModel
    {
        public string FlighClass { get; set; }
        public string DepartureCity { get; set; }
        public string TrainClass { get; set; }

        public int Price { get; set; }
        public int PriceInAUD { get; set; }
    }
}
