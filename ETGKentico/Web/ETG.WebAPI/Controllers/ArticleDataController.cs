using System.Linq;
using AutoMapper;
using Devotion.Web.Base.Extensions;
using ETG.Data.Article.Models;
using ETG.Data.Article.Repositories;
using ETG.Data.Models.Base;
using ETG.Data.Repositories.Modules;
using ETG.WebAPI.Models;
using ETG.WebAPI.Models.ArticleData;
using ETG.WebAPI.Routing;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Core.Internal;
using System;
using ETG.Core.Extensions;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("article")]
    public class ArticleDataController : ApiController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        public ArticleDataController(IMapper mapper, IArticleRepository articleRepository, IArticleCategoryRepository articleCategoryRepository)
        {
            _articleCategoryRepository = articleCategoryRepository;
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        [Route("listing")]
        public async Task<IHttpActionResult> GetArticleListing(string pageNumber, string pageSize, string category)//ArticleListingRequest request)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (pageSize == null)
            {
                return Ok(baseResponse);
            }

            /*var categoryObject = _articleCategoryRepository.GetArticleCategory(category?.Replace("-", string.Empty));
            var categoryGuid = "";

            if (categoryObject != null)
            {
                categoryGuid = categoryObject.ItemGuid.ToString();
            }*/
            var filter = new ArticleLandingFilter
            {
                PageNumber = pageNumber.ToInteger(),
                ItemPerPage = pageSize.ToInteger(),
                //ArticleCategoryId = categoryGuid

            };
            var result = _articleRepository.GetArticlesAsync(filter);


            await Task.FromResult(result);

            var response = new ArticleListingResponse
            {
                Success = true,
                SearchResult = _mapper.Map<SearchResults<ArticleAPIModel>>(result.Result)
            };

            if (response.SearchResult?.Results != null && !response.SearchResult.Results.IsNullOrEmpty())
            {
                response.SearchResult.Results = response.SearchResult.Results.Select(a => ImgixifyImage(a));
            }

            return Ok(response);
        }

        private ArticleAPIModel ImgixifyImage(ArticleAPIModel model)
        {
            if (model == null)
            {
                return null;
            }

            model.Image = model.Image.Imgixify(290, 163, true);
            return model;
        }
    }
}