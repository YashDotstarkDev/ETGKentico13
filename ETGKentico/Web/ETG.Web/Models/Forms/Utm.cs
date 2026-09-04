using Newtonsoft.Json;

namespace ETG.Web.Models.Forms
{
    public class Utm
    {
        [JsonProperty(Required = Required.Default)]
        public string utm_campaign { get; set; }
        
        [JsonProperty(Required = Required.Default)]
        public string utm_content { get; set; }
        
        [JsonProperty(Required = Required.Default)]
        public string utm_medium { get; set; }
        
        [JsonProperty(Required = Required.Default)]
        public string utm_source { get; set; }
        
        [JsonProperty(Required = Required.Default)]
        public string utm_term { get; set; }
    }
}