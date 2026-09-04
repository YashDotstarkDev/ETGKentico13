using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Requests
{
    public class CreatePaymentRequest
    {
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "referenceno")]
        public string InvoiceReference { get; set; }
        [JsonProperty(PropertyName = "paymentamount")]
        public string Amount { get; set; }
        [JsonProperty(PropertyName = "paymentid")]
        public string PaymentId { get; set; }
        
        [JsonProperty(PropertyName = "g_recaptcha_response")]
        public string RecaptchaResponse { get; set; }
    }
}