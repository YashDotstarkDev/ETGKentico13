using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class PaymentAccountProxy
    {
        [JsonProperty("proxy")]
        public string Proxy { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
