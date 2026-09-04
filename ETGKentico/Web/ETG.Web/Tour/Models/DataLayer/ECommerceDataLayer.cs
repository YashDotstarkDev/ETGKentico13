using Newtonsoft.Json;

namespace ETG.Web.Tour.Models.DataLayer
{
    public class Event
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class Ecommerce
    {
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("book_now")]
        public Event BookNow { get; set; }
    }

    public class EcommerceDataLayerRoot
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("ecommerce")]
        public Ecommerce Ecommerce { get; set; }
    }

}