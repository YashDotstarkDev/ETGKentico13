using System.Collections.Generic;
using System.Web;
using AutoMapper;
using ETG.Core.Forms;
using ETG.Data.Repositories.Forms;
using ETG.Data.Services;
using ETG.Web.Attributes.Filters;
using ETG.Data.Forms;
using ETG.Web.Models.Forms;
using FluentValidation;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using System.Web.Mvc;
using ETG.Data.Configuration;
using ETG.Web.Controllers.Base;
using ETG.Web.Helpers;
using ETG.Web.Models.Base.ApiResponse;
using ETG.Web.Services._Interfaces;

namespace ETG.Web.Controllers
{
    public class ContactUsController : FormController
    {
        private readonly IMapper _mapper;
        private readonly IContactUsPageRepository _contactUsPageRepository;
        private readonly AbstractValidator<EnquireItem> _formValidator;
        private readonly IBizformEntry<EnquireItem> _bizFormEntry;
        private readonly IApiKeyProvider _apiKeyProvider;
        private readonly IUtmService _utmService;

        public ContactUsController(IMapper mapper, IContactUsPageRepository contactUsPageRepository,
            AbstractValidator<EnquireItem> formValidator,
            IApiKeyProvider apiKeyProvider,
            IBizformEntry<EnquireItem> bizFormEntry, IKenticoContactService contactService, IUtmService utmService) :
            base(contactService)
        {
            _mapper = mapper;
            _contactUsPageRepository = contactUsPageRepository;
            _formValidator = formValidator;
            _bizFormEntry = bizFormEntry;
            _apiKeyProvider = apiKeyProvider;
            _utmService = utmService;
        }

        private ContactUsPageViewModel GetViewModel(ContactUsFormViewModel formModel = null)
        {
            ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
            ViewBag.FormAction = "contact";
            var model = _contactUsPageRepository.Get("/Contact-Us");
            

            var viewModel = _mapper.Map<ContactUsPageViewModel>(model);

            if (formModel != null)
            {
                viewModel.Form = _mapper.Map<ContactUsFormViewModel>(formModel);
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
            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View(viewModel);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        [ReCaptchaV3Validation]
        [Route("api/contact-us/submit")]
        public ActionResult Submit(ContactUsPageViewModel model)
        {
            if (model?.Form == null)
            {
                return null;
            }

            if (!ModelState.IsValid)
            {
                if (ModelState.ContainsKey("Error") && ModelState["Error"].Errors.Count > 0)
                {
                    model.Form.ErrorMessage = ModelState["Error"].Errors[0].ErrorMessage;

                    var response = new GenericApiResponse()
                    {
                        objectSet = new ObjectSet()
                        {
                            actions = new List<JsonResponse>()
                            {
                                new JsonResponse() { type = "message", content = model.Form.ErrorMessage}
                            }
                        }
                    };
                    return Json(response);
                }
            }

            if (!model.Form.Validate())
            {

                var response = new GenericApiResponse()
                {
                    objectSet = new ObjectSet()
                    {
                        actions = new List<JsonResponse>()
                        {
                            new JsonResponse() { type = "message", content = model.Form.ErrorMessage}
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
                SubscribeToNewsletter = model.Form.SubscribeToNewsletter,
                Message = model.Form.Message
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
                var response = new GenericApiResponse()
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

            if (_bizFormEntry.Submit())
            {
                //AddContact(model.Form.SubscribeToNewsletter, model.Form.FirstName, model.Form.LastName,
                //    model.Form.Email, model.Form.Phone, model.Form.PreferredContactType);

                var response = new GenericApiResponse()
                {
                    objectSet = new ObjectSet()
                    {
                        actions = new List<JsonResponse>()
                        {
                            new JsonResponse() { type = "redirect", uri = "/contact-us/thank-you" }
                        }
                    }
                };
                return Json(response);
            }

            return null;
        }
    }
}