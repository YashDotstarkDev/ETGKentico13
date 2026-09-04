using AutoMapper;
using ETG.Data.Article.Models;
using ETG.Data.Article.Repositories;
using ETG.WebAPI.Models;
using ETG.WebAPI.Models.ArticleData;
using ETG.WebAPI.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ETG.Data.Models.Base;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("travel-deals")]
    public class TravelDealsDataController : ApiController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        public TravelDealsDataController(IMapper mapper, IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        [Route("listing")]
        [HttpPost]
        public async Task<IHttpActionResult> GetArticleListing(ArticleListingRequest request)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (request == null)
            {
                return Ok(baseResponse);
            }

            var filter = _mapper.Map<ArticleLandingFilter>(request);
            var result = _articleRepository.GetArticlesAsync(filter);

            await Task.FromResult(result);

            var response = new ArticleListingResponse
            {
                Success = true,
                SearchResult = _mapper.Map<SearchResults<ArticleAPIModel>>(result.Result)
            };
            return Ok(response);
        }
    }
}