using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Requests
{
    public class SubmitOtherOptionsStepRequest
    {
        [JsonProperty(PropertyName = "requireFareAssistance")]
        public bool RequireFareAssistance { get; set; }
        [JsonProperty(PropertyName = "requireInsuranceAssistance")]
        public bool RequireTravelInsuranceAssistance { get; set; }
    }
}