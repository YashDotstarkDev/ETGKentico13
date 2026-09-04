using AutoMapper;
using Castle.Core.Internal;
using CMS.Base;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.SiteProvider;
using Devotion.Web.Base.Extensions;
using ETG.Core.Encryption;
using ETG.Core.Http;
using ETG.Data.Destination.Services;
using ETG.Data.Helpers;
using ETG.Data.Repositories.Modules;
using ETG.Data.Repositories.Pages;
using ETG.Data.Services;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.Tour.Services;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.ECommerce.Payment;
using ETG.Module.Booking.ECommerce.Payment.Models;
using ETG.Module.Booking.Extensions;
using ETG.Module.Booking.Models;
using ETG.Module.Booking.Services;
using ETG.Module.Booking.Shopping;
using ETG.Web.Helpers;
using ETG.Web.Models.Forms;
using ETG.Web.Models.Pages;
using ETG.Web.Models.TourBooking;
using ETG.Web.Services;
using ETG.Web.Tour.BookNow.Models;
using ETG.Web.Tour.Models;
using Newtonsoft.Json;
using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ETG.Booking.Pricing.Services;
using ETG.Web.Tour.Models.DataLayer;
using ETG.Core.PageTypes;
using ETG.Data.Configuration;
using ETG.Data.Promotion;
using ETG.Data.Tour;
using ETG.Module.Booking.Constants;

namespace ETG.Web.Controllers
{
    public class TourBookingController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHttpRequest _httpRequest;
        private readonly IBookingCart _bookingCart;
        private readonly IShoppingService _shoppingService;
        private readonly IPaymentService _paymentService;
        private readonly IBookingCheckoutPageContentsRepository _bookingCheckoutPageContentsRepository;
        private readonly IPaymentPageRepository _paymentPageRepository;
        private readonly IKenticoContactService _contactService;
        private readonly ITourService _tourService;
        private readonly ITourExtraDetailService _tourExtraDetailService;
        private readonly ICartService _cartService;
        private readonly ETGSettings _settings;
        private readonly IDiscountService _discountService;
        private readonly IOrderService _orderService;
        private readonly IQuoteService _quoteService;
        private readonly IQuotePDFService _quotePDFService;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly IRefundProtectService _refundProtectService;
        private readonly IRefundProtectRepository _refundProtectRepository;
        private readonly IPaymentTermsProvider _paymentTermsProvider;
        private readonly IDestinationService _destinationService;
        private readonly IContactService _etgContactService;
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IOrderCancelPageRepository _orderCancelPageRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly CurrentCurrencyPricing _currentCurrencyPricing;
        private readonly IApiKeyProvider _apiKeyProvider;
        
        public TourBookingController(IMapper mapper,
            IBookingCart bookingCart,
            IHttpRequest httpRequest, IShoppingService shoppingService,
            IETGSettingsService _etgSettingsService,
            IBookingCheckoutPageContentsRepository bookingCheckoutPageContentsRepository,
            ITourService tourService, ITourExtraDetailService tourExtraDetailService, IOrderService orderService,
            IBookingDataProvider bookingDataProvider,
            IPaymentService paymentService, IKenticoContactService contactService, ICartService cartService,
            IDiscountService discountService,
            IQuoteService quoteService, IQuotePDFService quotePDFService,
            IPaymentPageRepository paymentPageRepository, IRefundProtectRepository refundProtectRepository,
            IRefundProtectService refundProtectService,
            IPaymentTermsProvider paymentTermsProvider, IDestinationService destinationService,
            IContactService etgContactService, ITourTypeRepository tourTypeRepository,
            IOrderCancelPageRepository orderCancelPageRepository,
            IPromotionRepository promotionRepository,
            ICurrencyService currencyService, IApiKeyProvider apiKeyProvider)
        {
            _mapper = mapper;
            _httpRequest = httpRequest;
            _bookingCart = bookingCart;
            _shoppingService = shoppingService;
            _bookingCheckoutPageContentsRepository = bookingCheckoutPageContentsRepository;
            _paymentService = paymentService;
            _contactService = contactService;
            _tourService = tourService;
            _tourExtraDetailService = tourExtraDetailService;
            _cartService = cartService;
            _discountService = discountService;
            _orderService = orderService;
            _settings = _etgSettingsService.GetSettings();
            _bookingDataProvider = bookingDataProvider;
            _quoteService = quoteService;
            _quotePDFService = quotePDFService;
            _refundProtectService = refundProtectService;
            _paymentPageRepository = paymentPageRepository;
            _etgContactService = etgContactService;
            _tourTypeRepository = tourTypeRepository;
            _paymentTermsProvider = paymentTermsProvider;
            _destinationService = destinationService;
            _refundProtectRepository = refundProtectRepository;
            _orderCancelPageRepository = orderCancelPageRepository;
            _promotionRepository = promotionRepository;
            _apiKeyProvider = apiKeyProvider;

            _currentCurrencyPricing = new CurrentCurrencyPricing(currencyService);
        }

