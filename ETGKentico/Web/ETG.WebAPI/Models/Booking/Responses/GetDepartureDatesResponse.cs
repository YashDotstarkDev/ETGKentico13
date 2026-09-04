using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class GetDepartureDatesResponse: BaseResponse
    {
        public string CurrencySymbol { get; set; }
        
        public int DepartureDatesOption { get; set; }
        
        public DateTime DepartureEarliestDate { get; set; }
        
        public DateTime DepartureLatestDate { get; set; }

        [JsonProperty(PropertyName = "datesAndPrices")]
        public List<DepartureDateAndPrice> DepartureDateAndPriceList { get; set; }
        public bool HasPeaceOfMind { get; set; }
        public bool HasFreedomOfChoice { get; set; }
    } 
} 