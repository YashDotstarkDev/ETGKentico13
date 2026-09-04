using AutoMapper;
using Devotion.Web.Base.Extensions;
using Devotion.Web.Base.Providers;
using ETG.Core.Http;
using ETG.Data.Article.Models;
using ETG.Data.Article.Repositories;
using ETG.Web.Article.Models;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Controllers.Base;
using ETG.Web.Helpers;
using System.Web;
using System.Web.Mvc;
using ETG.Web.SEO;

namespace ETG.Web.Article
{
    public class ArticleController : PageController<IArticlePageRepository, ArticlePageModel, ArticlePageViewModel>
    {
        private readonly System.Web.HttpRequest _httpRequest;
        private readonly IMapper _mapper;
        private readonly IArticlePageRepository _repository;
        private readonly IArticleJsonSchemaBuilder _articleJsonSchemaBuilder;
        public ArticleController(IMapper mapper,
            IArticlePageRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider,
            IHttpRequest httpRequest,
            IArticleJsonSchemaBuilder articleJsonSchemaBuilder)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _mapper = mapper;
            _repository = repository;
            _articleJsonSchemaBuilder = articleJsonSchemaBuilder;
            _httpRequest = httpRequest.GetRequest();
        }

        protected override void ProcessBeforeReturningView(ArticlePageViewModel viewModel)
        {
            var http = "https://";

            if (!_httpRequest.IsSecureConnection)
            {
                http = "http://";

            }

            viewModel.JsonSchema =
                _articleJsonSchemaBuilder.BuildJsonSchema(viewModel.Article, $"{http}{_httpRequest.Url.Host}");
            
            if (viewModel?.Article != null)
            {
                PageHelper.InitializePageBuilder(HttpContext, viewModel.Article.DocumentID);
            }
        }

        [HandleError]
        public ActionResult Landing()
        {
            var currentUrl = _httpRequest.Url.PathAndQuery.GetUrlPathOnly();

            var currentCategory = "all";
            var arr = currentUrl.Split('/');

            if (arr.Length == 3)
            {
                currentCategory = arr[2].ToLower();
            }
            var model = _repository.GetLandingPage(currentUrl, currentCategory);


            var viewModel = _mapper.Map<ArticleLandingPageViewModel>(model);

            if (viewModel == null)
            {
                throw new HttpException(404, "Page not found");
                
            }

            return View("Landing", viewModel);
        }
    }
}