        [HandleError]
        public ActionResult CheckoutForm()
        {
            var quoteID = 0;
            var quoteParameter = QueryHelper.GetText("quoteid", string.Empty);

            if (!quoteParameter.IsNullOrEmpty())
            {
                try
                {
                    quoteID = ETGEncryptor.DecryptString(quoteParameter).ToInteger(0);
                }
                catch
                {
                    Response.Redirect("/quote-invalid");
                }
            }

            if (_bookingCart.CartIsEmpty && quoteID == 0)
            {
                Response.Redirect("/");
            }

            var viewModel = GetCheckoutFormPageViewModel(quoteID);

            return View(viewModel);
        }

        [HandleError]
        public ActionResult QuoteCheckoutForm()
        {
            if (_bookingCart.CartIsEmpty)
            {
                Response.Redirect("/");
            }

            var viewModel = GetCheckoutFormPageViewModel(0);

            return View(viewModel);
        }

        public ActionResult BookNow()
        {
            var path = _httpRequest.GetRequest().Url.PathAndQuery.GetUrlPathOnly();
            var tour = _tourService.GetTourByUrl(path);
            if (tour == null || !tour.BookNowEnabled)
            {
                return View("Partial/Tour/_TourBookNowSidebar", new BookNowViewModel());
            }

            _tourService.AssignOtherDetailsForTiledTour(tour.TourSummaryInfo, true);

            tour.TourSummaryInfo.Promotion = _promotionRepository.GetPromotionInfoForNonAgent(tour, DateTime.Today)
                .MapToPromotionItem();

            if (tour.TourSummaryInfo.DiscountGuid != Guid.Empty)
            {
                tour.TourSummaryInfo.Discount = _discountService.GetDiscount(tour.TourSummaryInfo.DiscountGuid);
            }

            var changeOfMindComputedThreshold = !tour.HasPeaceOfMindGuarantee
                ? 0
                : (tour.ChangeOfMindThresholdDays > 0
                    ? tour.ChangeOfMindThresholdDays
                    : _settings.EntireFlexThresholdDays);
            var viewModel = new BookNowViewModel
            {
                Tour = _mapper.Map<TourViewModel>(tour),
                BookingForm = new BookingAgentFormViewModel(),
                BookNowContents = new BookNowContentsViewModel
                {
                    BookNowDisclaimer = ResourceHelper.GetString("booknow.disclaimer.default"),
                    CopyAboveBookNowButton = ResourceHelper.GetString("booknow.abovebooknowbutton.default")
                },
                EntireFlexAmountPerPerson = _settings.EntireFlexCostPerPerson.ToInteger(0),
                EntireFlexThresholdDays = changeOfMindComputedThreshold,
                HasNoRoomUpgrades = !_tourExtraDetailService.GetTourRoomUpgrades(tour.PageAliasPath).Any(),
                HideFlightsOtherOptions = !tour.TourSummaryInfo.PriceInclusions.IsNullOrEmpty() &&
                                          tour.TourSummaryInfo.PriceInclusions.Contains("Flights")
            };

            //if (tour.TourSummaryInfo.TourCode != _bookingCart.CurrentTourCode || !_bookingCart.CurrentCartComplete || Session.SessionID != _bookingCart.SessionID)
            //{
            if (Request["u"] == null || Request["u"] != "1")
            {
                _bookingCart.ClearCart();
            }
            //}

            viewModel.BookNowStepsDetails = _cartService.GetCartStepsData();
            viewModel.BookingCartIsComplete = _bookingCart.CurrentCartComplete;
            viewModel.DaysFromDepartureDateForFullPayment = changeOfMindComputedThreshold;

            var hotels = _tourExtraDetailService.GetHotelsByParentAliasPath(tour.TourSummaryInfo.NodeAliasPath);
            if (!hotels.IsNullOrEmpty())
            {
                viewModel.BookNowStepsDetails.StepRoomOptions.HotelName =
                    hotels.Where(h => h.IsMainHotel).Select(h => h.Name).FirstOrDefault();
            }

            if (viewModel.BookingCartIsComplete)
            {
                var agent = _bookingCart.GetAgentDetails();

                if (agent != null && !agent.AgentAgencyName.IsNullOrEmpty())
                {
                    viewModel.BookingForm = new BookingAgentFormViewModel
                    {
                        AgentAgencyName = agent.AgentAgencyName,
                        AgentConsultantName = agent.AgentConsultantName,
                        AgentEmail = agent.AgentEmail,
                        AgentPhone = agent.AgentPhone,
                        AgentPostcode = agent.AgentPostcode,
                        Comment = agent.Comment
                    };
                }
            }

            if (viewModel.Tour.TourSummaryInfo.TourIsOnSaleNow &&
                viewModel.Tour.TourSummaryInfo.OnSaleFullpaymentRquired)
            {
                viewModel.BookNowContents.BookNowDisclaimer =
                    ResourceHelper.GetString("booknow.disclaimer.fullpayment");
                viewModel.BookNowContents.CopyAboveBookNowButton =
                    ResourceHelper.GetString("booknow.abovebooknowbutton.onsalenow");
            }

            var choices = _tourExtraDetailService.GetTourFreedomOfChoiceOptions(tour.TourSummaryInfo.NodeAliasPath);

            if (tour.HasFreedomOfChoice)
            {
                var dayFreedomOfChoiceList = new List<DayFreedomOfChoiceViewModel>();
                var currentDay = string.Empty;
                var index = 0;
                foreach (var choice in choices)
                {
                    if (choice.DayCaption != currentDay)
                    {
                        currentDay = choice.DayCaption;
                        index++;
                        dayFreedomOfChoiceList.Add(new DayFreedomOfChoiceViewModel
                        {
                            DayName = $"dayGroup{index}",
                            DayLabel = choice.DayCaption,
                            Options = new List<KeyValuePair<Guid, string>>()
                        });
                    }

                    dayFreedomOfChoiceList[dayFreedomOfChoiceList.Count - 1].Options.Add(
                        new KeyValuePair<Guid, string>(choice.OptionGuid, choice.FreedomOfChoiceOptionName));
                }

                viewModel.BookingFreedomOfChoices = dayFreedomOfChoiceList;
                viewModel.HasFreedomOfChoice = !dayFreedomOfChoiceList.IsNullOrEmpty();
            }

            var contactDetails = _etgContactService.GetETGContactInfo();
            viewModel.Tour.ExclusivePackageDescription = contactDetails.ExclusiveProductDescription;
            viewModel.Tour.FreedomOfChoiceDescription = contactDetails.FreedomOfChoiceDescription;
            viewModel.Tour.SafeTravelDescription = contactDetails.SafeTravelDescription;
            viewModel.Tour.PeaceOfMindCheckList = contactDetails.PeachOfMindCheckList.IsNullOrEmpty()
                ? null
                : contactDetails.PeachOfMindCheckList.Split('\n').Select(a => a.Trim()).ToList();
            if (!viewModel.Tour.TourSummaryInfo.TypeCodes.IsNullOrEmpty())
            {
                var arr = viewModel.Tour.TourSummaryInfo.TypeCodes.Split(';');

                if (!arr.IsNullOrEmpty())
                {
                    var tourType = _tourTypeRepository.GetTourType(arr[0]);

                    viewModel.Tour.TourTypeDescription = tourType?.TourTypeDescription;
                }
            }

            return View("Partial/Tour/_TourBookNowSidebar", viewModel);
        }

