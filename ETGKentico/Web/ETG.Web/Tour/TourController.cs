using AutoMapper;
using Devotion.Web.Base.Providers;
using ETG.Core.Forms;
using ETG.Core.Http;
using ETG.Data.Configuration;
using ETG.Data.Forms;
using ETG.Data.Services;
using ETG.Data.Tour.Repositories;
using ETG.Data.Tour.Services;
using ETG.PDF;
using ETG.Web.Attributes.Filters;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Tour.Models;
using FluentValidation;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using ETG.Data.Extensions;
using ETG.Data.Tour.TourPdf;
using ETG.Web.Services._Interfaces;
using CMS.EventLog;
using System.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using ETG.Module.Booking.Models.HotelBuilder;
using System.IO;
using System.Linq;
using CMS.Base;
using CMS.Helpers;
using CMS.SiteProvider;
using ETG.Data.Repositories.Modules;
using ETG.Web.Models.Base.ApiResponse;
using Newtonsoft.Json.Serialization;

namespace ETG.Web.Tour
{
    public class TourController : Controller
    {
        private readonly ITourPageRepository _repository;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<EnquireItem> _formValidator;
        private readonly IBizformEntry<EnquireItem> _bizFormEntry;
        private readonly IKenticoContactService _kenticoContactService;
        private readonly IApiKeyProvider _apiKeyProvider;
        private readonly IHttpRequest _request;
        private readonly ITourService _tourService;
        private readonly ITourPdfProvider _tourPdfProvider;
        private readonly IUtmService _utmService;
        private readonly ITourExtraDetailService _tourExtraDetailService;
        private readonly IPDFGenerator _pdfGenerator;
        private readonly IContactService _etgContactService;
        private readonly ITourTypeRepository _tourTypeRepository;

        public TourController(IMapper mapper,
            ITourPageRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider,
            AbstractValidator<EnquireItem> formValidator,
            IBizformEntry<EnquireItem> bizFormEntry,
            IKenticoContactService kenticoContactService,
            IApiKeyProvider apiKeyProvider,
            IHttpRequest request,
            ITourService tourService,
            IPDFGenerator pdfGenerator,
            ITourPdfProvider tourPdfProvider,
            IUtmService utmService,
            ITourExtraDetailService tourExtraDetailService,
            IContactService etgContactService,
            ITourTypeRepository tourTypeRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _formValidator = formValidator;
            _bizFormEntry = bizFormEntry;
            _kenticoContactService = kenticoContactService;
            _apiKeyProvider = apiKeyProvider;
            _request = request;
            _tourService = tourService;
            _tourPdfProvider = tourPdfProvider;
            _utmService = utmService;
            _tourExtraDetailService = tourExtraDetailService;
            _pdfGenerator = pdfGenerator;
            _etgContactService = etgContactService;
            _tourTypeRepository = tourTypeRepository;
        }

