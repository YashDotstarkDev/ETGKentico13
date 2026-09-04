using ETG.Module.Booking.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Requests
{
    public class SubmitFreedomOfChoiceStepRequest
    {
        [JsonProperty(PropertyName = "freedomofchoice")]
        public string SelectNow { get; set; }

        public string dayGroup1 { get; set; }
        public string dayGroup2 { get; set; }
        public string dayGroup3 { get; set; }
        public string dayGroup4 { get; set; }
        public string dayGroup5 { get; set; }
        public string dayGroup6 { get; set; }
        public string dayGroup7 { get; set; }
        public string dayGroup8 { get; set; }
        public string dayGroup9 { get; set; }
        public string dayGroup10 { get; set; }
        public string dayGroup11 { get; set; }
        public string dayGroup12 { get; set; }
        public string dayGroup13 { get; set; }
        public string dayGroup14 { get; set; }
    }
}