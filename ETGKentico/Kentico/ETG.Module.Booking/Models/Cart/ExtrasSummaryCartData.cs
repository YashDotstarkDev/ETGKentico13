using Castle.Core.Internal;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Module.Booking.Models.Cart
{
    public class ExtrasSummaryCartData
    {
        public ExtrasSummaryCartData()
        {
            Extras = new List<RoomOptionCartItem>();
        }

        public bool SelectExtrasNow { get; set; }
        public List<RoomOptionCartItem> Extras { get; set; }
        public double ExtrasSubTotalPrice { get; set; }
    }
}
