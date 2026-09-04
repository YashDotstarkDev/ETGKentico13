using Newtonsoft.Json;

namespace ETG.Algolia.SearchDocumentCreators.SearchDocumentModels
{
    public class TourOptions
    {
        [JsonProperty("bookNow")]
        public bool BookNow { get; set; }

        [JsonProperty("freedomOfChoice")]
        public bool FreedomOfChoice { get; set; }

        [JsonProperty("exclusivePackages")]
        public bool ExclusivePackages { get; set; }

        [JsonProperty("onSale")]
        public bool OnSale { get; set; }

        [JsonProperty("peaceOfMind")]
        public bool PeaceOfMind { get; set; }

        [JsonProperty("safeTravels")]
        public bool SafeTravels { get; set; }
    }
    
    public class TourInclusions
    {
        [JsonProperty("accommodation")]
        public bool Accommodation { get; set; }

        [JsonProperty("meals")]
        public bool Meals { get; set; }

        [JsonProperty("transfer")]
        public bool Transfer { get; set; }

        [JsonProperty("flights")]
        public bool Flights { get; set; }
    }


    public class ObjectIDResult
    {
        public string objectID { get; set; }
    }

}