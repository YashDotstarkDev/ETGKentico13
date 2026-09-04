using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Forms
{
    public class FormObjectSet
    {
        [JsonProperty("actions")]
        public List<FormAction> Actions { get; set; }
    }
}