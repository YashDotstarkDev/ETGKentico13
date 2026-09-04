using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace ETG.Data.Search.Models
{
    public class TypeAheadSearchResult
    {
        public List<CategorySearchResult> CategoryResults { get; set; }

        public bool HasResult
        {
            get
            {
                if (CategoryResults.IsNullOrEmpty())
                {
                    return false;
                }

                return CategoryResults.Any(a => !a.Results.IsNullOrEmpty());
            }
        }

        public void AddCategory(string category, string viewAllUrl)
        {
            if (CategoryResults == null)
            {
                CategoryResults = new List<CategorySearchResult>();
            }

            CategoryResults.Add(new CategorySearchResult(category, viewAllUrl));
        }
        public CategorySearchResult GetCategoryResult(string category)
        {
            if (CategoryResults.IsNullOrEmpty())
            {
                return null;
            }

            return CategoryResults.FirstOrDefault(a => a.Category?.ToLower() == category.ToLower());
        }
    }
}
