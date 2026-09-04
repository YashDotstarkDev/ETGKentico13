using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.ArticleData
{
    public class ArticleListingRequest
    {
        [JsonProperty(PropertyName = "categoryid")]
        public string ArticleCategoryId { get; set; }

        [JsonProperty(PropertyName = "pageNumber")]
        public int pageNumber { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public string pageSize { get; set; }
    }
}