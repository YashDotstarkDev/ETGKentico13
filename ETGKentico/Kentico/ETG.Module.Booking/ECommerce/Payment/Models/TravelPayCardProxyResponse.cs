using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class Pricing
    {
        [JsonProperty("customerFee")]
        public int CustomerFee { get; set; }

        [JsonProperty("merchantFee")]
        public int MerchantFee { get; set; }

        [JsonProperty("processingAmount")]
        public int ProcessingAmount { get; set; }

        [JsonProperty("paymentAmount")]
        public int PaymentAmount { get; set; }
    }

    public class TravelPayCardProxyResponse
    {
        [JsonProperty("cardProxy")]
        public string CardProxy { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("expiry")]
        public string Expiry { get; set; }

        [JsonProperty("cardType")]
        public string CardType { get; set; }

        [JsonProperty("failureCode")]
        public string FailureCode { get; set; }

        [JsonProperty("failureReason")]
        public string FailureReason { get; set; }

        [JsonProperty("pricing")]
        public Pricing Pricing { get; set; }
    }
}
