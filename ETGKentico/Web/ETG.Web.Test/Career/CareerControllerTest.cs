using AutoMapper;
using CMS.Tests;
using Devotion.Web.Base.Providers;
using ETG.Core.Forms;
using ETG.Core.Http;
using ETG.Data.Career.Repositories;
using ETG.Data.Career.Services;
using ETG.Data.Forms;
using ETG.Web.Career.Controllers;
using ETG.Web.Career.Models;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Services;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;
using System.Web;
using ETG.Data.Configuration;

namespace ETG.Web.Tests.Services

{
    [TestFixture]
    public class CareerControllerTest : UnitTests
    {
        private CareerController controller;
        private IMapper mapper;

        private ICareerPageRepository careerPageRepository;
        private ICareerService careerService;
        private ICareerApplyPageRepository careerApplyPageRepository;
        private IAuthenticationProvider<UserModel> baseAuthenticationProvider;
        private IStorageService storageService;
        private AbstractValidator<CareerRoleApplicationItem> formValidator;
        private IBizformEntry<CareerRoleApplicationItem> bizformEntry;
        private IHttpRequest httpRequest;
        private IApiKeyProvider apiKeyProvider;
        [SetUp]
        public void Setup()
        {
            bizformEntry = Substitute.For<IBizformEntry<CareerRoleApplicationItem>>();
            careerPageRepository = Substitute.For<ICareerPageRepository>();
            careerService = Substitute.For<ICareerService>();
            careerApplyPageRepository = Substitute.For<ICareerApplyPageRepository>();
            storageService = Substitute.For<IStorageService>();
            mapper = Substitute.For<IMapper>();
            baseAuthenticationProvider = Substitute.For<IAuthenticationProvider<UserModel>>();
            formValidator = Substitute.For<AbstractValidator<CareerRoleApplicationItem>>();
            apiKeyProvider = Substitute.For<IApiKeyProvider>();

            httpRequest = Substitute.For<IHttpRequest>();
            httpRequest.GetRequest().Returns(new HttpRequest("a", "http://test.com/abc", string.Empty));

        }

        [Test]
        public void Submit_ShouldReturnResult()
        {
            bizformEntry.Validate().Returns(true);
            bizformEntry.Submit().Returns(true);
            storageService.SaveFile(Arg.Any<string>(), Arg.Any<HttpPostedFileBase>()).Returns("dummyfile");
            controller = new CareerController(mapper, careerPageRepository, careerService, careerApplyPageRepository,
                baseAuthenticationProvider, storageService, formValidator, httpRequest, apiKeyProvider, bizformEntry);
            var viewModel = new CareerApplyPageViewModel
            {
                Form = new CareerApplyFormViewModel()
            };

            var result = controller.Submit(viewModel);
            Assert.IsNotNull(result);
        }


    }
}