        private BookingCheckoutFormViewModel GetCheckoutFormPageViewModel(int quoteId = 0)
        {
            CustomerInfo customerInfo = null;
            var comments = "";

            var agent = _bookingCart.GetAgentDetails();
            if (quoteId > 0)
            {
                var quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(quoteId);
                if (quote == null)
                {
                    Response.Redirect($"/quote-invalid");
                }
                else if (quote.HasExpired())
                {
                    Response.Redirect($"/quote-invalid?qid={quoteId}");
                }

                if (_currentCurrencyPricing.CurrentCurrency != quote.BookingQuoteCurrency)
                {
                    //change currency cookie in javascript
                    return new BookingCheckoutFormViewModel
                    {
                        NewCurrencyCookie = quote.BookingQuoteCurrency
                    };
                }

                var tourForQuote = _tourService.GetTourByTourCode(quote.BookingQuoteTourCode);

                if (tourForQuote == null)
                {
                    Response.Redirect($"/quote-invalid?qid={quoteId}");
                }

                comments = quote.BookingQuoteCustomerComments;
                try
                {
                    _bookingCart.AddQuoteToCart(quote);
                    agent = _bookingCart.GetAgentDetails();
                }
                catch (Exception ex)
                {
                    EventLogProvider.LogException("CheckoutForm", "AddQuoteToCart", ex);
                    Response.Redirect("/quote-invalid");
                }

                if (_currentCurrencyPricing.CurrencyAppliesDiscounts)
                {
                    var promotion = _bookingCart.GetPromotion();
                    if (promotion != null && !promotion.IsValid(DateTime.Today))
                    {
                        _bookingCart.SetPromotion(null);
                    }
                }

                customerInfo = CustomerInfoProvider.GetCustomerInfo(quote.BookingQuoteCustomerID);
            }
            else
            {
                if (_currentCurrencyPricing.CurrencyAppliesDiscounts)
                {
                    if (agent == null)
                    {
                        var tourAgent = _tourService.GetTourByTourCode(_bookingCart.CurrentTourCode);
                        var promotion = _promotionRepository.GetPromotionInfoForNonAgent(tourAgent, DateTime.Today);

                        if (promotion != null)
                        {
                            _bookingCart.SetPromotion(promotion);
                        }
                    }
                    else
                    {
                        _bookingCart.AddPromotionForAgent(agent.AgentEmail);
                    }
                }
            }

            var summary = _bookingCart.GetBookingSummary();
            if (summary == null)
            {
                Response.Redirect("/");
            }

            summary.ConvertPricesToCurrentCurrency(_currentCurrencyPricing);

            var tour = _tourService.GetTourByTourCode(_bookingCart.CurrentTourCode);
            var contactDetails = _etgContactService.GetETGContactInfo();
            var viewModel = new BookingCheckoutFormViewModel
            {
                IsFromQuote = quoteId > 0,
                PageContents = _mapper.Map<BookingCheckoutPageContentsViewModel>(
                    _bookingCheckoutPageContentsRepository.GetBookingCheckoutPageContents()),
                BookingSummary = summary,
                ETGSettings = _settings,
                TourUrl = tour.TourSummaryInfo.Path,
                HasPeaceOfMindGuarantee = tour.HasPeaceOfMindGuarantee,
                PeaceOfMindCheckList = contactDetails == null || contactDetails.PeachOfMindCheckList.IsNullOrEmpty()
                    ? null
                    : contactDetails.PeachOfMindCheckList.Split('\n').Select(a => a.Trim()).ToList(),
                IsAgent = agent != null,
                CustomerForm = new BookingCustomerViewModel
                {
                    FirstName = customerInfo?.CustomerFirstName,
                    LastName = customerInfo?.CustomerLastName,
                    MobilePhone = (agent == null || agent.AgentPhone.IsNullOrEmpty())
                        ? customerInfo?.CustomerPhone
                        : agent?.AgentPhone,
                    State = customerInfo?.GetStringValue("CustomerState", ""),
                    Comments = comments,
                    Email = agent?.AgentEmail
                },
                QuoteID = quoteId,
                CurrentCurrencyPricing = _currentCurrencyPricing
            };

            viewModel.PaymentTermsCopy = _paymentTermsProvider.GetBookingPaymentTerms(summary, tour.PaymentTerms)
                .Replace("[depositvalue]",
                    _currentCurrencyPricing.GetCurrentCurrencyDisplayPrice(
                        _settings.BookingRequiredDeposit.ToInteger(0)));


            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookNowProceedWithAgent(BookingAgentFormViewModel form)
        {
            var agentDetails = new BookingAgentDetails
            {
                AgentAgencyName = form.AgentAgencyName,
                AgentConsultantName = form.AgentConsultantName,
                AgentPostcode = form.AgentPostcode,
                AgentPhone = form.AgentPhone,
                AgentEmail = form.AgentEmail,
                Comment = form.Comment
            };
            _bookingCart.SetAgentDetails(agentDetails);
            if (form.AgentFormOption == "book")
            {
                return new RedirectResult("/booking/checkout");
            }
            else
            {
                return new RedirectResult("/booking/quote");
            }
        }

        public ActionResult BookingSuccess()
        {
            var request = _httpRequest.GetRequest();

            var orderId = ValidationHelper.GetInteger(request["oid"], 0);

            if ( //request.UrlReferrer == null ||  
                orderId <= 0)
            {
                Response.Redirect("/");
            }

            var order = OrderInfoProvider.GetOrderInfo(orderId);

            if (order == null)
            {
                Response.Redirect("/");
            }

            var tour =
                _tourService.GetTourByTourCode(order.GetStringValue("OrderTourCode", string.Empty));

            if (tour == null)
            {
                Response.Redirect("/");
            }

            var checkoutContents = _bookingCheckoutPageContentsRepository.GetBookingCheckoutPageContents();
            var thankYouCopy = string.Empty;

            if (checkoutContents?.ThankYouPageCopy != null)
            {
                thankYouCopy = checkoutContents.ThankYouPageCopy.Replace("{%referenceno%}",
                    orderId.ToString());
            }


            var orderSummary = order.GetOrderSummary();
            orderSummary.ConvertPricesToCurrentCurrency(_currentCurrencyPricing);


            var viewModel = new BookingSuccessViewModel
            {
                IsFullPayment = (order.OrderGrandTotalInMainCurrency - order.GetDecimalValue("OrderAmountPaid", 0)) == 0,
                HeroImage = tour.TourSummaryInfo.Images.FirstOrDefault()?.ImagePath,
                TourCode = tour.TourSummaryInfo.TourCode,
                TourName = tour.TourSummaryInfo.Name,
                ThankYouCopy = thankYouCopy,
                Order = order,
                OrderTotalPriceInCurrentCurrency = orderSummary.AgentDefinedPrice > 0
                    ? orderSummary.AgentDefinedPrice
                    : orderSummary.TotalPrice,
                EcommerceDataLayer = new EcommerceDataLayerRoot
                {
                    Event = "book now",
                    Ecommerce = new Ecommerce
                    {
                        CurrencyCode = _currentCurrencyPricing.CurrentCurrency,
                        BookNow = new Event
                        {
                            Id = order.OrderID.ToString(),
                            Name = tour.TourSummaryInfo.Name,
                            Brand = "entire travel",
                            Category = tour.TourSummaryInfo.PrimaryCountryName,
                            Quantity = 1,
                            Price = order.OrderGrandTotalInMainCurrency.ToString("#.00")
                        }
                    }
                },
                CurrentCurrencyPricing = _currentCurrencyPricing
            };

            return View("BookingSuccess", viewModel);
        }

        public ActionResult OtherPaymentSuccess()
        {
            var request = _httpRequest.GetRequest();

            var orderId = ValidationHelper.GetInteger(request["oid"], 0);

            var paymentPageContents = _paymentPageRepository.GetPaymentPageContents();


            if ( //request.UrlReferrer == null ||  
                orderId <= 0)
            {
                Response.Redirect("/");
            }

            var order = OrderInfoProvider.GetOrderInfo(orderId);

            if (order == null)
            {
                Response.Redirect("/");
            }

            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);


            var tour =
                _tourService.GetTourByTourCode(order.GetStringValue("OrderTourCode", string.Empty));

            if (tour == null)
            {
                Response.Redirect("/");
            }

            var summary = _bookingDataProvider.GetSummaryData(order.GetOrderItemCustomData(), true);
            var checkoutContents = _bookingCheckoutPageContentsRepository.GetBookingCheckoutPageContents();
            var thankYouCopy = string.Empty;

            if (checkoutContents?.ThankYouPageCopy != null)
            {
                thankYouCopy = checkoutContents.ThankYouPageCopy.Replace("{%referenceno%}",
                    orderId.ToString());
            }


            var viewModel = new OtherPaymentSuccessViewModel()
            {
                HeroImage = tour.TourSummaryInfo.Images.FirstOrDefault()?.ImagePath,
                TourCode = tour.TourSummaryInfo.TourCode,
                TourName = tour.TourSummaryInfo.Name,
                ThankYouCopy = checkoutContents?.OtherPaymentThankYouPageCopy,
                Order = order,
                PaymentOptions = paymentPageContents.PaymentOptions,
                TotalDepositDue = summary?.Due.TotalDeposit ?? 0,
                CustomerName = customer.GetStringValue("CustomerAgentName", string.Empty) != string.Empty
                    ? customer.GetStringValue("CustomerAgentName", string.Empty)
                    : customer.CustomerFirstName,
                CurrentCurrencyPricing = _currentCurrencyPricing,
                EcommerceDataLayer = new EcommerceDataLayerRoot
                {
                    Event = "book now agent",
                    Ecommerce = new Ecommerce
                    {
                        CurrencyCode = _currentCurrencyPricing.CurrentCurrency,
                        BookNow = new Event
                        {
                            Id = order.OrderID.ToString(),
                            Name = tour.TourSummaryInfo.Name,
                            Brand = "entire travel",
                            Category = tour.TourSummaryInfo.PrimaryCountryName,
                            Quantity = 1,
                            Price = order.OrderGrandTotalInMainCurrency.ToString("#.00")
                        }
                    }
                },
            };

            return View("OtherPaymentSuccess", viewModel);
        }

