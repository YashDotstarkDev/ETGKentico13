using System;
using System.Collections.Generic;
using System.Linq;
using ETG.Data.Search.Models;
using Algolia.Search;
using Algolia.Search.Clients;
using Algolia.Search.Http;
using Algolia.Search.Models.Search;
using Castle.Core.Internal;
using CMS.EventLog;

namespace ETG.Data.Search
{
    public class AlgoliaSearchService : ISearchService
    {
        private readonly SearchClient _searchClient;
        private readonly ISearchConfiguration _searchConfiguration;
        public AlgoliaSearchService(ISearchConfiguration searchConfiguration)
        {
            _searchConfiguration = searchConfiguration;
            _searchClient = new SearchClient(searchConfiguration.ApplicationId, searchConfiguration.APIKey);
        }

        private CategorySearchResult GetCategoryResult(List<CombinedSearchObject> hits, string searchCategory, string className,
            string searchAllUrl)
        {
            var categoryResult = new CategorySearchResult
            {
                Category = searchCategory,
                CategoryViewAllUrl = searchAllUrl
            };
            categoryResult.Results = hits
                .Where(a => a.classname.ToLower().Equals(className.ToLower())).Take(3)
                .Select(a => new KeyValuePair<string, string>(a.DocumentName, a.Url)).ToList();

            return categoryResult;
        }

        public TypeAheadSearchResult TypeAheadSearch(string keywords)
        {
            try
            {
                var q = new Query(keywords);
                q.HitsPerPage = 500;
                var index = _searchClient.InitIndex(_searchConfiguration.IndexTypeAhead);
                var results = index.Search<CombinedSearchObject>(q);
                var typeAheadResult = new TypeAheadSearchResult
                {
                    CategoryResults = new List<CategorySearchResult>()
                };
                if (results.Hits.IsNullOrEmpty())
                {
                    typeAheadResult.AddCategory(SearchCategory.Tour, $"/search?query={keywords}");
                    typeAheadResult.AddCategory(SearchCategory.Destinations, $"/destinations");
                    typeAheadResult.AddCategory(SearchCategory.Inspiration, $"/articles");
                    return typeAheadResult;
                }


                typeAheadResult.CategoryResults.Add(GetCategoryResult(results.Hits, SearchCategory.Tour,
                    Core.PageTypes.Tour.CLASS_NAME, $"/search?query={keywords}"));
                typeAheadResult.CategoryResults.Add(GetCategoryResult(results.Hits, SearchCategory.Destinations,
                    Core.PageTypes.Destination.CLASS_NAME, $"/destinations"));
                typeAheadResult.CategoryResults.Add(GetCategoryResult(results.Hits, SearchCategory.Inspiration,
                    Core.PageTypes.Article.CLASS_NAME, $"/articles"));
                return typeAheadResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
