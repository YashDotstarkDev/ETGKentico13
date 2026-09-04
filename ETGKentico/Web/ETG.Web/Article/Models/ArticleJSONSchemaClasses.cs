using Newtonsoft.Json;

namespace ETG.Web.Article.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class MainEntityOfPage
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@id")]
        public string Id { get; set; }
    }

    public class Author
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Logo
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Publisher
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logo")]
        public Logo Logo { get; set; }
    }

    public class ArticlJSONSchemaRoot
    {
        [JsonProperty("@context")]
        public string Context { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("mainEntityOfPage")]
        public MainEntityOfPage MainEntityOfPage { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("publisher")]
        public Publisher Publisher { get; set; }

        [JsonProperty("datePublished")]
        public string DatePublished { get; set; }

        [JsonProperty("dateModified")]
        public string DateModified { get; set; }
    }

}