using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class PricingItemModel : IDataModel
    {
        public string FlighClass { get; set; }
        public  string DepartureCity { get; set; }
        public string TrainClass { get; set; }

        public int Price { get; set; }
        public int PriceInAUD { get; set; }
    }
}
