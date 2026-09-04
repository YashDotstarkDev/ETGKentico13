using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Booking.Pricing.Models
{
    public class DepartureDateBookingOptions
    {
        public List<BookingOption> TwinShareRoomOptions { get; set; }
        public List<BookingOption> SingleRoomOptions { get; set; }
        public List<BookingOption> ExtraOptions { get; set; }
    }
}
