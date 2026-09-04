using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Reviews
{
    public class ReviewItemAPIModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("reviewer")]
        public string Reviewer { get; set; }

        [JsonProperty("reviewer_avatar")]
        public string ReviewerAvatar { get; set; }

        [JsonProperty("reviewer_id")]
        public string ReviewerId { get; set; }

        [JsonProperty("reviewer_url")]
        public string ReviewerUrl { get; set; }

        [JsonProperty("datetime")]
        public DateTime Datetime { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("likes")]
        public object Likes { get; set; }
        
        [JsonProperty("photos")]
        public List<PhotoModel> Photos { get; set; }
    }

    public class ReviewTotalsModel
    {
        [JsonProperty("average_rating")]
        public decimal AverageRating { get; set; }
        [JsonProperty("review_count")]
        public int ReviewCount { get; set; }
    }

    public class PlaceDetailsModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public class PhotoModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}