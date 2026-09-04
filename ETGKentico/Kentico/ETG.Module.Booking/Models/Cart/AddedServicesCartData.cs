using Castle.Core.Internal;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Module.Booking.Models.Cart
{
    public class AddedServicesCartData
    {
        public AddedServicesCartData()
        {
            Services = new List<ItemBreakdown>();
        }

        public List<ItemBreakdown> Services { get; set; }

        public double TotalPrice
        {
            get
            {
                if (Services.IsNullOrEmpty())
                {
                    return 0;
                }

                return Services.Select(a => a.TotalPrice).Sum();
            }
        }
    }
}
