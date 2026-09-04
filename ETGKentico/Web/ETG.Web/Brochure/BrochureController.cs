using System.Collections.Generic;
using AutoMapper;
using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using Devotion.Web.Base.Providers;
using ETG.Core.Forms;
using ETG.Data.Brochure.Repositories;
using ETG.Web.Brochure.Models;
using ETG.Web.Common.Models.Authentication;
using ETG.Data.Forms;
using FluentValidation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETG.Data.Brochure.Services;
using ETG.Data.Configuration;
using ETG.Web.Attributes.Filters;
using ETG.Web.Helpers;
using ETG.Web.Models.Base.ApiResponse;
using LinqToTwitter;

namespace ETG.Web.Controllers
{
    public class BrochureController : Controller// PageController<IBrochureListingPageRepository, BrochureListingPageModel, BrochureListingPageViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IBrochureListingPageRepository _brochureLandingPageRepository;
        private readonly IBrochureService _brochureService;
        private readonly AbstractValidator<BrochureOrderItem> _formValidator;
        private readonly IBizformEntry<BrochureOrderItem> _bizFormEntry;
        private readonly IApiKeyProvider _apiKeyProvider;
        public BrochureController(IMapper mapper,
            IBrochureListingPageRepository brochureLandingPageRepository, IBrochureService brochureService,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider,
            AbstractValidator<BrochureOrderItem> formValidator,
            IApiKeyProvider apiKeyProvider,
            IBizformEntry<BrochureOrderItem> bizFormEntry)
        {
            _apiKeyProvider = apiKeyProvider;
            _brochureLandingPageRepository = brochureLandingPageRepository;
            _mapper = mapper;
            _brochureService = brochureService;
            _formValidator = formValidator;
            _bizFormEntry = bizFormEntry;
        }

        private BrochureOrderPageViewModel GetBrochureOrderPageViewModel(string brochures, BrochureOrderFormViewModel formModel = null)
        {
            ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
            ViewBag.FormAction = "get-brochure";
            var model = _brochureLandingPageRepository.GetBrochureOrderPage(brochures);

            if (model == null)
            {
                return null;
            }

            var viewModel = _mapper.Map<BrochureOrderPageViewModel>(model);
            List<BrochureViewModel> brochureOrders = null;

            if (viewModel.Form != null)
            {
                brochureOrders = viewModel.Form.BrochureOrders;

            }
            if (formModel != null)
            {
                viewModel.Form = _mapper.Map<BrochureOrderFormViewModel>(formModel);
                viewModel.Form.BrochureOrders = brochureOrders;
            }

            return viewModel;
        }

        [HandleError]
        public ActionResult Index(string brochures)
        {
            var path = HttpContext.Request.Url.PathAndQuery.GetUrlPathOnly();

            var model = _brochureLandingPageRepository.Get(path, RouteData.GetAlias());
            if (model == null)
            {
                throw new HttpException(404, "Page not found");
            }
            var viewModel = _mapper.Map<BrochureListingPageViewModel>(model);
            viewModel.OrderedBrochureGuids = brochures;
            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View(viewModel);
        }

        [HandleError]
        public ActionResult Order(string brochures)
        {
            var viewModel = GetBrochureOrderPageViewModel(brochures);
            if (viewModel == null)
            {
                throw new HttpException(404, "Page not found");
            }
            return View("Order", viewModel);
        }

        [HandleError]
        public ActionResult View(string alias)
        {
            var page = _brochureLandingPageRepository.GetBrochurePage(string.Empty, alias);
            if (page == null)
            {
                throw new HttpException(404, "Page not found");
            }
            ViewBag.HideFooterSubscribe = true;
            ViewBag.HideFeefo = true;
            var viewModel = _mapper.Map<BrochurePageViewModel>(page);
            return View("View", viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ReCaptchaV3Validation]
        [Route("api/brochure/submit")]
        public ActionResult Submit(string brochures, BrochureOrderPageViewModel model)
        {
            
           
            if (model == null || model.Form == null)
            {
                var viewModel = GetBrochureOrderPageViewModel(brochures);
                if (viewModel == null)
                {
                    throw new HttpException(404, "Page not found");
                }
                //return View("Order", viewModel);
                
                var response = new GenericApiResponse()
                {
                    objectSet = new ObjectSet()
                    {
                        actions = new List<JsonResponse>()
                        {
                            new JsonResponse() { type = "message", content = "Page not found"}
                        }
                    }
                };
                return Json(response);
            }

            if (!ModelState.IsValid)
            {
                if (ModelState.ContainsKey("Error") && ModelState["Error"].Errors.Count > 0)
                {
                    model.Form.ErrorMessage = ModelState["Error"].Errors[0].ErrorMessage;

                    var viewModel = GetBrochureOrderPageViewModel(model.Form.BrochureGuids, model.Form);

                    //return View("Order", viewModel);
                    
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

            if (ModelState.IsValid)
            {
                if (model.Form.BrochureGuids.IsNullOrEmpty())
                {

                    model.Form.ErrorMessage = "No brochures selected.";
                    var viewModel = GetBrochureOrderPageViewModel(model.Form.BrochureGuids, model.Form);

                    //return View("Order", viewModel);
                    var response = new GenericApiResponse()
                    {
                        objectSet = new ObjectSet()
                        {
                            actions = new List<JsonResponse>()
                            {
                                new JsonResponse() { type = "message", content = "No brochures selected"}
                            }
                        }
                    };
                    return Json(response);
                }

                var brochureOrders = _brochureService.GetBrochures(model.Form.BrochureGuids.Split(',').Where(a => a.IsGuid()).Select(a => a.ToGuid()).ToList());

                if (model.Form.BrochureGuids.IsNullOrEmpty())
                {

                    model.Form.ErrorMessage = "No brochures selected.";
                    var viewModel = GetBrochureOrderPageViewModel(model.Form.BrochureGuids, model.Form);

                    var response = new GenericApiResponse()
                    {
                        objectSet = new ObjectSet()
                        {
                            actions = new List<JsonResponse>()
                            {
                                new JsonResponse() { type = "message", content = "No brochures selected"}
                            }
                        }
                    };
                    return Json(response);
                }

                var bizformItem = new BrochureOrderItem
                {
                    FirstName = model.Form.FirstName,
                    LastName = model.Form.LastName,
                    Email = model.Form.Email,
                    Phone = model.Form.Phone,
                    Message = model.Form.Message,
                    BrochureOrdersGUIDs = model.Form.BrochureGuids,
                    BrochureOrders = string.Join(",", brochureOrders.Select(a => a.Name))

                };


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
                    _brochureService.SendOrderResponse(bizformItem.FirstName, bizformItem.Email, bizformItem.BrochureOrdersGUIDs);

                    //Response.Redirect("/brochures/order/thank-you");
                    
                    var response = new GenericApiResponse()
                    {
                        objectSet = new ObjectSet()
                        {
                            actions = new List<JsonResponse>()
                            {
                                new JsonResponse() { type = "redirect", uri = "/brochures/order/thank-you" }
                            }
                        }
                    };
                    return Json(response);
                }
            }

            return null;
        }
    }
}