        public ActionResult QuoteSuccess()
        {
            var request = _httpRequest.GetRequest();

            var quoteId = ValidationHelper.GetInteger(request["qid"], 0);

            if ( //request.UrlReferrer == null ||  
                quoteId <= 0)
            {
                Response.Redirect("/");
            }

            var quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(quoteId);

            if (quote == null)
            {
                Response.Redirect("/");
            }

            var tour =
                _tourService.GetTourByTourCode(quote.BookingQuoteTourCode);

            if (tour == null)
            {
                Response.Redirect("/");
            }

            var checkoutContents = _bookingCheckoutPageContentsRepository.GetBookingCheckoutPageContents();
            var thankYouCopy = string.Empty;

            if (checkoutContents?.QuoteThankYouPageCopy != null)
            {
                thankYouCopy = checkoutContents.QuoteThankYouPageCopy.Replace("{%referenceno%}",
                    quoteId.ToString());
            }

            var encryptedID = ETGEncryptor.EncryptString(quote.BookingQuoteID.ToString());
            var viewModel = new QuoteSuccessViewModel
            {
                DownloadQuoteUrl = $"/booking/downloadquote?id={HttpContext.Server.UrlEncode(encryptedID)}",
                BookNowUrl = $"/booking/checkout?quoteid={HttpContext.Server.UrlEncode(encryptedID)}",
                HeroImage = tour.TourSummaryInfo.Images.FirstOrDefault()?.ImagePath,
                TourCode = tour.TourSummaryInfo.TourCode,
                TourName = tour.TourSummaryInfo.Name,
                ThankYouCopy = thankYouCopy,
                Quote = quote
            };

            return View("QuoteSuccess", viewModel);
        }

