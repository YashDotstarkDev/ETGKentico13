
using Newtonsoft.Json;

namespace ETG.Web.Models.Base.ApiResponse
{
    /// <summary>
    /// Repose type
    /// Message
    /// SetCookie
    /// Redirect
    /// Modal
    /// FbqTrack
    /// GtmTrack
    /// GATrack
    /// </summary>
    /// [Serializable]
    public class JsonResponse: BaseApiResult
    {
        [JsonProperty(PropertyName = "type")]
        public string type { get; set; } 

        [JsonProperty(PropertyName = "target")]
        public string target { get; set; }

        [JsonProperty(PropertyName = "action")]
        public string action { get; set; }

        [JsonProperty(PropertyName = "markup")]
        public string markup { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string uri { get; set; }
        
        [JsonProperty(PropertyName = "content")]
        public string content { get; set; }
    }
}