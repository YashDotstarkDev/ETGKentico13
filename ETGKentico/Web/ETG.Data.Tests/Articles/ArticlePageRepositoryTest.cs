using CMS.Tests;
using ETG.Data.Article.Models;
using ETG.Data.Article.Repositories;
using ETG.Data.Article.Services;
using ETG.Data.Cache;
using ETG.Data.Models.Modules;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Modules;
using FizzWare.NBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ETG.Core.Http;
using ETG.Data.Services;
using ETG.Data.Tour.Services;
using System.Web;
using ETG.Data.Repositories.Image;

namespace ETG.Data.Tests.Articles
{
    [TestFixture]
    public class ArticlePageRepositoryTest : UnitTests
    {
        private IArticlePageRepository repository;
        private ICacheService cacheService;
        private IArticleService articleService;
        private IImageRepository imageRepository;
        private IPageItemRepository pageRepository;
        private IArticleCategoryRepository articleCategoryRepository;
        private IArticleRepository articleRepository;
        private ITourService tourService;
        private IShareLinksService shareLinksService;
        private IHttpRequest httpRequest;
        [SetUp]
        public void Setup()
        {
            articleRepository = Substitute.For<IArticleRepository>();
            cacheService = Substitute.For<ICacheService>();
            articleService = Substitute.For<IArticleService>();
            imageRepository = Substitute.For<IImageRepository>();
            pageRepository = Substitute.For<IPageItemRepository>();
            articleCategoryRepository = Substitute.For<IArticleCategoryRepository>();
            tourService = Substitute.For<ITourService>();
            httpRequest = Substitute.For<IHttpRequest>();
            httpRequest.GetRequest().Returns(new HttpRequest("a", "http://test.com/abc", string.Empty));

            shareLinksService = Substitute.For<IShareLinksService>();

        }

        private ArticlePageRepository GetArticlePageRepository()
        {
            return new ArticlePageRepository(articleRepository, articleService, cacheService, imageRepository, pageRepository, articleCategoryRepository, tourService, shareLinksService);

        }

        [Test]
        public void GetTest_ArticleFound_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<Article.Models.ArticleModel>(Arg.Any<Func<ArticleModel>>(), Arg.Any<string>()).Returns(Builder<Article.Models.ArticleModel>.CreateNew().Build());
            repository = GetArticlePageRepository();
            var result = repository.Get("/article");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTest_ArticleNotFound_ShouldReturnNullResult()
        {
            cacheService.GetDocumentDependentOnPath<Article.Models.ArticleModel>(Arg.Any<Func<ArticleModel>>(), Arg.Any<string>()).ReturnsNull();
            repository = GetArticlePageRepository();
            var result = repository.Get("/article");
            Assert.IsNull(result);
        }
        [Test]
        public void GetTest_ArticleFound_HasRelatedArticle_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<Article.Models.ArticleModel>(Arg.Any<Func<ArticleModel>>(), Arg.Any<string>()).Returns(Builder<Article.Models.ArticleModel>.CreateNew().Build());
            articleService.GetRelatedArticles(Arg.Any<ArticleModel>()).Returns(new List<ArticleModel>
            {
                new ArticleModel
                {
                    Title =  "Article title"
                }
            });
            repository = GetArticlePageRepository();
            var result = repository.Get("/article");
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotEmpty(result.RelatedArticles)
            );
        }
        [Test]
        public void GetLandingTest_ArticleCategoriesFound_ShouldReturnResult()
        {
            articleCategoryRepository.GetAllCategories().Returns(Builder<List<ArticleCategoryModel>>.CreateNew().Build());
            //cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());
            repository = GetArticlePageRepository();
            var result = repository.GetLandingPage("/articles", null);
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotEmpty(result.Tabs)
            );
        }

        [Test]
        public void GetLandingTest_ArticleCategoryNotFound_ShouldReturnNullResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();
            repository = GetArticlePageRepository();
            var result = repository.GetLandingPage("/articles", null);

            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotEmpty(result.Tabs)
                );
        }
    }
}