        public ActionResult QuoteInvalid()
        {
            var checkoutContents = _bookingCheckoutPageContentsRepository.GetBookingCheckoutPageContents();

            var viewModel = new QuoteInvalidViewModel
            {
                Message = checkoutContents.QuoteGenericMessage
            };
            var request = _httpRequest.GetRequest();

            var quoteId = ValidationHelper.GetInteger(request["qid"], 0);

            if (quoteId <= 0)
            {
                return View("QuoteInvalid", viewModel);
            }

            var quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(quoteId);

            if (quote == null)
            {
                return View("QuoteInvalid", viewModel);
            }


            var message = string.Empty;

            var tour =
                _tourService.GetTourByTourCode(quote.BookingQuoteTourCode);

            if (tour == null)
            {
                tour = _tourService.GetPreviewableTourByTourCode(quote.BookingQuoteTourCode);

                if (tour == null)
                {
                    return View("QuoteInvalid", viewModel);
                }
                else
                {
                    var destination = _destinationService.GetDestination(tour.TourSummaryInfo.PrimaryCountryGuid);

                    message = checkoutContents.QuoteTourRemovedMessage
                        .Replace("{%countryname%}", tour.TourSummaryInfo.PrimaryCountryName).Replace(
                            "{%destinationurl%}", $"{SiteContext.CurrentSite.SitePresentationURL}{destination.Path}");
                }
            }

            if (quote.HasExpired())
            {
                message = checkoutContents.QuoteExpiredMessage
                    .Replace("{%toururl%}", $"{SiteContext.CurrentSite.SitePresentationURL}{tour.TourSummaryInfo.Path}")
                    .Replace("{%validdays%}", quote.BookingQuoteValidDays.ToString());
            }

            /*var quoteData = quote.GetDetails();

            if (quoteData.TourHasPeaceOfMind)
            {
                if (DateTime.Today > quoteData.DepartureDate.Date.AddDays(-_settings.EntireFlexThresholdDays))
                {
                    message = checkoutContents.QuoteExpiredEntireFlexMessage.Replace("{%toururl%}", $"{SiteContext.CurrentSite.SitePresentationURL}{tour.TourSummaryInfo.Path}");
                }
            }*/

            viewModel.HeroImage = tour.TourSummaryInfo.Images.FirstOrDefault()?.ImagePath;
            viewModel.TourCode = tour.TourSummaryInfo.TourCode;
            viewModel.TourName = tour.TourSummaryInfo.Name;
            viewModel.Message = message;
            viewModel.Quote = quote;
            viewModel.Customer = CustomerInfoProvider.GetCustomerInfo(quote.BookingQuoteCustomerID);
            return View("QuoteInvalid", viewModel);
        }

