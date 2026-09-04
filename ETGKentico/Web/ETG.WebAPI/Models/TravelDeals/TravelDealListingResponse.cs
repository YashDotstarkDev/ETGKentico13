using ETG.Data.Article.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Data.Models.Base;

namespace ETG.WebAPI.Models.TravelDeals
{
    public class TravelDealListingResponse : BaseResponse
    {
        public SearchResults<TravelDealAPIModel> SearchResult { get; set; }
    }
}