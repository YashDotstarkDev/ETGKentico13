using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Search.Models
{
    public class CategorySearchResult
    {
        public CategorySearchResult()
        {

        }
        public CategorySearchResult(string category, string viewAllUrl)
        {
            Category = category;
            CategoryViewAllUrl = viewAllUrl;
            Results = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>();
        }
        public string Category { get; set; }
        public string CategoryViewAllUrl { get; set; }
        public List<KeyValuePair<string, string>> Results { get; set; }
    }
}
