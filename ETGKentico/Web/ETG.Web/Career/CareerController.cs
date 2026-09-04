using System;
using System.Collections.Generic;
using AutoMapper;
using Devotion.Web.Base.Providers;
using ETG.Core.Constants;
using ETG.Core.Forms;
using ETG.Data.Career.Models;
using ETG.Data.Career.Repositories;
using ETG.Data.Career.Services;
using ETG.Web.Career.Models;
using ETG.Web.Common.Models.Authentication;
using ETG.Data.Forms;
using FluentValidation;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using CMS.Base;
using CMS.Core;
using CMS.IO;
using ETG.Core.Http;
using ETG.Data.Configuration;
using ETG.Web.Attributes.Filters;
using ETG.Web.Controllers.Base;
using ETG.Web.Helpers;
using ETG.Web.Models.Base.ApiResponse;
using ETG.Web.Services;

namespace ETG.Web.Career.Controllers
{
    public class CareerController : PageController<ICareerPageRepository, CareerPageModel, CareerPageViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICareerService _careerService;
        private readonly ICareerApplyPageRepository _careerApplyPageRepository;
        private readonly IStorageService _storageService;
        private readonly AbstractValidator<CareerRoleApplicationItem> _formValidator;
        private readonly IHttpRequest _httpRequest;
        private readonly IBizformEntry<CareerRoleApplicationItem> _bizFormEntry;
        private readonly IApiKeyProvider _apiKeyProvider;
        public CareerController(IMapper mapper,
            ICareerPageRepository repository,
            ICareerService careerService,
            ICareerApplyPageRepository careerApplyPageRepository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider,
            IStorageService storageService,
            AbstractValidator<CareerRoleApplicationItem> formValidator,
            IHttpRequest httpRequest,
            IApiKeyProvider apiKeyProvider,
            IBizformEntry<CareerRoleApplicationItem> bizFormEntry)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _mapper = mapper;
            _careerService = careerService;
            _careerApplyPageRepository = careerApplyPageRepository;
            _formValidator = formValidator;
            _storageService = storageService;
            _httpRequest = httpRequest;
            _bizFormEntry = bizFormEntry;
            _apiKeyProvider = apiKeyProvider;
        }
        protected override void ProcessBeforeReturningView(CareerPageViewModel viewModel)
        {
            if (viewModel != null && viewModel.Page != null)
            {
                PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            }
        }
        private CareerApplyPageViewModel GetApplyPageViewModel(string roleAlias,CareerApplyFormViewModel formModel = null)
        {
            ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
            ViewBag.FormAction = "apply";
            var model = _careerApplyPageRepository.GetApplyPage(roleAlias);

            var viewModel = _mapper.Map<CareerApplyPageViewModel>(model);

            if (formModel != null)
            {
                viewModel.Form = _mapper.Map<CareerApplyFormViewModel>(formModel);

            }

            return viewModel;
        }
        [HandleError]
        public ActionResult Role(string alias)
        {
            var model = _careerService.GetCareerRole($"{PathConstants.PATH_CAREER_ROLES}/{alias}");

            if (model == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var viewModel = _mapper.Map<CareerRoleViewModel>(model);

            return View("Role", viewModel);
        }
        [HandleError]
        public ActionResult Apply(string rolealias)
        {

            var viewModel = GetApplyPageViewModel(rolealias);
            if (viewModel == null)
            {
                throw new HttpException(404, "Page not found");
            }
            return View("Apply", viewModel);
        }

        [HttpPost]
        [ReCaptchaV3Validation]
        public ActionResult Submit(CareerApplyPageViewModel model)
        {
            if (model == null || model.Form == null)
            {
                return null;
            }
            
            if (!ModelState.IsValid)
            {
                if (ModelState.ContainsKey("Error") && ModelState["Error"].Errors.Count > 0)
                {
                    model.Form.ErrorMessage = ModelState["Error"].Errors[0].ErrorMessage;

                    var viewModel = GetApplyPageViewModel(model.Form.PageAlias, model.Form);

                    return View("Apply", viewModel);
                }
            }
            if (ModelState.IsValid)
            {
                var fileRelativePath = _storageService.SaveFile("/etg/jobs/", model.Form.CVFile);

                
                var request = _httpRequest.GetRequest();
                var bizformItem = new CareerRoleApplicationItem
                {
                    FirstName = model.Form.FirstName,
                    LastName = model.Form.LastName,
                    Email = model.Form.Email,
                    Phone = model.Form.Phone,
                    PreferredContactMethod = model.Form.PreferredContactType,
                    Comments = model.Form.Message,
                    Role = model.Form.Role,
                    CVFilePath = fileRelativePath// $"{(request.IsSecureConnection? "https://" : "http://")}{request.Url.Host}{fileRelativePath}",
                    

                };

                _bizFormEntry.Initialize(_formValidator, bizformItem);

                if (!_bizFormEntry.Validate())
                {
                    model.Form.ErrorMessage = "Please fill up mandatory fields.";
                    var viewModel = GetApplyPageViewModel(model.Form.PageAlias, model.Form);

                    return View("Apply", viewModel);
                }

                if (_bizFormEntry.Submit())
                {
                    Response.Redirect("/careers/apply/thank-you");
                }
            }

            return null;
        }
    }
}