using ETG.Data.Search;
using ETG.WebAPI.Models.SearchTypeAhead;
using ETG.WebAPI.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("search")]
    public class SearchController : ApiController
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [Route("typeahead")]
        public async Task<IHttpActionResult> TypeAheadSearch(string keywords)
        {
            var results = _searchService.TypeAheadSearch(keywords);

            var searchTypeAheadResult = new SearchTypeAheadResult
            {
                all = new All
                {
                    title = "View all results",
                    url = $"/search?keywords={keywords}"
                },
                results = new Results()
            };

            if (results == null || !results.HasResult)
            {
                return Ok(searchTypeAheadResult);
            }

            var tourResult = results.GetCategoryResult(SearchCategory.Tour);

            if (tourResult != null)
            {
                searchTypeAheadResult.results.tours = new Tours
                {
                    name = SearchCategory.Tour,
                    links = new List<Link>
                    {
                        new Link {title = "View all", url = tourResult.CategoryViewAllUrl}
                    },
                    results = tourResult.Results.Select(a => new Link { title = a.Key, url = a.Value }).ToList()
                };
            }

            var destinationResult = results.GetCategoryResult(SearchCategory.Destinations);

            if (destinationResult != null)
            {
                searchTypeAheadResult.results.destinations = new Destinations
                {
                    name = SearchCategory.Destinations,
                    links = new List<Link>
                    {
                        new Link {title = "View all", url = destinationResult.CategoryViewAllUrl}
                    },
                    results = destinationResult.Results.Select(a => new Link { title = a.Key, url = a.Value }).ToList()
                };
            }

            var inspirationResult = results.GetCategoryResult(SearchCategory.Inspiration);

            if (inspirationResult != null)
            {
                searchTypeAheadResult.results.inspired = new Inspired
                {
                    name = SearchCategory.Inspiration,
                    links = new List<Link>
                    {
                        new Link {title = "View all", url = inspirationResult.CategoryViewAllUrl}
                    },
                    results = inspirationResult.Results.Select(a => new Link { title = a.Key, url = a.Value }).ToList()
                };
            }


            return Ok(searchTypeAheadResult);
        }
    }
}