        public ActionResult BookingProcess()
        {
            var request = _httpRequest.GetRequest();

            var orderId = ValidationHelper.GetInteger(request["CustomerReference"], 0);

            if ( //request.UrlReferrer == null ||  
                orderId <= 0)
            {
                Response.Redirect("/");
            }

            var order = OrderInfoProvider.GetOrderInfo(orderId);

            if (order == null || order.OrderPaymentResult != null)
            {
                Response.Redirect("/");
            }


            var paymentResult = new PaymentResultInfo
            {
                PaymentTransactionID = request["PaymentReference"],
                PaymentDescription = JsonConvert.SerializeObject(new TravelPayJQueryPaymentResult
                {
                    PaymentStatus = QueryHelper.GetInteger("PaymentStatus", 0),
                    PaymentStatusString = request["PaymentStatusString"],
                    PaymentReference = request["PaymentReference"],
                    BaseAmount = QueryHelper.GetDouble("BaseAmount", 0),
                    CustomerFee = QueryHelper.GetDouble("CustomerFee", 0),
                    FundsToMerchant = QueryHelper.GetDouble("FundsToMerchant", 0),
                    ProcessedAmount = QueryHelper.GetDouble("ProcessedAmount", 0),
                    CardNo = request["AccountOrCardNo"],
                    CardType = request["PaymentCard"],
                    FailureCode = request["FailureCode"],
                    FailureReason = request["FailureReason"]
                }),
                PaymentIsCompleted = request["PaymentStatus"] == "3",
                PaymentDate = DateTime.Now,
                PaymentMethodID = 1,
                PaymentIsFailed = request["PaymentStatus"] != "3",
            };

            var paymentSuccess = request["PaymentStatus"] == "3";
            order.UpdateOrderStatus(paymentResult);
            order.OrderStatusID = paymentSuccess ? 2 : 3; //Paid

            if (paymentSuccess)
            {
                order.SetValue("OrderAmountPaid", request["FundsToMerchant"]);
            }

            order.OrderOtherPayments = request.Url.PathAndQuery;
            order.Update();

            if (!paymentSuccess)
            {
                return new RedirectResult($"/payment/failed");
            }

            _orderService.SendConfirmationEmail(order, _settings.RefundProtectMemberID);
            _orderService.SendNotificationEmail(order);


            /*
            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);

            if (customer != null)
            {
                var result = _refundProtectService.PostRefundProtect(order, customer, true);

                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Accepted)
                {
                    var summary = order.GetOrderSummary();
                    _refundProtectRepository.AddRefundProtect(order, summary.DepartureDate);
                }
            }*/

            return new RedirectResult($"/booking/success?oid={order.OrderID}");
        }


