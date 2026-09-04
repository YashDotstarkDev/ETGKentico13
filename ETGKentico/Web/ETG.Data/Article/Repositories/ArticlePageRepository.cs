using System;
using Castle.Core.Internal;
using ETG.Core.Constants;
using ETG.Data.Article.Models;
using ETG.Data.Article.Services;
using ETG.Data.Cache;
using ETG.Data.Models.Common;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Modules;
using System.Collections.Generic;
using System.Linq;
using ETG.Data.Repositories.Image;
using ETG.Data.Search;
using ETG.Data.Services;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Services;

namespace ETG.Data.Article.Repositories
{
    public class ArticlePageRepository : BasePageRepository, IArticlePageRepository
    {
        private readonly IPageItemRepository _pageRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ICacheService _cacheService;
        private readonly IArticleService _articleService;
        private readonly ISearchConfiguration _searchConfiguration;
        private readonly ITourService _tourService;
        public ArticlePageRepository(IArticleRepository articleRepository, IArticleService articleService,
           ICacheService cacheService, IImageRepository imageRepository, IPageItemRepository pageRepository,
          ITourService tourService, IShareLinksService shareLinksService, ISearchConfiguration searchConfiguration) : base(imageRepository, cacheService, shareLinksService)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
            _cacheService = cacheService;
            _pageRepository = pageRepository;
            _tourService = tourService;
            _searchConfiguration = searchConfiguration;
        }
        
        public ArticlePageModel Get(string url, string path = "")
        {
            var article = _cacheService.GetDocumentDependentOnPath
                (() => _articleRepository.GetArticleByAlias(path), $"GetArticleByAlias{path}");
            if (article == null)
            {
                return null;
            }

            var viewModel = new ArticlePageModel
            {
                Article = article
            };
            viewModel.Article.GalleryImages = GetGalleryImages(article.PageAliasPath);
            viewModel.RelatedArticles = _articleService.GetRelatedArticles(article);

            if (article.FirstDestinationGuid != Guid.Empty)
            {
                viewModel.FeatureTours = new TourListingModel
                {
                    Tours = _tourService.GetTiledToursByDestination(article.FirstDestinationGuid, 8)

                };
            }
            viewModel.BreadCrumbs = GetBreadCrumbs("Inspirations", "/articles", article.Title);
            return viewModel;
        }

        private string GetArticleLandingUrl(string name = "")
        {
            if (name.IsNullOrEmpty())
            {
                return "/articles";
            }
            return $"/articles/{name.Replace(" ", "-")}".ToLower();
        }

        public ArticleLandingPageModel GetLandingPage(string currentUrl, string currentCategory)
        {
            if (currentUrl.IsNullOrEmpty())
            {
                return null;
            }

            return new ArticleLandingPageModel
            {
                Page = _cacheService.GetDocumentDependentOnPath(
                    () => _pageRepository.GetPage(PathConstants.PATH_ARTICLE_LANDING), "ArticleLanding", PathConstants.PATH_ARTICLE_LANDING),
                BreadCrumbs = GetBreadCrumbs("Inspirations"),
                ArticleIndexName = _searchConfiguration.IndexArticle
            };
        }
    }
}
