using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ETG.Module.Booking.HotelBuilder.Models
{
    public class HotelEntity
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string ParentName { get; set; }
    }

    public class HotelItem
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }

    public class HotelSearchResult
    {
        [JsonProperty("hotels")]
        public List<HotelItem> Hotels { get; set; }
    }


}