        public ActionResult DownloadQuote(bool excludeDepositTerms = false)
        {
            var quoteId = 0;
            var id = QueryHelper.GetText("id", string.Empty);
            try
            {
                quoteId = ETGEncryptor.DecryptString(id).ToInteger(0);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("DownloadQuote", "DecryptID", ex);
                Response.Redirect("/quote-invalid");
            }

            var quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(quoteId);
            if (quote == null)
            {
                Response.Redirect($"/quote-invalid");
            }
            else if (quote.HasExpired())
            {
                Response.Redirect($"/quote-invalid?qid={quoteId}");
            }

            var tourForQuote = _tourService.GetTourByTourCode(quote.BookingQuoteTourCode);

            if (tourForQuote == null)
            {
                tourForQuote = _tourService.GetPreviewableTourByTourCode(quote.BookingQuoteTourCode);

                if (tourForQuote == null)
                {
                    Response.Redirect($"/quote-invalid");
                }
                else
                {
                    var checkoutContents = _bookingCheckoutPageContentsRepository.GetBookingCheckoutPageContents();

                    var viewModel = new QuoteInvalidViewModel
                    {
                        Message = checkoutContents.QuoteGenericMessage
                    };
                    var destination =
                        _destinationService.GetDestination(tourForQuote.TourSummaryInfo.PrimaryCountryGuid);

                    var message = checkoutContents.QuoteTourRemovedMessage
                        .Replace("{%countryname%}", tourForQuote.TourSummaryInfo.PrimaryCountryName).Replace(
                            "{%destinationurl%}", $"{SiteContext.CurrentSite.SitePresentationURL}{destination.Path}");

                    viewModel.HeroImage = tourForQuote.TourSummaryInfo.Images.FirstOrDefault()?.ImagePath;
                    viewModel.TourCode = tourForQuote.TourSummaryInfo.TourCode;
                    viewModel.TourName = tourForQuote.TourSummaryInfo.Name;
                    viewModel.Message = message;
                    viewModel.Quote = quote;
                    viewModel.Customer = CustomerInfoProvider.GetCustomerInfo(quote.BookingQuoteCustomerID);
                    return View("QuoteInvalid", viewModel);
                }
            }

            bool.TryParse(QueryHelper.GetText("excludeDepositTerms", string.Empty), out excludeDepositTerms);

            var ms = _quotePDFService.GenerateQuotePDF(quoteId, excludeDepositTerms);

            if (ms == null)
            {
                Response.Redirect("/quote-invalid");
            }

            return File(ms.ReadAllBytes(), "application/pdf", $"Quote{quoteId}.pdf");
        }

        public ActionResult PaymentProcess()
        {
            var request = _httpRequest.GetRequest();
            var customerReference = QueryHelper.GetText("MerchantUniquePaymentId", "");
            if (customerReference.IndexOf("P") > -1)
            {
                customerReference = customerReference.Substring(1);
            }

            var paymentId = ValidationHelper.GetInteger(customerReference, 0);

            if ( //request.UrlReferrer == null ||  
                paymentId <= 0)
            {
                Response.Redirect("/");
            }

            var payment = PaymentInfoProvider.GetPaymentInfo(paymentId);

            if (payment == null || payment.PaymentAmountPaid > 0)
            {
                Response.Redirect("/");
            }

            var paymentSuccess = request["PaymentStatus"] == "3";
            var paymentResult = new PaymentResultInfo
            {
                PaymentTransactionID = request["PaymentReference"],
                PaymentIsCompleted = request["PaymentStatus"] == "3",
                PaymentDate = DateTime.Now,
                PaymentMethodID = 1,
                PaymentIsFailed = request["PaymentStatus"] != "3",
            };

            payment.PaymentMerchantUniquePaymentId = request["MerchantUniquePaymentId"];
            payment.PaymentReference = request["PaymentReference"];
            payment.PaymentAmountPaid = request["FundsToMerchant"].ToDouble();
            payment.PaymentAdditionalInfo = JsonConvert.SerializeObject(paymentResult);
            payment.PaymentDetails = JsonConvert.SerializeObject(new TravelPayJQueryPaymentResult
            {
                PaymentStatus = QueryHelper.GetInteger("PaymentStatus", 0),
                PaymentStatusString = request["PaymentStatusString"],
                PaymentReference = request["PaymentReference"],
                BaseAmount = QueryHelper.GetDouble("BaseAmount", 0),
                CustomerFee = QueryHelper.GetDouble("CustomerFee", 0),
                FundsToMerchant = QueryHelper.GetDouble("FundsToMerchant", 0),
                ProcessedAmount = QueryHelper.GetDouble("ProcessedAmount", 0),
                CardNo = request["AccountOrCardNo"],
                CardType = request["PaymentCard"],
                FailureCode = request["FailureCode"],
                FailureReason = request["FailureReason"]
            });
            payment.Update();

            if (!paymentSuccess)
            {
                return new RedirectResult($"/payment/failed");
            }

            _orderService.SendConfirmationEmail(payment);
            _orderService.SendNotificationEmail(payment);

            return new RedirectResult($"/payment/success?pid={payment.PaymentID}");
        }

