using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.WebAPI.Models.TravelDeals
{ 
    public class TravelDealAPIModel
    {
        public string Title
        {
            get; set;
        }
        public  string Image { get; set; }
        public  string Description { get; set; }
        public string Categories { get; set; }
        public  string Url { get; set; }
    }
}