using Castle.Core.Internal;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Module.Booking.Models.Cart
{
    public class RoomOptionsSummaryCartData
    {
        public RoomOptionsSummaryCartData()
        {
            RoomOptions = new List<RoomOptionCartItem>();
        }
        public List<RoomOptionCartItem> RoomOptions { get; set; }
        public double RoomOptionsSubTotalPrice { get
        {
            if (RoomOptions.IsNullOrEmpty())
            {
                return 0;
            }

            return RoomOptions.Where(a=> a!= null).Select(a => a.RoomOptionTotalPrice).Sum();
        }}
    }
}
