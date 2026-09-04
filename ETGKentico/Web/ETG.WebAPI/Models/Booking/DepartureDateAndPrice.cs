using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking
{
    public class DepartureDateAndPrice
    {
        public string GroupHeading => DepartureDate.ToString("MMMM yyyy");
        public DateTime DepartureDate { get; set; }
        public string CurrencySymbol { get; set; }
        public string DepartureDateAndPriceDisplay => $"{DepartureDate:ddd d MMM yyyy} - {CurrencySymbol}{Price:#,###}";
        public string DepartureFormattedDate => DepartureDate.ToString("dd/MM/yyyy");
        public double Price { get; set; }
        public bool HasPrePostNightPrice { get; set; }
    }
}