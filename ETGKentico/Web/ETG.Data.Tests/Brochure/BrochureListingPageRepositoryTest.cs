using CMS.Tests;
using ETG.Data.Brochure.Models;
using ETG.Data.Brochure.Repositories;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories;
using FizzWare.NBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ETG.Data.Cache;
using ETG.Data.Brochure.Services;
using ETG.Data.Models.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Tests.Brochure
{
    [TestFixture]
    public class BrochureListingPageRepositoryTest : UnitTests
    {
        private IBrochureListingPageRepository repository;
        private ICacheService cacheService;
        private IPageItemRepository pageRepository;
        private IBrochureService brochureService;
        private IShareLinksService shareLinksService;
        private IImageRepository imageRepository;
        [SetUp]
        public void Setup()
        {
            brochureService = Substitute.For<IBrochureService>();
            cacheService = Substitute.For<ICacheService>();
            pageRepository = Substitute.For<IPageItemRepository>();
            shareLinksService = Substitute.For<IShareLinksService>();
            imageRepository = Substitute.For<IImageRepository>();
        }

        private IBrochureListingPageRepository GetBrochureListingPageRepository()
        {
            return new BrochureListingPageRepository(pageRepository, brochureService, cacheService, imageRepository, shareLinksService);
        }

        [Test]
        public void GetTest_BrochureLandingNodeFound_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());

            repository = GetBrochureListingPageRepository();
            var result = repository.Get("/brochures");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTest_BrochureLandingNodeNotFound_ShouldReturnNullResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();


            repository = GetBrochureListingPageRepository(); 
            var result = repository.Get("/brochures");
            Assert.IsNull(result);
        }
        [Test]
        public void GetTest_BrochureLandingNodeFound_HasBrochureList_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns( new PageItemModel{
                    Page = Builder<PageNodeModel>.CreateNew().Build()
                });
            cacheService.GetDocumentDependentOnChildrenPath<List<BrochureModel>>(Arg.Any<Func<List<BrochureModel>>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(
                new List<BrochureModel>
                {
                    new BrochureModel()
                }
                );

            brochureService.GetAllBrochures().Returns(new List<BrochureModel>
            {
                new BrochureModel()
            });

            repository = GetBrochureListingPageRepository(); 
            var result = repository.Get("/brochures");
            Assert.IsNotNull(result);
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotEmpty(result.Brochures)
            );
        }

        [Test]
        public void GetBrochureOrderTest_PageFound_ValidGuid_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());
            brochureService.GetBrochures(Arg.Any<List<Guid>>()).Returns(new List<BrochureModel>
            {
                new BrochureModel()
            });

            repository = GetBrochureListingPageRepository(); 
            var result = repository.GetBrochureOrderPage("ae1b8694-deeb-4c3b-84cc-d69f53faff75,ae1b8694-deeb-4c3b-84cc-d69f53faff75");
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotNull(result.Form),
                () => Assert.IsNotEmpty(result.Form.BrochureOrders),
                () => Assert.IsNotEmpty(result.Form.BrochureGuids));
            ;
        }

        [Test]
        public void GetBrochureOrderTest_PageNotFound_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();


            repository = GetBrochureListingPageRepository(); 
            var result = repository.GetBrochureOrderPage("ae1b8694-deeb-4c3b-84cc-d69f53faff75");
            Assert.IsNull(result);
        }

        [Test]
        public void GetBrochureOrderTest_PageFound_NoBrochureGuid_ShouldReturnResultWithGuid()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());


            repository = GetBrochureListingPageRepository(); 
            var result = repository.GetBrochureOrderPage("");
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNull(result.Form));
        }

        [Test]
        public void GetBrochureOrderTest_PageFound_SomeInvalidBrochureGuid_ShouldReturnResultWithGuid()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());


            repository = GetBrochureListingPageRepository(); 
            brochureService.GetBrochures(Arg.Any<List<Guid>>()).Returns(new List<BrochureModel>
            {
                new BrochureModel { BrochureNodeGuid = new Guid("ae1b8694-deeb-4c3b-84cc-d69f53faff75")}
            });
            var result = repository.GetBrochureOrderPage("ae1b8694-deeb-4c3b-84cc-d69f53faff75,ae1b8694-deeb-4c3b-");
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotNull(result.Form),
                () => Assert.IsNotEmpty(result.Form.BrochureOrders),
                () => Assert.IsNotEmpty(result.Form.BrochureGuids),
                () => Assert.AreEqual(36,result.Form.BrochureGuids.Length));
        }

    }
}
