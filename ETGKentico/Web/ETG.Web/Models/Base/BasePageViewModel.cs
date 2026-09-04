using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Web.Models.Common;
using ETG.Web.Models.Menu;
using Newtonsoft.Json;

namespace ETG.Web.Models.Base
{
    public class BasePageViewModel 
    {
        [JsonProperty("g_recaptcha_response")]
        public string g_recaptcha_response { get; set; }
        public virtual List<SimpleLinkViewModel> BreadCrumbs { get; set; }
    }
}