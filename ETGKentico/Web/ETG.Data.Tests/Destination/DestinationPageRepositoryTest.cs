using System;
using System.Collections.Generic;
using AutoMapper;
using CMS.Tests;
using ETG.Data.Article.Models;
using ETG.Data.Article.Services;
using ETG.Data.Brochure.Services;
using ETG.Data.Cache;
using ETG.Data.Destination.Models;
using ETG.Data.Destination.Repositories;
using ETG.Data.Destination.Services;
using ETG.Data.DestinationExpertTeam.Services;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Common;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;
using ETG.Data.Tour.Services;
using FizzWare.NBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace ETG.Data.Tests.Destination
{
    [TestFixture]
    public class DestinationPageRepositoryTest : UnitTests
    {
        private IDestinationPageRepository repository;

        private IDestinationExpertTeamService destinationExpertService;
        private IAccordionItemRepository accordionItemRepository;
        private ICacheService cacheService;
        private IArticleService articleService;
        private IImageRepository imageRepository;
        private IContainerRepository containerRepository;
        private IMapper mapper;
        private IDestinationService destinationService;
        private ITourService tourService;
        private IBrochureService brochureService;

        private IShareLinksService shareLinksService;
        [SetUp]
        public void Setup()
        {
            mapper = Substitute.For<IMapper>();
            destinationService = Substitute.For<IDestinationService>();
            cacheService = Substitute.For<ICacheService>();
            articleService = Substitute.For<IArticleService>();
            imageRepository = Substitute.For<IImageRepository>();
            containerRepository = Substitute.For<IContainerRepository>();
            tourService = Substitute.For<ITourService>();
            brochureService = Substitute.For<IBrochureService>();

            destinationExpertService = Substitute.For<IDestinationExpertTeamService>();
            accordionItemRepository = Substitute.For<IAccordionItemRepository>();
            shareLinksService = Substitute.For<IShareLinksService>();
        }

        private DestinationPageRepository GetDestinationPageRepository()
        {
            return new DestinationPageRepository(cacheService, destinationService, containerRepository, destinationExpertService, imageRepository,
                articleService, accordionItemRepository, tourService, brochureService, shareLinksService, mapper);

        }


        [Test]
        public void GetTest_DestinationNodeFound_ShouldReturnResult()
        {
            var destination = Builder<DestinationModel>.CreateNew().Build();
            destination.BasicInfo = Builder<DestinationSummaryModel>.CreateNew().Build();

            destinationService.GetDestinationFullPage(Arg.Any<string>()).Returns(destination);

            repository = GetDestinationPageRepository();
            var result = repository.Get("/italy");
            Assert.IsNotNull(result);
        }
        
        [Test]
        public void GetTest_DestinationNotFound_ShouldReturnNullResult()
        {
            cacheService.GetDocumentDependentOnPath<DestinationModel>(Arg.Any<Func<DestinationModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

            repository = GetDestinationPageRepository();
            var result = repository.Get("/italy");
            Assert.IsNull(result);
        }
        [Test]
        public void GetTest_DestinationNodeFound_HasRelatedArticle_ShouldReturnResult()
        {
            var destination = Builder<DestinationModel>.CreateNew().Build();
            destination.BasicInfo = Builder<DestinationSummaryModel>.CreateNew().Build();

            destinationService.GetDestinationFullPage( Arg.Any<string>()).Returns(destination);
            articleService.GetRelatedArticlesByDestination(Arg.Any<Guid>())
                .Returns(new List<ArticleModel>
            {
                new ArticleModel
                {
                    Title = "Article 1"
                }
            });

            repository = GetDestinationPageRepository();
            var result = repository.Get("/italy");
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

            repository = GetDestinationPageRepository();
            var result = repository.GetLandingPage("/destinations");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetLandingTest_PageNotFound_ShouldReturnNullResult()
        {

            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

            repository = GetDestinationPageRepository();
            var result = repository.GetLandingPage("/destinations");
            Assert.IsNull(result);
        }

        [Test]
        public void GetLandingTest_PageFound_DestinationsFound_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new PageItemModel
            {
                Page = Builder<PageNodeModel>.CreateNew().Build()
            }); ;
            cacheService.GetDocumentDependentOnChildrenPath<List<DestinationSummaryModel>>(Arg.Any<Func<List<DestinationSummaryModel>>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new List<DestinationSummaryModel> { new DestinationSummaryModel() });
            destinationService.GetMainDestinations().Returns(Builder<List<DestinationSummaryModel>>.CreateNew().Build());
            mapper.Map<List<PrimaryLandingItemModel>>(Arg.Any<List<DestinationSummaryModel>>())
                .Returns(new List<PrimaryLandingItemModel> {new PrimaryLandingItemModel()});

            repository = GetDestinationPageRepository();
            var result = repository.GetLandingPage("/destinations");
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotEmpty(result.Items)
            );
        }
    }
}
