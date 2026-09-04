using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Forms
{
    public class FormResponse : BaseResponse
    {
        [JsonProperty("objectSet")]
        public FormObjectSet ObjectSet { get; set; }
    }
}