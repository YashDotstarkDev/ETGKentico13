using System.Collections.Generic;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Reviews
{
    public class ReviewListingResponse
    {
        public List<ReviewItemAPIModel> Reviews { get; set; }
        [JsonProperty("totals")]
        public ReviewTotalsModel Totals { get; set; }
        [JsonProperty("place_details")]
        public PlaceDetailsModel PlaceDetails { get; set; }
    }
}