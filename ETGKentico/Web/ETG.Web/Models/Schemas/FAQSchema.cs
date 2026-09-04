using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.Schemas
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AcceptedAnswer
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class MainEntity
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("acceptedAnswer")]
        public AcceptedAnswer AcceptedAnswer { get; set; }
    }

    public class FAQSchema
    {
        [JsonProperty("@context")]
        public string Context { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("mainEntity")]
        public List<MainEntity> MainEntity { get; set; }
    }


}