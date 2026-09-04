using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ETG.Web.Models.Base.ApiResponse
{
    public class GenericApiResponse
    {
        [JsonProperty(PropertyName = "objectSet")]
        public ObjectSet objectSet { get; set; }
    }

    [Serializable]
    public class ObjectSet
    {
        [JsonProperty(PropertyName = "actions")]
        public List<JsonResponse> actions { get; set; }
    }
}