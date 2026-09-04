using ETG.Data.Extensions;
using ETG.Data.Models.eCommerce;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class CreatePaymentResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "paymentid")]
        public int PaymentId { get; set; }

        [JsonProperty(PropertyName = "travelpay")]
        public TravelPayParameters TravelPayParameters { get; set; }
    }
}