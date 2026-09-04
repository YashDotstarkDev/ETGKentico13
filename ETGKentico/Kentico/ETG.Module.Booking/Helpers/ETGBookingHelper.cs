using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Module.Booking.Helpers
{
    public static class ETGBookingHelper
    {
        public static bool EntireFlexIsOffered(DateTime departureDate, int flexDaysThreshold)
        {
            return departureDate.AddDays(-(flexDaysThreshold)) >= DateTime.Now.Date;
        }
    }
}
