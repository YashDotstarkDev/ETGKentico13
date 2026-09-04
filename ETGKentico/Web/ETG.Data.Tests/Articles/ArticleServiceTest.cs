using CMS.Tests;
using ETG.Data.Article.Models;
using ETG.Data.Article.Repositories;
using ETG.Data.Article.Services;
using ETG.Data.Repositories.Modules;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ETG.Data.Cache;

namespace ETG.Data.Tests.Articles
{
    [TestFixture]
    public class ArticleServiceTest : UnitTests
    {
        private IArticleService service;
        private ICacheService cacheService;
        private IArticleCategoryRepository articleCategoryRepository;
        private IArticleRepository articleRepository;
        [SetUp]
        public void Setup()
        {
            articleRepository = Substitute.For<IArticleRepository>();
            cacheService = Substitute.For<ICacheService>();
            articleCategoryRepository = Substitute.For<IArticleCategoryRepository>();
        }

        [Test]
        public void GetRelatedArticlesByDestinationTest_ArticleFound_ShouldReturnResult()
        {

            cacheService.GetDocumentDependentOnAll(Arg.Any<Func<List<ArticleModel>>>(),
                    Arg.Any<string>(), Arg.Any<string>())
                .Returns(new List<ArticleModel> { new ArticleModel() });
            service = new ArticleService(articleRepository, articleCategoryRepository, cacheService);
            var result = service.GetRelatedArticlesByDestination(Guid.NewGuid());
            Assert.IsNotNull(result);
        }


        [Test]
        public void GetRelatedArticlesByExperienceTest_ArticleFound_ShouldReturnResult()
        {

            cacheService.GetDocumentDependentOnAll(Arg.Any<Func<List<ArticleModel>>>(),
                    Arg.Any<string>(), Arg.Any<string>())
                .Returns(new List<ArticleModel> { new ArticleModel() });
            service = new ArticleService(articleRepository, articleCategoryRepository, cacheService);
            var result = service.GetRelatedArticlesByExperience(Guid.NewGuid());
            Assert.IsNotNull(result);
        }
    }
}
