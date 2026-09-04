using CMS.Tests;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories;
using FizzWare.NBuilder;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using ETG.Data.Cache;
using ETG.Data.Career.Models;
using ETG.Data.Career.Repositories;
using ETG.Data.Career.Services;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Tests.Career
{
    [TestFixture]
    public class CareerApplyPageRepositoryTest : UnitTests
    {
        private ICareerApplyPageRepository repository;
        private ICacheService cacheService;
        private IPageItemRepository pageRepository;
        private ICareerService careerService;
        private IShareLinksService shareLinksService;
        private IImageRepository imageRepository;
        [SetUp]
        public void Setup()
        {
            careerService = Substitute.For<ICareerService>();
            cacheService = Substitute.For<ICacheService>();
            pageRepository = Substitute.For<IPageItemRepository>();
            shareLinksService = Substitute.For<IShareLinksService>();
            imageRepository = Substitute.For<IImageRepository>();
        }
        private CareerApplyPageRepository GetCareerApplyPageRepository()
        {
            return new CareerApplyPageRepository(pageRepository, careerService, cacheService,imageRepository, shareLinksService);

        }
        [Test]
        public void GetTest_PageNodeFound_ShouldReturnResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());

            repository = GetCareerApplyPageRepository();
            var result = repository.GetApplyPage("/career");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTest_PageNodeNotFound_ShouldReturnNullResult()
        {
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

            repository = GetCareerApplyPageRepository();
            
            var result = repository.GetApplyPage("/career");
            Assert.IsNull(result);
        }
        [Test]
        public void GetTest_PageNodeFound_HasRole_ShouldReturnResult()
        {
            var role = Builder<CareerRoleModel>.CreateNew().Build();
            role.BasicInfo =  new CareerRoleBasicInfoModel();
            careerService.GetCareerRole(Arg.Any<string>())
                .Returns(role);
            cacheService.GetDocumentDependentOnPath<PageItemModel>(Arg.Any<Func<PageItemModel>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Builder<PageItemModel>.CreateNew().Build());


            repository = GetCareerApplyPageRepository();
            var result = repository.GetApplyPage("/brochures");
            Assert.IsNotNull(result);
            CMSAssert.All(() => Assert.IsNotNull(result),
                () => Assert.IsNotNull(result.Role)
            );
        }


    }
}
