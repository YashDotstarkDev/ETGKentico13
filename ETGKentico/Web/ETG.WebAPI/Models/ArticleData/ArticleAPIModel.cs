using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.WebAPI.Models.ArticleData
{
    public class ArticleAPIModel
    {
        public string Title
        {
            get; set;
        }
        public  string Image { get; set; }
        public  string Date { get; set; }
        public string Categories { get; set; }
        public  string Url { get; set; }
        public string Destination { get; set; }
    }
}