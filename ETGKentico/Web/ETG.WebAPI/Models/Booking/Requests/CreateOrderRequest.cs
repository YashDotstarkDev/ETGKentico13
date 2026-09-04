using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Requests
{
    public class CreateOrderRequest
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "middlename")]
        public string MiddleName { get; set; }
        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "dob")]
        public string DateOfBirth { get; set; }
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "passenger2title")]
        public string Passenger2Title { get; set; }
        [JsonProperty(PropertyName = "passenger2firstname")]
        public string Passenger2FirstName { get; set; }
        [JsonProperty(PropertyName = "passenger2middlename")]
        public string Passenger2MiddleName { get; set; }
        [JsonProperty(PropertyName = "passenger2lastname")]
        public string Passenger2LastName { get; set; }
        [JsonProperty(PropertyName = "passenger2dob")]
        public string Passenger2DateOfBirth { get; set; }
        [JsonProperty(PropertyName = "subscribe")]
        public string Subscribe { get; set; }
        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "orderid")]
        public string OrderId { get; set; }
        [JsonProperty(PropertyName = "quoteid")]
        public string QuoteId { get; set; }
        [JsonProperty(PropertyName = "createquote")]
        public string CreateQuoteButton { get; set; }
        [JsonProperty(PropertyName = "createorder")]
        public string CreateOrderButton { get; set; }
        [JsonProperty(PropertyName = "agentprice")]
        public string AgentPrice { get; set; }
    }
}