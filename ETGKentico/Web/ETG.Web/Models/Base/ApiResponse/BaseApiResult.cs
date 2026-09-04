using Newtonsoft.Json;

namespace ETG.Web.Models.Base.ApiResponse
{
    public class BaseApiResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "delay")]
        public int Delay { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "speed")]
        public int Speed { get; set; }
    }
}