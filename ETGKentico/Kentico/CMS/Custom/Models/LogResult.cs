using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMSApp.Custom.Models;

namespace ETG.Module.Booking.Models
{
    public class LogResult
    {
        public int LineNumber { get; set; }
        public string TourCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public BookingPriceImportModel LineData { get; set; }
    }
}