        private TourEnquirePageViewModel GetViewModel(TourEnquireFormViewModel formModel = null, string tourcode = null,
            string date = null, string flightclass = null, string city = null)
        {
            ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
            ViewBag.FormAction = "enquire";
            var model = _repository.GetEnquiryPage(tourcode);

            if (model == null || model.TourInfo == null || model.TourInfo.TourSummaryInfo == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var viewModel = _mapper.Map<TourEnquirePageViewModel>(model);
            viewModel.Form = new TourEnquireFormViewModel
            {
                TourCode = model.TourInfo.TourSummaryInfo.TourCode,
                TourName = model.TourInfo.TourSummaryInfo.Name,
                DepartureClass = flightclass,
                DepartureDate = date,
                DepartureCity = city,
                DiscountText = model.TourInfo.TourSummaryInfo.Discount?.ToString(),
            };

            if (formModel != null)
            {
                viewModel.Form = _mapper.Map<TourEnquireFormViewModel>(formModel);
            }

            var contactDetails = _etgContactService.GetETGContactInfo();

            if (contactDetails != null)
            {
                viewModel.TourInfo.PeaceOfMindCheckList = contactDetails.PeachOfMindCheckList.IsNullOrEmpty()
                    ? null
                    : contactDetails.PeachOfMindCheckList.Split('\n').Select(a => a.Trim()).ToList();
            }

            return viewModel;
        }

        [HandleError]
        public ActionResult Enquire(string tourcode, string date, string flightclass, string city)
        {
            var viewModel = (TourEnquirePageViewModel)TempData["form"];

            if (viewModel == null)
            {
                viewModel = GetViewModel(null, tourcode, date, flightclass, city);
            }

            return View("Enquire", viewModel);
        }

        [ReCaptchaV3Validation]
        [HttpPost]
        public ActionResult Submit(TourEnquirePageViewModel model)
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

                    var viewModel = GetViewModel(model.Form, model.Form.TourCode, model.Form.DepartureDate,
                        model.Form.DepartureClass, model.Form.DepartureCity);
                    ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
                    ViewBag.FormAction = "enquire";
                    TempData["recaptchaKey"] = _apiKeyProvider.RecaptchaV3SiteKey;
                    TempData["form"] = GetViewModel(model.Form, model.Form.TourCode);
                    return RedirectToAction("Enquire",
                        new { tourcode = model.Form.TourCode });
                }
            }

            if (!model.Form.Validate())
            {
                ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
                ViewBag.FormAction = "enquire";
                TempData["recaptchaKey"] = _apiKeyProvider.RecaptchaV3SiteKey;
                TempData["form"] = GetViewModel(model.Form, model.Form.TourCode);
                return RedirectToAction("Enquire",
                    new { tourcode = model.Form.TourCode }); //View("Enquire", GetViewModel(model.Form));
            }

            var bizformItem = new EnquireItem
            {
                Firstname = model.Form.FirstName,
                Lastname = model.Form.LastName,
                Email = model.Form.Email,
                Phone = model.Form.Phone,
                PreferredContactMethod = model.Form.PreferredContactType,
                TourCode = model.Form.TourCode,
                TourClass = model.Form.DepartureClass,
                TourDepartureCity = model.Form.DepartureCity,
                TourName = model.Form.TourName,
                TourQuotedPrice = model.Form.QuotedPrice,
                TourDiscountText = model.Form.DiscountText,
                PriceCurrency = model.Form.PriceCurrency.DefaultCurrencyIfBlank(),
                SubscribeToNewsletter = model.Form.SubscribeToNewsletter,
                Message = model.Form.Message,
            };
            bizformItem.TourDate = model.Form.DepartureDate;

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
                var pageModel = _repository.GetEnquiryPage(model.TourInfo.PageAlias);

                var viewModel = _mapper.Map<TourEnquirePageViewModel>(pageModel);
                viewModel.Form = _mapper.Map<TourEnquireFormViewModel>(model.Form);

