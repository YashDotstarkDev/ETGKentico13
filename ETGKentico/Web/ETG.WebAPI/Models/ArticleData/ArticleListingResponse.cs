using ETG.Data.Article.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Data.Models.Base;

namespace ETG.WebAPI.Models.ArticleData
{
    public class ArticleListingResponse : BaseResponse
    {
        public SearchResults<ArticleAPIModel> SearchResult { get; set; }
    }
}