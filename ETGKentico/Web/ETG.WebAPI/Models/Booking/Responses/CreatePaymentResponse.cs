using ETG.Data.Extensions;
using ETG.Data.Models.eCommerce;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class CreateOrderResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "orderid")]
        public int OrderId { get; set; }

        [JsonProperty(PropertyName = "travelpay")]
        public TravelPayParameters TravelPayParameters { get; set; }
    }
    
    public class CreateOrderOtherResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "orderid")]
        public int OrderId { get; set; }

        [JsonProperty(PropertyName = "redirecturl")]
        public string RedirectUrl { get; set; }
    }
}