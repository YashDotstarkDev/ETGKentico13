using CMS.Tests;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories;
using FizzWare.NBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ETG.Data.Cache;
using ETG.Data.Career.Models;
using ETG.Data.Career.Repositories;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Tests.Career
{
    [TestFixture]
    public class CareerPageRepositoryTest : UnitTests
    {
        private ICareerPageRepository repository;
        private ICacheService cacheService;
        private IPageItemRepository pageRepository;
        private ICareerRoleRepository careerRoleRepository;
        private IShareLinksService shareLinksService;
        private IImageRepository imageRepository;
        [SetUp]
        public void Setup()
        {
            careerRoleRepository = Substitute.For<ICareerRoleRepository>();
            cacheService = Substitute.For<ICacheService>();
            pageRepository = Substitute.For<IPageItemRepository>();
            shareLinksService = Substitute.For<IShareLinksService>();
            imageRepository = Substitute.For<IImageRepository>();
        }

        private CareerPageRepository GetCareerPageRepository()
        {
            return new CareerPageRepository(pageRepository, careerRoleRepository, cacheService, imageRepository, shareLinksService);

        }
        [Test]
        public void GetTest_PageNodeFound_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());

            repository = GetCareerPageRepository();
            var result = repository.Get("/career");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTest_PageNodeNotFound_ShouldReturnNullResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

            repository = GetCareerPageRepository();
            var result = repository.Get("/brochures");
            Assert.IsNull(result);
        }
        [Test]
        public void GetTest_PageFound_HasRoles_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());
            cacheService.GetDocumentDependentOnChildrenPath<List<CareerRoleBasicInfoModel>>(Arg.Any<Func<List<CareerRoleBasicInfoModel>>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(
                new List<CareerRoleBasicInfoModel>
                {
                    new CareerRoleBasicInfoModel()
                }
                );


            repository = GetCareerPageRepository();
            var result = repository.Get("/brochures");

            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotEmpty(result.Roles)
            );
        }


    }
}
