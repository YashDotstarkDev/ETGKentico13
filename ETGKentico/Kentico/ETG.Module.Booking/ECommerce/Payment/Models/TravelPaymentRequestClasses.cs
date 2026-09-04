using Newtonsoft.Json;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    
    public class TravelPayPaymentRequest
    {
        [JsonProperty("customerReference")]
        public string CustomerReference { get; set; }

        [JsonProperty("paymentAmount")]
        public double PaymentAmount { get; set; }

        [JsonProperty("cardProxy")]
        public string CardProxy { get; set; }

        [JsonProperty("paymentAccountProxy")]
        public PaymentAccountProxy PaymentAccountProxy { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("contactNumber")]
        public string ContactNumber { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("additionalReference")]
        public string AdditionalReference { get; set; }

        [JsonProperty("abn")]
        public string Abn { get; set; }

        [JsonProperty("merchantUniquePaymentId")]
        public string MerchantUniquePaymentId { get; set; }

        [JsonProperty("sendPaymentConfirmation")]
        public bool SendPaymentConfirmation { get; set; }

        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty("oneOffPaymentReference")]
        public string OneOffPaymentReference { get; set; }

        [JsonProperty("overrideFeePayer")]
        public int OverrideFeePayer { get; set; }
    }


}
