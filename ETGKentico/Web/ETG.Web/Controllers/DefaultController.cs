using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using ETG.Booking.Pricing.Repositories;
using ETG.Core.Constants;
using ETG.Core.Http;
using ETG.Data.Repositories.Pages;
using ETG.Data.Tour.Repositories;
using ETG.Web.Helpers;
using ETG.Web.Models.PageTypes;
using ETG.Web.Tour.Models;
using ETG.Web.Web;
using System.Web;
using System.Web.Mvc;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.SiteProvider;
using DocumentFormat.OpenXml.Wordprocessing;
using ETG.Core.PageTypes;
using ETG.Data.Configuration;
using ETG.Data.Extensions;
using ETG.Data.Promotion;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.TravelType.Repositories;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.Extensions;
using ETG.Web.Models.Common;
using ETG.Web.Models.Schemas;
using ETG.Web.TravelType.Models;
using Newtonsoft.Json;

namespace ETG.Web.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IMapper _mapper;

        private readonly ITourPageRepository _tourPageRepository;
        private readonly IContainerPageRepository _repository;
        private readonly IHttpRequest _httpRequest;
        private readonly IViewDetector _controllerRedirect;
        private readonly IBookingPriceRepository _bookingPriceRepository;
        private readonly IApiKeyProvider _apiKeyProvider;
        private readonly ETGSettings _settings;
        private readonly IPromotionRepository _promotionRepository;
        private readonly ITravelTypeDetailPageRepository _travelTypeDetailPageRepository;


        public DefaultController(IMapper mapper,
            IContainerPageRepository repository,
            ITourPageRepository tourPageRepository,
            IViewDetector controllerRedirect,
            IHttpRequest httpRequest,
            IBookingPriceRepository bookingPriceRepository,
            IETGSettingsService etgSettingsService,
            IApiKeyProvider apiKeyProvider,
            IPromotionRepository promotionRepository,
            ITravelTypeDetailPageRepository travelTypeDetailPageRepository)
        {
            _repository = repository;
            _tourPageRepository = tourPageRepository;
            _mapper = mapper;
            _httpRequest = httpRequest;
            _controllerRedirect = controllerRedirect;
            _bookingPriceRepository = bookingPriceRepository;
            _apiKeyProvider = apiKeyProvider;
            _settings = etgSettingsService.GetSettings();
            _promotionRepository = promotionRepository;
            _travelTypeDetailPageRepository = travelTypeDetailPageRepository;
        }

        [HandleError]
        public ActionResult Index()
        {
            var path = _httpRequest.GetRequest().Url.PathAndQuery.GetUrlPathOnly();
            var view = _controllerRedirect.GetView(path);
            if (view == ViewConstants.TOUR)
            {
                var alias = "";

                if (path.Split('/').Length == 3)
                {
                    alias = path.Split('/')[2];
                    alias = HttpUtility.UrlDecode(alias);
                }

                var tour = _tourPageRepository.Get(path, alias);
                if (tour == null || tour.TourInfo == null)
                {
                    throw new HttpException(404, "Page not found");
                }

                if (!tour.TourInfo.IsPublished)
                {
                    var index = path.LastIndexOf("/");

                    if (index == -1)
                    {
                        Response.Redirect("/");
                    }
                    else
                    {
                        Response.RedirectPermanent($"/destinations{path.Substring(0, index)}");
                    }

                    return null;
                }

                var tourViewModel = _mapper.Map<TourPageViewModel>(tour);
                tourViewModel.EntireFlexAmountPerPerson =
                    _settings != null ? _settings.EntireFlexCostPerPerson.ToInteger(0) : 0;
                var dates = _bookingPriceRepository.GetDepartureBookingPrices(
                    tourViewModel.TourInfo.TourSummaryInfo.TourCode,
                    tour.TourInfo.BookingEarliestDepartureDate, tour.TourInfo.BookingLatestDepartureDate);
                if (tourViewModel.TourInfo.BookNowEnabled)
                {
                    tourViewModel.TourInfo.BookNowEnabled = !dates.IsNullOrEmpty();
                }

                if (!tourViewModel.FAQs.IsNullOrEmpty())
                {
                    var faqSchema = new FAQSchema();
                    faqSchema.Context = "https://schema.org";
                    faqSchema.Type = "FAQPage";
                    faqSchema.MainEntity = tourViewModel.FAQs.Select(f => new MainEntity
                    {
                        Type = "Question",
                        Name = f.Question,
                        AcceptedAnswer = new AcceptedAnswer
                        {
                            Type = "Answer",
                            Text = f.Answer.StripHtml()
                        }
                    }).ToList();

                    tourViewModel.ConnectWithUs = new ConnectWithUsViewModel
                    {
                        EnquireNowCta = "Enquire About this Package",
                        EnquireNowCtaUrl = $"/enquire/{tourViewModel.TourInfo.TourSummaryInfo.TourCode}"
                    };

                    tourViewModel.FAQSchema = JsonConvert.SerializeObject(faqSchema);
                }

                tourViewModel.TourInfo.TourSummaryInfo.Promotion = _promotionRepository
                    .GetPromotionInfoForNonAgent(tour.TourInfo, DateTime.Today).MapToPromotionItem();

                var googleReview = GoogleReviewHelper.GetGoogleReviewViewModel().ReviewListingResponse;
                if (googleReview != null)
                    tourViewModel.GoogleReview = googleReview;

                PageHelper.InitializePageBuilder(HttpContext, tourViewModel.TourInfo.DocumentID);
                return View("~/Views/Tour/Index.cshtml", tourViewModel);
            }

            var model = _repository.Get(path, RouteData.GetAlias());
            if (model == null)
            {
                var travelTypeVM =
                    _mapper.Map<TravelTypeDetailPageViewModel>(
                        _travelTypeDetailPageRepository.Get(_httpRequest.GetRequest().Url.PathAndQuery.GetUrlPathOnly()));
                if (travelTypeVM!=null)
                {
                    travelTypeVM.ConnectWithUs = new ConnectWithUsViewModel
                        { EnquireNowCta = "Enquire about this package", EnquireNowCtaUrl = "/enquire" };

                    PageHelper.InitializePageBuilder(HttpContext, travelTypeVM.Detail.DocumentID);
                    return View("~/Views/TravelType/Index.cshtml", travelTypeVM);
                }

                var travelTypeLandingVM =
                    _mapper.Map<TravelTypeLandingViewModel>(_travelTypeDetailPageRepository.GetLandingPage(_httpRequest.GetRequest().Url.PathAndQuery.GetUrlPathOnly()));

                if (travelTypeLandingVM!=null)
                {
                    PageHelper.InitializePageBuilder(HttpContext, travelTypeLandingVM.DocumentID);
                    return View("~/Views/TravelType/Landing.cshtml", travelTypeLandingVM);    
                }
                

                

                throw new HttpException(404, "Page not found");
            }

            if (!string.IsNullOrWhiteSpace(model.RedirectTo))
            {
                return Redirect(model.RedirectTo);
            }

            var viewModel = _mapper.Map<PageItemViewModel>(model);

            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.DocumentID);
            return View(viewModel);
        }

        [HandleError]
        public ActionResult CampaignLanding(string url)
        {
            var path = _httpRequest.GetRequest().Url.PathAndQuery.GetUrlPathOnly();

            var model = _repository.Get(path);
            if (model == null)
            {
                throw new HttpException(404, "Page not found");
            }

            if (!string.IsNullOrWhiteSpace(model.RedirectTo))
            {
                return Redirect(model.RedirectTo);
            }

            var viewModel = _mapper.Map<PageItemViewModel>(model);
            if (viewModel.FormEnabled)
            {
                ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
                ViewBag.FormAction = "campaign";
            }

            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.DocumentID);

            return model.IsNewLandingPage
                ? View("~/Views/Default/NewCampaignLanding.cshtml", viewModel)
                : View(viewModel);
        }

        [HandleError]
        public ActionResult GenericContent(string url)
        {
            var path = _httpRequest.GetRequest().Url.PathAndQuery.GetUrlPathOnly();
            var doc = DocumentHelper.GetDocuments().OnCurrentSite().Path(path).TopN(1).FirstOrDefault();

            if (doc == null)
            {
                throw new HttpException(404, "Page not found");
            }

            PageHelper.InitializePageBuilder(HttpContext, doc.DocumentID);

            return View("~/Views/Default/GenericContentTemplate.cshtml");
        }
    }
}