                viewModel.Form.ErrorMessage = "Please fill up mandatory fields.";
                return View("Enquire", viewModel);
            }

            var tour = _tourService.GetTourByTourCode(model.Form.TourCode);

            if (tour?.TourSummaryInfo != null)
            {
                bizformItem.PreferredDestination = tour.TourSummaryInfo.PrimaryCountryName;
                bizformItem.TourLink =
                    $"{(_request.GetRequest().IsSecureConnection ? "https" : "http")}://{_request.GetRequest().Url.Host}{tour.TourSummaryInfo.Path}";
            }

            if (_bizFormEntry.Submit())
            {
                //_kenticoContactService.AddContactForWebsiteConsumer(model.Form.FirstName,
                //    model.Form.LastName,
                //    model.Form.Email, model.Form.Phone, model.Form.PreferredContactType,
                //    model.Form.SubscribeToNewsletter);

                Response.Redirect("/enquire/thank-you");
            }

            return null;
        }

        [HttpPost]
        [ReCaptchaV3Validation]
        [Route("api/tour/submit")]
        public ActionResult SubmitEnquiry(TourEnquirePageViewModel model)
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

                    var viewModel = GetViewModel(model.Form, model.Form.TourCode, model.Form.DepartureDate,
                        model.Form.DepartureClass, model.Form.DepartureCity);
                    ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
                    ViewBag.FormAction = "enquire";
                    TempData["recaptchaKey"] = _apiKeyProvider.RecaptchaV3SiteKey;
                    TempData["form"] = GetViewModel(model.Form, model.Form.TourCode);
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
                ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
                ViewBag.FormAction = "enquire";
                TempData["recaptchaKey"] = _apiKeyProvider.RecaptchaV3SiteKey;
                TempData["form"] = GetViewModel(model.Form, model.Form.TourCode);
                // return RedirectToAction("Enquire",
                //     new { tourcode = model.Form.TourCode }); //View("Enquire", GetViewModel(model.Form));
                
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
                TourCode = model.Form.TourCode,
                TourClass = model.Form.DepartureClass,
                TourDepartureCity = model.Form.DepartureCity,
                TourName = model.Form.TourName,
                TourQuotedPrice = model.Form.QuotedPrice,
                TourDiscountText = model.Form.DiscountText,
                PriceCurrency = model.Form.PriceCurrency.DefaultCurrencyIfBlank(),
                SubscribeToNewsletter = model.Form.SubscribeToNewsletter,
                Message = model.Form.Message,
            };
            bizformItem.TourDate = model.Form.DepartureDate;

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
                // var pageModel = _repository.GetEnquiryPage(model.TourInfo.PageAlias);
                //
                // var viewModel = _mapper.Map<TourEnquirePageViewModel>(pageModel);
                // viewModel.Form = _mapper.Map<TourEnquireFormViewModel>(model.Form);
                //
                // viewModel.Form.ErrorMessage = "Please fill up mandatory fields.";


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

            var tour = _tourService.GetTourByTourCode(model.Form.TourCode);

            if (tour?.TourSummaryInfo != null)
            {
                bizformItem.PreferredDestination = tour.TourSummaryInfo.PrimaryCountryName;
                bizformItem.TourLink =
                    $"{(_request.GetRequest().IsSecureConnection ? "https" : "http")}://{_request.GetRequest().Url.Host}{tour.TourSummaryInfo.Path}";
            }

            if (_bizFormEntry.Submit())
            {
                //_kenticoContactService.AddContactForWebsiteConsumer(model.Form.FirstName,
                //    model.Form.LastName,
                //    model.Form.Email, model.Form.Phone, model.Form.PreferredContactType,
                //    model.Form.SubscribeToNewsletter);

                var response = new GenericApiResponse()
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

        public ActionResult Print(string alias)
        {
            var tour = _repository.Get(string.Empty, alias);
            if (tour == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var tourViewModel = _mapper.Map<TourPageViewModel>(tour);

            var contactDetails = _etgContactService.GetETGContactInfo();
            if (contactDetails != null)
            {
                tourViewModel.TourInfo.TourSummaryInfo.HideIconIcons = true;
                tourViewModel.TourInfo.ExclusivePackageDescription =
                    contactDetails.ExclusiveProductDescription ?? string.Empty;
                tourViewModel.TourInfo.FreedomOfChoiceDescription =
                    contactDetails.FreedomOfChoiceDescription ?? string.Empty;
                tourViewModel.TourInfo.SafeTravelDescription = contactDetails.SafeTravelDescription ?? string.Empty;
                tourViewModel.TourInfo.PeaceOfMindCheckList =
                    string.IsNullOrWhiteSpace(contactDetails.PeachOfMindCheckList)
                        ? null
                        : contactDetails.PeachOfMindCheckList.Contains('\n')
                            ? contactDetails.PeachOfMindCheckList
                                .Split('\n')
                                .Select(a => a.Trim())
                                .ToList()
                            : new List<string>
                                { contactDetails.PeachOfMindCheckList.Trim() };
            }

            if (!string.IsNullOrWhiteSpace(tourViewModel.TourInfo?.TourSummaryInfo?.TypeCodes))
            {
                var arr = tourViewModel.TourInfo.TourSummaryInfo.TypeCodes.Split(';');

                var tourType = _tourTypeRepository.GetTourType(arr[0]);

                tourViewModel.TourInfo.TourTypeDescription = tourType?.TourTypeDescription;
            }

            ViewBag.IsPrint = true;

            return View("~/Views/Tour/Print.cshtml", tourViewModel);
        }

        public ActionResult PrintWithHotels(string alias, string bookingnumber, string hotelIds = "",
            string nights = "", string travelstartdate = "")
        {
            var tour = _repository.Get(string.Empty, alias);
            if (tour == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var contactDetails = _etgContactService.GetETGContactInfo();
            var tourViewModel = _mapper.Map<TourPageViewModel>(tour);

            ViewBag.IsPrint = true;
            tourViewModel.BookingNumber = bookingnumber;
            tourViewModel.PrintHotelIds = hotelIds;
            tourViewModel.PrintNights = nights;

            var startDate = DateTime.MinValue;
            if (DateTime.TryParseExact(travelstartdate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out startDate))
            {
                tourViewModel.HotelPDFStartDate = startDate;
            }

            if (!string.IsNullOrWhiteSpace(hotelIds))
            {
                var ids = hotelIds.Contains(",")
                    ? hotelIds
                        .Split(',')
                        .Select(a => a.ToInteger(0))
                        .ToList()
                    : new List<int> { hotelIds.ToInteger(0) };

                tourViewModel.Hotels =
                    _mapper.Map<List<HotelViewModel>>(_tourExtraDetailService.GetHotelsByHotelIds(ids));
            }

            tourViewModel.TourInfo.PeaceOfMindCheckList = contactDetails.PeachOfMindCheckList.IsNullOrEmpty()
                ? null
                : contactDetails.PeachOfMindCheckList.Split('\n').Select(a => a.Trim()).ToList();

            return View("~/Views/Tour/PrintWithHotels.cshtml", tourViewModel);
        }

        public FileResult GeneratePdf(string tourCode, string force)
        {
            var forceRegenerate = !force.IsNullOrEmpty();
            var tourPdf = _tourPdfProvider.GetTourPdf(tourCode, forceRegenerate);

            if (tourPdf == null)
            {
                throw new HttpException(404, "Page not found");
            }

            Response.AppendHeader("content-disposition", $"inline;filename={tourCode}.pdf");
            return new FileStreamResult(tourPdf.GetFileStream(), "application/pdf");
        }

        public FileResult GenerateUrlPDF(string url)
        {
            var ms = _pdfGenerator.GeneratePDF(url);
            return new FileContentResult(ms.GetBuffer(), "application/pdf");
        }

        public FileResult GeneratePDFWithHotels()
        {
            var stream = Request.InputStream;
            StreamReader sr = new StreamReader(stream);
            JsonSerializer serializer = new JsonSerializer();
            HotelBuilderRequest hotelBuilderRequest =
                (HotelBuilderRequest)serializer.Deserialize(sr, typeof(HotelBuilderRequest));

            if (hotelBuilderRequest == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var tour = _tourService.GetTourByTourCode(hotelBuilderRequest.TourCode);
            if (tour == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var url = URLHelper.GetAbsoluteUrl(
                $"~/tourprintwithhotels/{tour.PageAlias?.ToLower()}/{hotelBuilderRequest.BookingNumber}?hotelIds={hotelBuilderRequest.HotelIdsCommaSeparated}&nights={hotelBuilderRequest.NightsCommaSeparated}&travelstartdate={hotelBuilderRequest.TravelStartDate}",
                SiteContext.CurrentSite.SitePresentationURL);

            var ms = _pdfGenerator.GeneratePDF(url);

            return new FileContentResult(ms.GetBuffer(), "application/pdf");
        }
    }
}