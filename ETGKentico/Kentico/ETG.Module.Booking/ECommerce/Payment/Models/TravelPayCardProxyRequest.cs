using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class TravelPayCardProxyRequest
    {
        [JsonProperty("customerUniqueId")]
        public string customerUniqueId { get; set; }

        [JsonProperty("cardNumber")]
        public string cardNumber { get; set; }

        [JsonProperty("expiry")]
        public string expiry { get; set; }

        [JsonProperty("cardHolderName")]
        public string cardHolderName { get; set; }

        [JsonProperty("paymentAmount")]
        public int paymentAmount { get; set; }
    }

}
