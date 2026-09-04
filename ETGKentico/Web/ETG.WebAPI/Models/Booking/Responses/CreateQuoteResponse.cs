using ETG.Data.Extensions;
using ETG.Data.Models.eCommerce;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class CreateQuoteResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "quoteid")]
        public int QuoteID { get; set; }
        
    }
}