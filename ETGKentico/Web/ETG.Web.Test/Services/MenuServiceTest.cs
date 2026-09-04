using CMS.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ETG.Data.Global;
using ETG.Web.Services;
using AutoMapper;
using ETG.Core.Kentico;
using ETG.Data.Cache;
using ETG.Data.Models.Global;
using ETG.Web.Models.Menu;
using ETG.Web.Services.Menu;
using NSubstitute;

namespace ETG.Web.Tests.Services

{
    [TestFixture]
    public class MenuServiceTest : UnitTests
    {
        private IMenuService service;
        private ICacheService cacheService;
        private IMapper mapper;
        private ILinkRepository linkRepository;
        private ISiteContext siteContext;
        [SetUp]
        public void Setup()
        {
            linkRepository = Substitute.For<ILinkRepository>();
            cacheService = Substitute.For<ICacheService>();
            mapper = Substitute.For<IMapper>();
            siteContext = Substitute.For<ISiteContext>();
        }

        [Test]
        public void GetMainMenuViewModel_ShouldReturnResult()
        {

            cacheService.GetDocumentDependentOnChildrenPath<List<LinkModel>>(Arg.Any<Func<List<LinkModel>>>(),
                    Arg.Any<string>(), Arg.Any<string>())
                .Returns(new List<LinkModel> { new LinkModel() });
            mapper.Map<List<LinkViewModel>>(Arg.Any<List<LinkModel>>()).Returns(new List<LinkViewModel> { new LinkViewModel() });
            service = new MenuService(mapper, linkRepository, cacheService);
            var result = service.GetMainMenuViewModel();
            Assert.IsNotNull(result);
        }


    }
}
