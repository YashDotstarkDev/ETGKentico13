using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Requests
{
    public class SubmitExtrasRequest
    {
        [JsonProperty(PropertyName = "optionalextras")]
        public string SelectNow { get; set; }
        public string extrasOption1 { get; set; }
        public string extrasOption2 { get; set; }
        public string extrasOption3 { get; set; }
        public string extrasOption4 { get; set; }
        public string extrasOption5 { get; set; }
        public string extrasOption6 { get; set; }
        public string extrasOption7 { get; set; }
        public string extrasOption8 { get; set; }
        public string extrasOption9 { get; set; }
        public string extrasOption10 { get; set; }
    }
}