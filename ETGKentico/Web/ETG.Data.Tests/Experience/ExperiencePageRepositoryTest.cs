using AutoMapper;
using CMS.Tests;
using ETG.Data.Article.Models;
using ETG.Data.Article.Services;
using ETG.Data.Experience.Models;
using ETG.Data.Experience.Repositories;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories;
using FizzWare.NBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ETG.Data.Cache;
using ETG.Data.Models.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;
using ETG.Data.Tour.Services;

namespace ETG.Data.Tests.Experience
{
    [TestFixture]
    public class ExperiencePageRepositoryTest : UnitTests
    {
        private IExperiencePageRepository repository;
        private ICacheService cacheService;
        private IArticleService articleService;
        private IImageRepository imageRepository;
        private IContainerRepository containerRepository;
        private IMapper mapper;
        private IExperienceRepository experienceRepository;
        private ITourService tourService;
        private IShareLinksService shareLinksService;
        [SetUp]
        public void Setup()
        {
            mapper = Substitute.For<IMapper>();
            experienceRepository = Substitute.For<IExperienceRepository>();
            cacheService = Substitute.For<ICacheService>();
            articleService = Substitute.For<IArticleService>();
            imageRepository = Substitute.For<IImageRepository>();
            containerRepository = Substitute.For<IContainerRepository>();
            tourService = Substitute.For<ITourService>();
            shareLinksService = Substitute.For<IShareLinksService>();
        }

        private ExperiencePageRepository GetExperiencePageRepository()
        {
            return new ExperiencePageRepository(containerRepository, experienceRepository, articleService, cacheService, mapper, imageRepository, tourService,shareLinksService);

        }

        [Test]
        public void GetTest_ExperienceNodeFound_ShouldReturnResult()
        {
            var experience = Builder<ExperienceModel>.CreateNew().Build();
            experience.SummaryInfo = Builder<ExperienceSummaryModel>.CreateNew().Build();

            cacheService.GetDocumentDependentOnPath<ExperienceModel>(Arg.Any<Func<ExperienceModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(experience);

            repository = GetExperiencePageRepository();
            var result = repository.Get("/food");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTest_ExperienceNotFound_ShouldReturnNotResult()
        {
            cacheService.GetDocumentDependentOnPath<ExperienceModel>(Arg.Any<Func<ExperienceModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

            repository = GetExperiencePageRepository();
            var result = repository.Get("/food");
            Assert.IsNull(result);
        }
        [Test]
        public void GetTest_ExperienceNodeFound_HasRelatedArticle_ShouldReturnResult()
        {
            var experience = Builder<ExperienceModel>.CreateNew().Build();
            experience.SummaryInfo = Builder<ExperienceSummaryModel>.CreateNew().Build();
            cacheService.GetDocumentDependentOnPath<ExperienceModel>(Arg.Any<Func<ExperienceModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(experience);

            repository = GetExperiencePageRepository();
            articleService.GetRelatedArticlesByExperience(Arg.Any<Guid>())
                .Returns(new List<ArticleModel>
            {
                new ArticleModel
                {
                    Title = "Article 1"
                }
            });

            repository = GetExperiencePageRepository();
            var result = repository.Get("/food");
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotEmpty(result.RelatedArticles)
            );
        }
        [Test]
        public void GetLandingTest_PageFound_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new PageItemModel
            {
                Page = Builder<PageNodeModel>.CreateNew().Build()
            });

            repository = GetExperiencePageRepository();
            var result = repository.GetLandingPage("/Experiences");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetLandingTest_PageNotFound_ShouldReturnNullResult()
        {

            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

            repository = GetExperiencePageRepository();
            var result = repository.GetLandingPage("/Experiences");
            Assert.IsNull(result);
        }

        [Test]
        public void GetLandingTest_PageFound_ExperiencesFound_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new PageItemModel
            {
                Page = Builder<PageNodeModel>.CreateNew().Build()
            });
            cacheService.GetDocumentDependentOnChildrenPath<List<ExperienceSummaryModel>>(Arg.Any<Func<List<ExperienceSummaryModel>>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new List<ExperienceSummaryModel> { new ExperienceSummaryModel() });
            mapper.Map<List<PrimaryLandingItemModel>>(Arg.Any<List<ExperienceSummaryModel>>())
                .Returns(new List<PrimaryLandingItemModel> { new PrimaryLandingItemModel() });

            repository = GetExperiencePageRepository();
            var result = repository.GetLandingPage("/Experiences");
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotEmpty(result.Items)
            );
        }
    }
}
