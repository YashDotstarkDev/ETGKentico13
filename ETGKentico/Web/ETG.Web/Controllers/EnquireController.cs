using System.Collections.Generic;
using System.Web;
using AutoMapper;
using ETG.Core.Forms;
using ETG.Data.Configuration;
using ETG.Data.Forms;
using ETG.Data.Repositories.Forms;
using ETG.Data.Services;
using ETG.Web.Attributes.Filters;
using ETG.Web.Models.Forms;
using FluentValidation;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using System.Web.Mvc;
using ETG.Data.Destination.Services;
using ETG.Web.Controllers.Base;
using ETG.Web.Helpers;
using ETG.Web.Models.Base.ApiResponse;
using ETG.Web.Services._Interfaces;

namespace ETG.Web.Controllers
{
    public class EnquireController : FormController
    {
        private readonly IMapper _mapper;
        private readonly IGenericEnquiryPageRepository _genericEnquiryPageRepository;
        private readonly AbstractValidator<EnquireItem> _formValidator;
        private readonly EnquireBizformEntry _bizFormEntry;
        private readonly IApiKeyProvider _apiKeyProvider;
        private readonly IDestinationService _destinationService;
        private readonly IUtmService _utmService;
        
        public EnquireController(
            IMapper mapper, 
            IGenericEnquiryPageRepository genericEnquiryPageRepository,
            AbstractValidator<EnquireItem> formValidator,
            EnquireBizformEntry bizFormEntry,
            IApiKeyProvider apiKeyProvider,
            IKenticoContactService kenticoContactService,
            IDestinationService destinationService, 
            IUtmService utmService) :
            base(kenticoContactService)
        {
            _mapper = mapper;
            _destinationService = destinationService;
            _genericEnquiryPageRepository = genericEnquiryPageRepository;
            _formValidator = formValidator;
            _bizFormEntry = bizFormEntry;
            _apiKeyProvider = apiKeyProvider;
            _utmService = utmService;
        }

        private GenericEnquiryPageViewModel GetViewModel(GenericEnquiryFormViewModel formModel = null)
        {
            ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
            ViewBag.FormAction = "enquire";
            var model = _genericEnquiryPageRepository.Get("/Enquire");

            var viewModel = _mapper.Map<GenericEnquiryPageViewModel>(model);

            if (formModel != null)
            {
                viewModel.Form = _mapper.Map<GenericEnquiryFormViewModel>(formModel);
            }

            return viewModel;
        }

        [HandleError]
        public ActionResult Index()
        {
            var viewModel = GetViewModel();
            if (viewModel == null)
            {
                throw new HttpException(404, "Page not found");
            }
            
            if (!string.IsNullOrWhiteSpace(viewModel.Page.RedirectTo))
            {
                return Redirect(viewModel.Page.RedirectTo);
            }
            
            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View(viewModel);
        }

        [HttpPost]
        [ReCaptchaV3Validation]
        [Route("api/enquire/submit")]
        public ActionResult Submit(GenericEnquiryPageViewModel model)
        {
            if (model?.Form == null)
            {
                return null;
            }

            var response = new GenericApiResponse();

            if (!ModelState.IsValid)
            {
                if (ModelState.ContainsKey("Error") && ModelState["Error"].Errors.Count > 0)
                {
                    model.Form.ErrorMessage = ModelState["Error"].Errors[0].ErrorMessage;

                    response = new GenericApiResponse()
                    {
                        objectSet = new ObjectSet()
                        {
                            actions = new List<JsonResponse>()
                            {
                                new JsonResponse() { type = "message", content = model.Form.ErrorMessage }
                            }
                        }
                    };
                    return Json(response);
                }
            }
            
            if (!model.Form.Validate())
            {
                
                response = new GenericApiResponse()
                {
                    objectSet = new ObjectSet()
                    {
                        actions = new List<JsonResponse>()
                        {
                            new JsonResponse() { type = "message", content = "Please fill up mandatory fields."}
                        }
                    }
                };
                return Json(response);
            }

            var bizformItem = new EnquireItem
            {
                Firstname = model.Form.FirstName,
                Lastname = model.Form.LastName,
                Email = model.Form.Email,
                Phone = model.Form.Phone,
                PreferredContactMethod = model.Form.PreferredContactType,
                PreferredDestination = model.Form.PreferredDestination,
                PreferredExperience = model.Form.PreferredExperience,
                PreferredTourType = model.Form.PreferredTourType,
                SubscribeToNewsletter = model.Form.SubscribeToNewsletter,
                Message = model.Form.Message,
            };

            var utm = _utmService.GetUtmCookie();
            if (utm != null)
            {
                bizformItem.UTMSource = utm.utm_source;
                bizformItem.UTMCampaign = utm.utm_campaign;
                bizformItem.UTMContent = utm.utm_content;
                bizformItem.UTMMedium = utm.utm_medium;
                bizformItem.UTMTerm = utm.utm_term;
            }

            _bizFormEntry.Initialize(_formValidator, bizformItem);

            if (!_bizFormEntry.Validate())
            {
                response = new GenericApiResponse()
                {
                    objectSet = new ObjectSet()
                    {
                        actions = new List<JsonResponse>()
                        {
                            new JsonResponse() { type = "message", content = "Please fill up mandatory fields."}
                        }
                    }
                };
                return Json(response);
            }
            

            _bizFormEntry.OverrideNotificationEmail =
                _destinationService.GetDestinationEmailNotification(model.Form.PreferredDestination);
       
            if (_bizFormEntry.Submit())
            {
                //AddContact(model.Form.SubscribeToNewsletter, model.Form.FirstName, model.Form.LastName,
                //    model.Form.Email, model.Form.Phone, model.Form.PreferredContactType);

                response = new GenericApiResponse()
                {
                    objectSet = new ObjectSet()
                    {
                        actions = new List<JsonResponse>()
                        {
                            new JsonResponse() { type = "redirect", uri = "/enquire/thank-you" }
                        }
                    }
                };
                return Json(response);
            }

            return null;
        }
    }
}