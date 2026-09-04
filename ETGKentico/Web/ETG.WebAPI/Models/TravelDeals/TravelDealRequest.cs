using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.TravelDeals
{
    public class TravelDealRequest
    {
        public string FilterDestination { get; set; }
        public string FilterExperience { get; set; }
        public string FilterDuration { get; set; }
        public int FilterPriceMin { get; set; }
        public int FilterPriceMax { get; set; }

        public int PageNumber { get; set; }

        public int ItemPerPage { get; set; }
    }
}