        [HandleError]
        public ActionResult PaymentPage()
        {
            ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
            ViewBag.FormAction = "payment";
            var viewModel = _mapper.Map<PaymentPageContentsViewModel>(_paymentPageRepository.GetPaymentPageContents());
            PageHelper.InitializePageBuilder(HttpContext, viewModel.PaymentPageDocumentID);
            viewModel.ETGSettings = _settings;
            return View(viewModel);
        }

        [HandleError]
        public ActionResult OtherPaymentPage()
        {
            var viewModel = _mapper.Map<PaymentPageContentsViewModel>(_paymentPageRepository.GetPaymentPageContents());
            PageHelper.InitializePageBuilder(HttpContext, viewModel.PaymentPageDocumentID);
            viewModel.ETGSettings = _settings;
            return View(viewModel);
        }

        public ActionResult PaymentSuccess()
        {
            var request = _httpRequest.GetRequest();

            var paymentId = ValidationHelper.GetInteger(request["pid"], 0);

            if ( //request.UrlReferrer == null ||  
                paymentId <= 0)
            {
                Response.Redirect("/");
            }

            var payment = PaymentInfoProvider.GetPaymentInfo(paymentId);

            if (payment == null)
            {
                Response.Redirect("/");
            }

            var contents = _paymentPageRepository.GetPaymentPageContents();
            var thankYouCopy = string.Empty;

            if (contents?.ThankyouPageBodyContent != null)
            {
                thankYouCopy = contents?.ThankyouPageBodyContent.Replace("{%referenceno%}",
                    payment.PaymentMerchantUniquePaymentId.ToString()).Replace("{%invoicereferenceno%}",
                    payment.PaymentInvoiceReference.ToString());
            }


            var viewModel = new ThankyouPaymentPageContentsViewModel
            {
                HeroImage = contents.ThankyouPageHeroImage,
                ThankYouCopy = thankYouCopy,
                Payment = payment
            };

            return View("PaymentSuccess", viewModel);
        }

        public ActionResult PaymentFailed()
        {
            var contents = _paymentPageRepository.GetPaymentPageContents();

            var viewModel = new ThankyouPaymentPageContentsViewModel
            {
                HeroImage = contents?.ThankyouPageHeroImage
            };
            return View("PaymentFailed", viewModel);
        }

        public ActionResult Refundable()
        {
            return View("Refundable");
        }

        public ActionResult ApplyRefund()
        {
            var orderId = 0;
            var id = QueryHelper.GetText("id", string.Empty);
            try
            {
                orderId = ETGEncryptor.DecryptString(id).ToInteger(0);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("ApplyRefund", "DecryptID", ex);
                Response.Redirect("/apply-refund-error");
            }

            var order = OrderInfoProvider.GetOrderInfo(orderId);

            if (order == null)
            {
                Response.Redirect("/apply-refund-error");
            }

            if (order.OrderStatusID != BookingConstants.ORDER_STATUS_PAID)
            {
                Response.Redirect("/apply-refund-error");
            }

            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
            var vm = new ApplyRefundViewModel
            {
                OrderID = order.OrderID,
                PageContents = _mapper.Map<OrderCancelPageViewModel>(_orderCancelPageRepository.GetContents()),
                CustomerFirstName = customer?.CustomerFirstName,
                CustomerLastName = customer?.CustomerLastName
            };

            return View("ApplyRefundPage", vm);
        }

        [HttpPost]
        public ActionResult RedirectToRefundProtectRefundPage(ApplyRefundViewModel applyRefundViewModel)
        {
            var order = OrderInfoProvider.GetOrderInfo(applyRefundViewModel.OrderID);
            var result = _refundProtectService.SendApplyRefundEmailNotificationToAdmin(order);
            var message = "Please check your email to apply for refund";
            if (!result.Success)
            {
                return PartialView("Partial/Common/_DisplayMessage", result.Message);
            }

            return Redirect(
                $"https://form.refundable.me/forms/refund?memberId={_settings.RefundProtectMemberID}&bookingReference={order.OrderID}");
        }
    }
}