using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace ETG.Module.Booking.Models.HotelBuilder
{
    public class HotelBuilderRequest
    {
        public string TourCode { get; set; }
        public string BookingNumber { get; set; }
        public string TravelStartDate { get; set; }
        public List<string> HotelIds { get; set; }
        public List<string> Nights { get; set; }

        public string HotelIdsCommaSeparated
        {
            get
            {
                if (HotelIds.IsNullOrEmpty())
                {
                    return string.Empty;

                }

                return string.Join(",", HotelIds);
            }
        }

        public string NightsCommaSeparated
        {
            get
            {
                if (Nights.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                return string.Join(",", Nights);
            }
        }
    }
}
