using Castle.Core.Internal;
using CMS.Base;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using Devotion.Web.Base.Extensions;
using ETG.Data.Helpers;
using ETG.Data.Models.Common;
using ETG.Data.Models.eCommerce;
using ETG.Data.Services;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.Tour.Services;
using ETG.WebAPI.Models;
using ETG.WebAPI.Models.Booking;
using ETG.WebAPI.Models.Booking.Requests;
using ETG.WebAPI.Models.Booking.Responses;
using ETG.WebAPI.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using CMS.Core;
using ETG.Booking.Pricing.Enums;
using ETG.Booking.Pricing.Models;
using ETG.Booking.Pricing.Repositories;
using ETG.Booking.Pricing.Services;
using ETG.Core.Http;
using ETG.Core.Services.Validation;
using ETG.Data.Promotion;
using ETG.Data.Recaptcha;
using ETG.Data.Tour;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.Extensions;
using ETG.Module.Booking.Models;
using ETG.Module.Booking.Models.Steps;
using ETG.Module.Booking.Services;
using ETG.Module.Booking.Shopping;
using ETG.WebAPI.Filters;
using ETG.WebAPI.Services;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("booking")]
    public class BookingController : ApiController
    {
        private readonly ITourService _tourService;
        private readonly IBookingPriceRepository _bookingPriceRepository;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly IBookingCart _bookingCart;
        private readonly ICartService _cartOptionsService;
        private readonly IShoppingService _shoppingService;
        private readonly ETGSettings _settings;
        private readonly IKenticoContactService _contactService;
        private readonly IQuoteService _quoteService;
        private readonly ICurrencyService _currencyService;
        private readonly CurrentCurrencyPricing _currentCurrencyPricing;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IOrderService _orderService;
        //private readonly IRecaptchaValidator _recaptchaValidator;
        private readonly IReCaptchaV3ValidatorService _reCaptchaV3ValidatorService; 
        public BookingController(
            ITourService tourService,
            IBookingPriceRepository bookingPriceRepository,
            IBookingCart bookingCart,
            IKenticoContactService contactService,
            ICartService cartOptionsService,
            IETGSettingsService _etgSettingsService,
            IShoppingService shoppingService,
            IBookingDataProvider bookingDataProvider,
            IQuoteService quoteService,  
            ICurrencyService currencyService,
            IPromotionRepository promotionRepository,
            IOrderService orderService, IRecaptchaValidator recaptchaValidator, IReCaptchaV3ValidatorService reCaptchaV3ValidatorService)
        {
            _bookingCart = bookingCart;
            _tourService = tourService;
            _bookingPriceRepository = bookingPriceRepository;
            _cartOptionsService = cartOptionsService;
            _shoppingService = shoppingService;
            _settings = _etgSettingsService.GetSettings();
            _bookingDataProvider = bookingDataProvider;
            _contactService = contactService;
            _quoteService = quoteService;
            _promotionRepository = promotionRepository;
            _orderService = orderService;
            //_recaptchaValidator = recaptchaValidator;
            _reCaptchaV3ValidatorService = reCaptchaV3ValidatorService;
            _currencyService = currencyService;
            _currentCurrencyPricing = new CurrentCurrencyPricing(_currencyService);
            _promotionRepository = promotionRepository;
        }

        [Route("clearcart")]
        [HttpGet]
        public async Task<IHttpActionResult> ClearCart()
        {
            _bookingCart.ClearCart();
            return Ok(new BaseResponse
            {
                Success = true
            });
        }

        [Route("getdeparturedates")]
        public async Task<IHttpActionResult> GetDepartureDates(string tourcode)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            var tour = _tourService.GetTourByTourCode(tourcode);

            if (tour == null)
            {
                baseResponse.Message = "Invalid Tour code";
                return Ok(baseResponse);
            }

            var response = new GetDepartureDatesResponse
            {
                HasFreedomOfChoice = tour.HasFreedomOfChoice,
                HasPeaceOfMind = tour.HasPeaceOfMindGuarantee,
                DepartureDatesOption = tour.BookingDepartureDatesOptions,
                DepartureEarliestDate = tour.BookingEarliestDepartureDate,
                DepartureLatestDate = tour.BookingLatestDepartureDate,
                CurrencySymbol = _currentCurrencyPricing.CurrentCurrencySymbol
            };

            DateTime? from = null, to = null;

            if (tour.BookingEarliestDepartureDate != DateTime.MinValue)
            {
                from = tour.BookingEarliestDepartureDate;
            }

            if (tour.BookingLatestDepartureDate != DateTime.MinValue)
            {
                to = tour.BookingLatestDepartureDate;
            }

            var bookingPrices = _bookingPriceRepository.GetDepartureBookingPrices(tourcode, from, to);

            var prices = new List<DepartureDateAndPrice>();
            if (!bookingPrices.IsNullOrEmpty())
            {
               
                foreach (var bookingPrice in bookingPrices)
                {
                    for (var dt = bookingPrice.StartDate; dt <= bookingPrice.EndDate; dt = dt.AddDays(1))
                    {
                        if (dt <= DateTime.Now || (from != null && dt < from) || (to != null && dt > to))
                        {
                            continue;
                        }

                        if (!tour.DepartureDaysOfWeek.IsNullOrEmpty())
                        {
                            var intDayOfWeek = (int)dt.DayOfWeek;

                            if (!tour.DepartureDaysOfWeek.Contains(intDayOfWeek))
                            {
                                continue;
                            }
                        }

                        var price = _currentCurrencyPricing.ConvertAUDToCurrentCurrency(bookingPrice.TwinSharePrice);

                        
                        prices.Add(new DepartureDateAndPrice
                        {
                            CurrencySymbol = _currentCurrencyPricing.CurrentCurrencySymbol,
                            DepartureDate = dt,
                            Price = price,
                            HasPrePostNightPrice = bookingPrice.HasPrePostNightPrice
                        });
                    }
                }
            }
            response.DepartureDateAndPriceList = prices;
            response.Success = true;
            return Ok(response);
        }


        [Route("submittravelagentdetails")]
        public async Task<IHttpActionResult> SubmitTravelAgentBookingStep(Dictionary<string, string> param)
        {
            BookingAgentDetails validateAgentParameters()
            {
                if (!param.ContainsKey("agencyName") || !param.ContainsKey("agentName") ||
                    !param.ContainsKey("agentEmail") || !param.ContainsKey("agencyPostcode"))
                {
                    return null;
                }

                var agent = new BookingAgentDetails
                {
                    AgentAgencyName = param["agencyName"],
                    AgentEmail = param["agentEmail"],
                    AgentConsultantName = param["agentName"],
                    AgentPhone = param["agencyPhone"],
                    AgentPostcode = param["agencyPostcode"]
                };

                if (agent.IsValid())
                {
                    return agent;
                }

                return null;
            }

            var baseResponse = new BaseResponse
            {
                Success = false
            };


            var agentDetails = validateAgentParameters();
            
            if (param == null || !param.ContainsKey("sessionId") || !param.ContainsKey("tourCode") || !param.ContainsKey("skuId") ||
                    !param.ContainsKey("tourDays") || agentDetails == null)
            {
                baseResponse.Message = "Invalid parameters";
                return Ok(baseResponse);
            }

            var skuId = param["skuId"].ToInteger();

            if (skuId == 0)
            {
                baseResponse.Message = "Invalid skuId";
                return Ok(baseResponse);
            }

            var tourCode = param["tourCode"];

            _bookingCart.AddTravelAgentBookingToCart(param["sessionId"], tourCode, skuId, agentDetails);
            var response = new BaseResponse
            {
                Success = true
            };

            _bookingCart.SetCartComplete(false);
            return Ok(response);
        }


        private SubmitBookingDateStepResponse GetBookingDateStepResponse(bool hasPrePostNights, int tourDays, string tourCode, DateTime departureDate,
            bool hasSingleSupplementPrice, bool getPrePostNightsResponse)
        {
            var allOptions = _cartOptionsService.GetCartStepsData();
            var allRoomOptions = _bookingPriceRepository.GetBookingOptions(tourCode, departureDate);
            var response = new SubmitBookingDateStepResponse
            {
                SelectedDepartureDisplayDate = departureDate.ToString("dddd, dd MMM yyyy"),
                
                TwinShareOptions = allOptions?.StepTraveller?.TwinShareOptions,
                SingleRoomOptions = allOptions?.StepTraveller?.SingleRoomOptions
            };
            
            if (!response.TwinShareOptions.IsNullOrEmpty())
            {
                response.TwinShareOptions.ForEach(r=> _currentCurrencyPricing.ConvertAUDToCurrentCurrency(r.SupplementalCost));
            }
            
            if (!response.SingleRoomOptions.IsNullOrEmpty())
            {
                response.SingleRoomOptions.ForEach(r=> _currentCurrencyPricing.ConvertAUDToCurrentCurrency(r.SupplementalCost));
            }
            response.HasSingleSupplementOption = hasSingleSupplementPrice;
            if (allOptions != null)
            {
                response.HasTwinShareOptions = !allRoomOptions.TwinShareRoomOptions.IsNullOrEmpty();
                response.HasSingleRoomOptions = !allRoomOptions.SingleRoomOptions.IsNullOrEmpty();
            }

            if (hasPrePostNights)
            {
                string GetDateRangeLabel(DateTime dtFrom, DateTime dtTo)
                {
                    if (dtFrom.Month == dtTo.Month)
                    {
                        return $"{dtFrom.Day} - {dtTo.Day} {dtTo:MMM} {dtTo.Year}";
                    }

                    if (dtFrom.Year != dtTo.Year)
                    {
                        return $"{dtFrom:d MMM yyyy} - {dtTo:d MMM yyyy}";
                    }
                    
                    return $"{dtFrom:d MMM} - {dtTo:d MMM yyyy}";
                }

                //Get Prenights options
                var labels = new List<PrePostNightLabels>();
                for (var i = 1; i <= 10; i++)
                {
                    labels.Add(new PrePostNightLabels
                    {
                        Value = i.ToString(),
                        Label = $"{i} night{(i == 1 ? "" : "s")} ({GetDateRangeLabel(departureDate.AddDays(-i), departureDate)})"
                    });
                }

                response.PreNights = labels;
                    
                //Get Postnights options
                labels = new List<PrePostNightLabels>();
                var finalDate = departureDate.AddDays(tourDays-1);
                for (var i = 1; i <= 10; i++)
                {
                    labels.Add(new PrePostNightLabels
                    {
                        Value = i.ToString(),
                        Label = $"{i} night{(i == 1 ? "" : "s")} ({GetDateRangeLabel(finalDate, finalDate.AddDays(i))})"
                    });
                }
                
                response.PostNights = labels;
            }
            
            return response;
        }

        [Route("submitbookingdatenonagent")]
        public async Task<IHttpActionResult> SubmitNonAgentBookingDateStep(Dictionary<string, string> param)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (param == null || !param.ContainsKey("sessionId") || !param.ContainsKey("tourCode") ||
                !param.ContainsKey("tourDays") || !param.ContainsKey("skuId") || !param.ContainsKey("departureDate"))
            {
                baseResponse.Message = "Invalid parameters";
                return Ok(baseResponse);
            }

            var skuId = param["skuId"].ToInteger();

            if (skuId == 0)
            {
                baseResponse.Message = "Invalid skuId";
                return Ok(baseResponse);
            }

            var tourCode = param["tourCode"];
            DateTime departureDate;
            if (!DateTime.TryParseExact(param["departureDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out departureDate))
            {
                baseResponse.Message = "Invalid departure date";
                return Ok(baseResponse);
            }

            var price = _bookingPriceRepository.GetDepartureBookingPrice(tourCode, departureDate);
            _bookingCart.AddNonAgentBookingToCart(param["sessionId"], tourCode, skuId, departureDate, price);
            _bookingCart.SetCartComplete(false);

            var hasPrePostNights = param.ContainsKey("hasPrePostNights") && param["hasPrePostNights"] == "1";

            var singleSupplementPrice = _bookingCart.GetBookingPriceSingleSupplementPrice();
            var response = GetBookingDateStepResponse(hasPrePostNights, param["tourDays"].ToInteger(0), _bookingCart.CurrentTourCode, _bookingCart.CurrentDepartureDate, singleSupplementPrice > 0, false);
            response.Success = true;
            return Ok(response);

        }

        [Route("submitbookingdateagent")]
        public async Task<IHttpActionResult> SubmitAgentBookingDateStep(Dictionary<string, string> param)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (param == null || !param.ContainsKey("departureDate"))
            {
                baseResponse.Message = "Invalid parameters";
                return Ok(baseResponse);
            }
            
            var tourCode = param["tourCode"];
            DateTime departureDate;
            if (!DateTime.TryParseExact(param["departureDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out departureDate))
            {
                baseResponse.Message = "Invalid departure date";
                return Ok(baseResponse);
            }

            var hasPrePostNights = param.ContainsKey("hasPrePostNights") && param["hasPrePostNights"] == "1";

            var price = _bookingPriceRepository.GetDepartureBookingPrice(tourCode, departureDate);
            _bookingCart.SetDepartureDateAndPrice(departureDate, price);
            _bookingCart.SetCartComplete(false);


            var singleSupplementPrice = _bookingCart.GetBookingPriceSingleSupplementPrice();
            var response = GetBookingDateStepResponse(hasPrePostNights, param["tourDays"].ToInteger(0), _bookingCart.CurrentTourCode, _bookingCart.CurrentDepartureDate, singleSupplementPrice > 0, false);
            response.Success = true;
            return Ok(response);
            
        }

        
        [Route("submitprepostnights")]
        public async Task<IHttpActionResult> SubmitPrePostNights(Dictionary<string, string> param)
        {

            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (param == null || !param.ContainsKey("preNights") || !param.ContainsKey("postNights") )
            {
                baseResponse.Message = "Invalid parameters";
                return Ok(baseResponse);
            }

            _bookingCart.SetPrePostNights(param["preNights"].ToInteger(0), param["postNights"].ToInteger(0));

            var nightsLabel = _bookingCart.GetPrePostNightsDescription();
            
            var response = new SubmitPrePostResponse
            {
                SelectedPrePost = nightsLabel
            };
            
            _bookingCart.SetCartComplete(false);
            return Ok(response);
        }


        [Route("submittravellercount")]
        public async Task<IHttpActionResult> SubmitNumberOfTravellerStep(Dictionary<string, string> param)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (param == null || !param.ContainsKey("twinShareOptions") || !param.ContainsKey("singleRoomOptions"))
            {
                baseResponse.Message = "Invalid parameters";
                return Ok(baseResponse);
            }

            var twinShareRoomCount = param["twinShareOptions"].ToInteger();
            var singleRoomCount = param["singleRoomOptions"].ToInteger();
            var twinRoomsType = param.ContainsKey("selectedRoomTypes") ? param["selectedRoomTypes"] : "";
            if (twinShareRoomCount == 0 && singleRoomCount == 0)
            {
                baseResponse.Message = "Travellers cannot be zero";
                return Ok(baseResponse);
            }

            if (twinShareRoomCount + singleRoomCount > 9)
            {
                baseResponse.Message = "Travellers cannot be more than 9";
                return Ok(baseResponse);
            }

            _bookingCart.SetNoOfRooms(twinShareRoomCount, singleRoomCount, twinRoomsType);
            _bookingCart.SetPrePostNights(0,0);
            var allOptions = _cartOptionsService.GetCartStepsData();
            var response = new SubmitNumberOfTravellerStepResponse
            {
                NumberOfTravellers = twinShareRoomCount * 2 + singleRoomCount,
                CurrencySymbol = _currentCurrencyPricing.CurrentCurrencySymbol,
                TotalPrice = _bookingCart.CalculateBasicTotalPrice(),
                TotalRooms = twinShareRoomCount + singleRoomCount,
                ExtraOptions = allOptions?.StepExtras?.ExtraOptions,
                HasExtras = allOptions != null && allOptions.StepExtras != null &&
                            !allOptions.StepExtras.ExtraOptions.IsNullOrEmpty(),
                RoomOptions = allOptions.StepRoomOptions.RoomOptions
            };

            if (!_currentCurrencyPricing.CurrentCurrencyIsAUD)
            {
                if (!response.RoomOptions.IsNullOrEmpty())
                {
                    response.RoomOptions.ForEach(room => room.Options.ForEach(r =>
                        r.SupplementalCost = _currentCurrencyPricing.ConvertAUDToCurrentCurrency(r.SupplementalCost)));
                }    
                
                if (!response.ExtraOptions.IsNullOrEmpty())
                {
                    response.ExtraOptions.ForEach(r =>
                        r.SupplementalCost = _currentCurrencyPricing.ConvertAUDToCurrentCurrency(r.SupplementalCost));
                }    
            }
            
            
            _bookingCart.SetCartComplete(false);
            response.Success = true;
            return Ok(response);
        }


        [Route("submitroomoptions")]
        public async Task<IHttpActionResult> SubmitRoomOptionsStep(SubmitRoomOptionsRequest request)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (request == null)
            {
                baseResponse.Message = "No options selected.";
                return Ok(baseResponse);
            }

            var selectedGuids = GetRoomGuids(request);
            _bookingCart.SetRoomOptions(selectedGuids);
            
            //reset prepost nights after room options step because prepost nights not available when there is single room upgrade
            _bookingCart.SetPrePostNights(0, 0);
            var totalRoomOptionsPrice = _bookingCart.GetRoomOptionsSubTotal();
            var response = new SubmitRoomOptionsStepResponse
            {
                CurrencySymbol = _currentCurrencyPricing.CurrentCurrencySymbol,
                Rooms = _bookingCart.TotalRooms, 
                RoomOptionSubTotalPrice = totalRoomOptionsPrice,
                Success = true
            };
            return Ok(response);
        }

        private List<Guid> GetRoomGuids(SubmitRoomOptionsRequest request)
        {
            var guids = new List<Guid>();
            Type requestType = request.GetType();

            for (var i = 1; i <= 10; i++)
            {
                if (i > _bookingCart.TotalTwinShareRooms)
                {
                    break;
                }

                var guid =
                    ValidationHelper.GetGuid(requestType.GetProperty($"twinRoomOption{i}")?.GetValue(request),
                        Guid.Empty);

                guids.Add(guid);
            }

            for (var i = 1; i <= 10; i++)
            {
                if (i > _bookingCart.TotalSingleRooms)
                {
                    break;
                }

                var guid =
                    ValidationHelper.GetGuid(requestType.GetProperty($"singleRoomOption{i}")?.GetValue(request),
                        Guid.Empty);

                guids.Add(guid);
            }

            return guids;
        }

        private List<Guid> GetExtrasGuids(SubmitExtrasRequest request)
        {
            var guids = new List<Guid>();
            Type requestType = request.GetType();

            for (var i = 1; i <= 10; i++)
            {
                var guid =
                    ValidationHelper.GetGuid(requestType.GetProperty($"extrasOption{i}")?.GetValue(request),
                        Guid.Empty);

                if (guid != Guid.Empty)
                {
                    guids.Add(guid);
                }
            }


            return guids;
        }

        [Route("submitextraoptions")]
        public async Task<IHttpActionResult> SubmitExtraOptionsStep(SubmitExtrasRequest request)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (request == null)
            {
                baseResponse.Message = "Invalid request.";
                return Ok(baseResponse);
            }

            var selectedGuids = GetExtrasGuids(request);
            _bookingCart.SetExtras(true, selectedGuids);
            var totalExtrasPrice = _bookingCart.GetExtrasSubTotal();
            var response = new SubmitExtraOptionsStepResponse
            {
                CurrencySymbol = _currentCurrencyPricing.CurrentCurrencySymbol,
                ExtrasSubTotalPrice = totalExtrasPrice,
                Success = true
            };
            return Ok(response);
        }

        private List<FreedomOfChoiceItem> GetFreedomOfChoices(SubmitFreedomOfChoiceStepRequest request)
        {
            var choices = new List<FreedomOfChoiceItem>();
            Type requestType = request.GetType();

            for (var i = 1; i <= 14; i++)
            {
                var str =
                    ValidationHelper.GetString(requestType.GetProperty($"dayGroup{i}")?.GetValue(request),
                        string.Empty);

                if (!str.IsNullOrEmpty())
                {
                    var arr = str.Split('|');
                    if (arr.Length == 2)
                    {
                        choices.Add(new FreedomOfChoiceItem
                        {
                            DayCaption = arr[0],
                            OptionLabel = arr[1]
                        });
                    }
                }
            }

            return choices;
        }

        [Route("submitfreedomofchoice")]
        public async Task<IHttpActionResult> SubmitFreedomOfChoiceStep(SubmitFreedomOfChoiceStepRequest request)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (request == null)
            {
                baseResponse.Message = "Invalid request.";
                return Ok(baseResponse);
            }

            var choices = GetFreedomOfChoices(request);
            _bookingCart.SetFreedomOfChoices(request.SelectNow == "selectnow", choices);

            var response = new SubmitExtraOptionsStepResponse
            {
                CurrencySymbol = _currentCurrencyPricing.CurrentCurrencySymbol,
                Success = true
            };
            return Ok(response);
        }

/*
        [Route("submitentireflex")]
        public async Task<IHttpActionResult> SubmitEntireFlexStep(SubmitEntireFlexStepRequest request)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };
            if (request == null)
            {
                baseResponse.Message = "Invalid request.";
                return Ok(baseResponse);
            }

            var subTotalPrice= _currentCurrencyPricing.ConvertAUDToCurrentCurrency(_bookingCart.SetEntireFlexOption(request.AvailEntireFlexOption));
            var response = new SubmitEntireFlexStepResponse {Success = true, EntireFlexSubTotalPrice = subTotalPrice};
            
            return Ok(response);
        }
*/
        [Route("submitotheroptions")]
        public async Task<IHttpActionResult> SubmitOtherOptionsStep(SubmitOtherOptionsStepRequest request)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            _bookingCart.SetOtherOptions(request.RequireFareAssistance, request.RequireTravelInsuranceAssistance);

            baseResponse.Success = true;
            return Ok(baseResponse);
        }

        [Route("getsummary")]
        public async Task<IHttpActionResult> GetCartSummary()
        {
            var response = new BookingSummaryResponse
            {
                Success = false
            };

            var summary = _bookingCart.GetBookingSummary();
            var message = string.Empty;


            var thresholdDays = _settings.EntireFlexThresholdDays;
            if (_bookingCart.CurrentTourChangeOfMindThreshold > 0)
            {
                thresholdDays = _bookingCart.CurrentTourChangeOfMindThreshold;
            }

            if (summary.Due == null && _bookingCart.CurrentTourHasPeaceOfMind)
            {
                var diff = (summary.DepartureDate - DateTime.Today).Days;
                if (diff > thresholdDays && diff <= 60) // && summary.EntireFlexPricing != null &&
                    // summary.EntireFlexPricing.Count == 0)
                {
                    message = ResourceHelper.GetString("booknow.fullpayment.availentireflex");
                }
            }

            if (summary != null)
            {
                summary.ConvertPricesToCurrentCurrency(_currentCurrencyPricing);
                response.Success = true;
                response.BookingSummary = summary;
                response.Message = message;
            }

            return Ok(response);
        }

        private void UpdateCustomer(ref CustomerInfo customer, CreateOrderRequest request,
            BookingAgentDetails agentDetails = null)
        {
            if (customer == null)
            {
                customer = new CustomerInfo
                {
                    CustomerGUID = Guid.NewGuid()
                };
            }

            customer.CustomerFirstName = request.FirstName;
            customer.CustomerLastName = request.LastName;
            customer.CustomerEmail = request.Email;
            customer.CustomerPhone = request.Phone;
            customer.SetValue("CustomerState", request.State);
            customer.SetValue("CustomerTitle", request.Title);
            customer.SetValue("CustomerMiddleName", request.MiddleName);

            DateTime dt;
            if (DateTime.TryParseExact(request.DateOfBirth, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                customer.SetValue("CustomerDateOfBirth", dt);
            }

            if (agentDetails != null && agentDetails.IsValid())
            {
                customer.SetValue("CustomerAgencyName", agentDetails.AgentAgencyName);
                customer.SetValue("CustomerAgentName", agentDetails.AgentConsultantName);
                customer.SetValue("CustomerAgentEmail", agentDetails.AgentEmail);
                customer.SetValue("CustomerAgencyPostcode", agentDetails.AgentPostcode);
                customer.SetValue("CustomerAgentPhone", agentDetails.AgentPhone);
                customer.SetValue("CustomerAgentComment", agentDetails.Comment);
            }
        }

        private (CustomerInfo, BookingAgentDetails) CreateCustomer(CreateOrderRequest request)
        {
            var customer = _shoppingService.GetCurrentCustomer();
            var agentDetails = _bookingCart.GetAgentDetails();

            UpdateCustomer(ref customer, request, agentDetails);

            return (customer, agentDetails);
        }

        private OrderInfo CreateOrderInternal(CreateOrderRequest request, ref CustomerInfo customer)
        {
            BookingAgentDetails agentDetails;
            (customer, agentDetails) = CreateCustomer(request);
            var order = _bookingCart.CreateOrder(customer);

            AddOrUpdatePassenger(request, customer);

            if (!request.Subscribe.IsNullOrEmpty())
            {
                var contactModel = new ContactInfoModel();
                if (agentDetails != null && agentDetails.IsValid())
                {
                    var names = agentDetails.AgentConsultantName.Split(' ');

                    if (names.Length == 1)
                    {
                        contactModel.FirstName = agentDetails.AgentConsultantName;
                        contactModel.LastName = "-";
                    }
                    else
                    {
                        contactModel.FirstName = string.Join(" ", names.ToList().GetRange(0, names.Length - 1));
                        contactModel.LastName = names[names.Length - 1];
                    }

                    contactModel.Email = agentDetails.AgentEmail;
                    contactModel.Phone = agentDetails.AgentPhone;
                    contactModel.IsAgent = true;
                }
                else
                {
                    contactModel.FirstName = request.FirstName;
                    contactModel.LastName = request.LastName;
                    contactModel.Email = request.Email;
                    contactModel.Phone = request.Phone;
                }

                _contactService.AddContact(contactModel, !request.Subscribe.IsNullOrEmpty());
            }

            return order;
        }

        private void AddOrUpdatePassenger(CreateOrderRequest request, CustomerInfo customer)
        {
            if (customer == null)
            {
                return;
            }

            if (!request.Passenger2FirstName.IsNullOrEmpty())
            {
                var passenger = PassengerInfoProvider.GetPassengers()
                    .WhereEquals(nameof(PassengerInfo.PassengerCustomerID), customer.CustomerID).FirstOrDefault();

                var newPassenger = false;
                if (passenger == null)
                {
                    passenger = new PassengerInfo
                    {
                        PassengerGuid = Guid.NewGuid(),
                        PassengerCustomerID = customer.CustomerID
                    };
                    newPassenger = true;
                }

                passenger.PassengerFirstName = request.Passenger2FirstName;
                passenger.PassengerLastName = request.Passenger2LastName;
                passenger.PassengerMiddleName = request.Passenger2MiddleName;
                passenger.PassengerTitle = request.Passenger2Title;
                passenger.PassengerLastModified = DateTime.Now;
                passenger.PassengerGuid = Guid.NewGuid();
                passenger.PassengerCustomerID = customer.CustomerID;
                DateTime dt;
                if (DateTime.TryParseExact(request.Passenger2DateOfBirth, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    passenger.PassengerDateOfBirth = dt;
                }

                if (newPassenger)
                {
                    passenger.Insert();
                }
                else
                {
                    passenger.Update();
                }
            }
        }

        [Route("createorder")]
        public async Task<IHttpActionResult> CreateOrder(CreateOrderRequest request)
        {
            var agentPrice = ValidationHelper.GetDouble(request.AgentPrice, 0);
            var (utmCampaign, utmContent, utmMedium, utmSource, utmTerm) = GetUtmCookieValues();

            var response = new CreateOrderResponse();
            try
            {
                CustomerInfo customer = null;
                OrderInfo order;

                if (!request.OrderId.IsNullOrEmpty())
                {
                    order = OrderInfoProvider.GetOrderInfo(request.OrderId.ToInteger(0));
                    if (order == null)
                    {
                        response.Message = "Invalid order";
                        return Ok(response);
                    }

                    customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
                    UpdateCustomer(ref customer, request);
                    customer.Update();

                    order.OrderNote = request.Comments;

                    if (order.OrderGrandTotal != (decimal)agentPrice && agentPrice > 0)
                    {
                        order.OrderGrandTotal = (decimal)agentPrice;
                        order.OrderGrandTotalInMainCurrency = (decimal)agentPrice;
                    }

                    order.AddUtmProperties(utmCampaign, utmContent, utmMedium, utmSource, utmTerm);
                    order.Update();
                }
                else
                {
                    if (_bookingCart.CartIsEmpty)
                    {
                        return Ok(response);
                    }

                    if (agentPrice > 0)
                    {
                        _bookingCart.AddAgentPrice(agentPrice);
                    }

                    var bookingSummary = _bookingCart.GetBookingSummary();
                    if (bookingSummary == null)
                    {
                        return Ok(response);
                    }

                    _bookingCart.SetBookNowShoppingCartNotes(request.Comments);

                    order = CreateOrderInternal(request, ref customer);

                    order.AddUtmProperties(utmCampaign, utmContent, utmMedium, utmSource, utmTerm);

                    order.Update();
                }


                var summary = _bookingDataProvider.GetSummaryData(order.GetOrderItemCustomData());

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");

                response.Success = true;
                response.OrderId = order.OrderID;
                response.TravelPayParameters = new TravelPayParameters
                {
                    APIUrl = _settings.ETGPaymentAPIBaseUrl,
                    ApiKey = _settings.ETGPaymentAPIKey,
                    Mode = 0,
                    PaymentAmount = summary.Due != null
                        ? summary.Due.TotalDeposit
                        : (double)order.OrderGrandTotalInMainCurrency,
                    OrderId = order.OrderID,
                    Timestamp = timestamp,
                    MerchantCode = _settings.ETGPaymentMerchantCode,
                };

                var agent = _bookingCart.GetAgentDetails();
                if (agent == null || agent.AgentAgencyName.IsNullOrEmpty())
                {
                    response.TravelPayParameters.CustomerName = customer.CustomerInfoName;
                    response.TravelPayParameters.CustomerEmail = customer.CustomerEmail;
                    response.TravelPayParameters.CustomerReference = customer.CustomerID.ToString();
                }

                response.TravelPayParameters.Fingerprint =
                    GenerateFingerPrint(order, response.TravelPayParameters.PaymentAmount, timestamp);

                return Ok(response);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CREATEORDER", "Error", ex);
                response.Message = ex.Message;
                return Ok(response);
            }
        }

        [Route("createorderother")]
        
        public async Task<IHttpActionResult> CreateOrderOther(CreateOrderRequest request)
        {
            var agentPrice = ValidationHelper.GetDouble(request.AgentPrice, 0);
            var (utmCampaign, utmContent, utmMedium, utmSource, utmTerm) = GetUtmCookieValues();

            var response = new CreateOrderResponse();
            try
            {
                CustomerInfo customer = null;
                OrderInfo order;
                var pendingOrderStatus = OrderStatusInfoProvider.GetOrderStatuses()
                    .WhereEquals(nameof(OrderStatusInfo.StatusName), "PendingPayment").FirstOrDefault();

                if (!request.OrderId.IsNullOrEmpty())
                {
                    order = OrderInfoProvider.GetOrderInfo(request.OrderId.ToInteger(0));
                    if (order == null)
                    {
                        response.Message = "Invalid order";
                        return Ok(response);
                    }
                    
                    customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
                    UpdateCustomer(ref customer, request);
                    customer.Update();

                    order.OrderNote = request.Comments;

                    if (order.OrderGrandTotal != (decimal)agentPrice && agentPrice > 0)
                    {
                        order.OrderGrandTotal = (decimal)agentPrice;
                        order.OrderGrandTotalInMainCurrency = (decimal)agentPrice;
                    }

                    if (pendingOrderStatus != null)
                    {
                        order.OrderStatusID = pendingOrderStatus.StatusID;
                    }
                    order.AddUtmProperties(utmCampaign, utmContent, utmMedium, utmSource, utmTerm);
                    order.Update();

                }
                else
                {
                    if (_bookingCart.CartIsEmpty)
                    {
                        return Ok(response);
                    }

                    if (agentPrice > 0)
                    {
                        _bookingCart.AddAgentPrice(agentPrice);
                    }

                    var bookingSummary = _bookingCart.GetBookingSummary();
                    if (bookingSummary == null)
                    {
                        return Ok(response);
                    }

                    _bookingCart.SetBookNowShoppingCartNotes(request.Comments);

                    order = CreateOrderInternal(request, ref customer);

                    if (pendingOrderStatus != null)
                    {
                        order.OrderStatusID = pendingOrderStatus.StatusID;
                    }
                    order.AddUtmProperties(utmCampaign, utmContent, utmMedium, utmSource, utmTerm);
                    order.Update();
                }
                
                _orderService.SendConfirmationEmail(order, _settings.RefundProtectMemberID);
                var otherPaymentResponse = new CreateOrderOtherResponse
                {
                    Success = true,
                    OrderId = order.OrderID,
                    RedirectUrl = $"/payments/otherpaymentsuccess?oid={order.OrderID}"
                };
                return Ok(otherPaymentResponse);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CREATEORDEROTHER", "Error", ex);
                response.Message = ex.Message;
                return Ok(response);
            }
        }

        [Route("createquote")]
        public async Task<IHttpActionResult> CreateQuote(CreateOrderRequest request)
        {
            var response = new CreateQuoteResponse();

            try
            {
                var agentPrice = ValidationHelper.GetDouble(request.AgentPrice, 0);
   
                var (utmCampaign, utmContent, utmMedium, utmSource, utmTerm) = GetUtmCookieValues();
                var tourCode = string.Empty;

                CustomerInfo customer = null;
                OrderInfo order;
                ContainerCustomData containerCustomData = null;
                BookingAgentDetails agentDetails;

                if (!request.OrderId.IsNullOrEmpty())
                {
                    order = OrderInfoProvider.GetOrderInfo(request.OrderId.ToInteger(0));
                    if (order == null)
                    {
                        response.Message = "Invalid order";
                        return Ok(response);
                    }

                    customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
                    agentDetails = new BookingAgentDetails();

                    if (agentDetails != null && agentDetails.IsValid())
                    {
                        agentDetails.AgentAgencyName = customer.GetStringValue("CustomerAgencyName", string.Empty);
                        agentDetails.AgentConsultantName = customer.GetStringValue("CustomerAgentName", string.Empty);
                        agentDetails.AgentEmail = customer.GetStringValue("CustomerAgentEmail", string.Empty);
                        agentDetails.AgentPostcode = customer.GetStringValue("CustomerAgencyPostcode", string.Empty);
                        agentDetails.AgentPhone = customer.GetStringValue("CustomerAgentPhone", string.Empty);
                        agentDetails.Comment = customer.GetStringValue("CustomerAgentComment", string.Empty);
                    }

                    tourCode = order.GetStringValue("OrderTourCode", string.Empty);
                    containerCustomData = order.GetOrderItemCustomData();
                }
                else
                {
                    if (_bookingCart.CartIsEmpty || _bookingCart.CurrentCartItemData == null)
                    {
                        return Ok(response);
                    }

                    _bookingCart.AddAgentPrice(agentPrice);

                    containerCustomData = _bookingCart.CurrentCartItemData;
                    tourCode = _bookingCart.CurrentTourCode;

                    (customer, agentDetails) = CreateCustomer(request);
                }

                CustomerInfoProvider.SetCustomerInfo(customer);
                if (customer == null)
                {
                    return Ok(response);
                }

                var quote = new BookingQuoteInfo
                {
                    BookingQuoteCustomData = containerCustomData?.ToString(),
                    BookingQuoteCustomerID = customer.CustomerID,
                    BookingQuoteTourCode = tourCode,
                    BookingQuoteCreated = DateTime.Now,
                    BookingQuoteValidDays = _settings.BookingQuoteValidityDays,
                    BookingQuoteCustomerComments = request.Comments,
                    BookingQuoteUtmCampaign = utmCampaign ?? string.Empty,
                    BookingQuoteUtmContent = utmContent ?? string.Empty,
                    BookingQuoteUtmMedium = utmMedium ?? string.Empty,
                    BookingQuoteUtmSource = utmSource ?? string.Empty,
                    BookingQuoteUtmTerm = utmTerm ?? string.Empty
                };

                var summary = _bookingDataProvider.GetSummaryData(containerCustomData);

                quote.BookingQuoteTotalPrice = summary.TotalPrice;
                quote.BookingQuoteNetPrice = summary.TotalNetPrice;
                quote.BookingQuoteCurrency = _currentCurrencyPricing.CurrentCurrency;
                quote.BookingQuoteConversionRate = _currentCurrencyPricing.ConversionRate;
                
                if (agentPrice > 0)
                {
                    quote.BookingQuoteAgentPrice = agentPrice;
                }

                quote.Insert();

                response.Success = true;
                response.QuoteID = quote.BookingQuoteID;

                _quoteService.SendQuoteConfirmationEmail(quote);
                _quoteService.SendQuoteNotificationEmail(quote);

                return Ok(response);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CREATEQuote", "Error", ex);
                response.Message = ex.Message;
                return Ok(response);
            }
        }

        private string GenerateFingerPrint(OrderInfo order, double paymentAmount, string timeStamp)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(
                    $"{_settings.ETGPaymentAPIKey}|{_settings.ETGPaymentAPIUsername}|{_settings.ETGPaymentAPIPassword}|0|{(paymentAmount * 100).ToInteger(0)}|{order.OrderID}|{timeStamp}");

            //var sha1 = SHA1.Create();
            //byte[] hashBytes = sha1.ComputeHash(bytes);
            var sha3 = new Sha3Digest(512);
            sha3.BlockUpdate(bytes, 0, bytes.Length);

            byte[] hashBytes = new byte[64]; // 512 bits = 64 bytes
            sha3.DoFinal(hashBytes, 0);

            return HexStringFromBytes(hashBytes);
        }

        private string GenerateFingerPrint(PaymentInfo payment, double paymentAmount, string timeStamp)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(
                    $"{_settings.ETGPaymentAPIKey}|{_settings.ETGPaymentAPIUsername}|{_settings.ETGPaymentAPIPassword}|0|{(paymentAmount * 100).ToInteger(0)}|P{payment.PaymentID}|{timeStamp}");

            //var sha1 = SHA1.Create();
            //byte[] hashBytes = sha1.ComputeHash(bytes);
            var sha3 = new Org.BouncyCastle.Crypto.Digests.Sha3Digest(512);
            sha3.BlockUpdate(bytes, 0, bytes.Length);

            byte[] hashBytes = new byte[64]; // 512 bits = 64 bytes
            sha3.DoFinal(hashBytes, 0);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }

        [Route("createpayment")]
        [RateLimit(Seconds = RateLimitAttribute.RateLimitInSeconds)]
        public async Task<IHttpActionResult> CreatePayment(CreatePaymentRequest request)
        {
            
            var response = new CreatePaymentResponse();
            if (ModelState.ContainsKey(RateLimitAttribute.ModelStateKey) && ModelState[RateLimitAttribute.ModelStateKey].Errors.Any())
            {
                response.Message = "Rate limit exceeded.";
                return Ok(response); 
            }
            try
            {
                var amount = ValidationHelper.GetDouble(request.Amount, 0);

                if (amount < 0)
                {
                    response.Message = "Amount needs to be more than 0";
                }

                if (request.FirstName.IsNullOrEmpty() || request.LastName.IsNullOrEmpty() ||
                    request.InvoiceReference.IsNullOrEmpty() ||
                    request.Email.IsNullOrEmpty())
                {
                    response.Message = "Invalid input parameter.";
                }

                var recaptchaResponse =
                    _reCaptchaV3ValidatorService.ValidateCaptchaToken(request.RecaptchaResponse, true);
                if (!recaptchaResponse.Success)
                {
                    response.Message = "Invalid token.";
                    EventLogProvider.LogInformation("CreatePayment","RecaptchaError");
                }

                if (!response.Message.IsNullOrEmpty())
                {
                    return Ok(response);
                }

                var paymentInfo = new PaymentInfo()
                {
                    PaymentGuid = Guid.NewGuid(),
                    PaymentCreated = DateTime.Now,
                    PaymentLastModified = DateTime.Now,
                    PaymentFirstName = request.FirstName,
                    PaymentLastName = request.LastName,
                    PaymentEmail = request.Email,
                    PaymentContactNumber = request.Phone,
                    PaymentInvoiceReference = request.InvoiceReference,
                    PaymentAmount = amount,
                };

                paymentInfo.Insert();

                response.Success = true;
                response.PaymentId = paymentInfo.PaymentID;

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");

                response.TravelPayParameters = new TravelPayParameters
                {
                    APIUrl = _settings.ETGPaymentAPIBaseUrl,
                    ApiKey = _settings.ETGPaymentAPIKey,
                    Mode = 0,
                    PaymentAmount = amount,
                    CustomerReference = $"P{paymentInfo.PaymentID}",
                    OrderId = paymentInfo.PaymentID,
                    Timestamp = timestamp,
                    MerchantCode = _settings.ETGPaymentMerchantCode,
                };

                response.TravelPayParameters.CustomerName = request.FirstName + " " + request.LastName;
                response.TravelPayParameters.CustomerEmail = request.Email;

                response.TravelPayParameters.Fingerprint =
                    GenerateFingerPrint(paymentInfo, response.TravelPayParameters.PaymentAmount, timestamp);

                return Ok(response);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CREATEPAYMENT", "Error", ex);
                response.Message = ex.Message;
                return Ok(response);
            }
        }

        private (string campaign, string content, string medium, string source, string term) GetUtmCookieValues()
        {
            var cookieValue = CookieHelper.GetValue("etgUtm");
            var cookieUtm = cookieValue != null
                ? JsonConvert.DeserializeObject<dynamic>(cookieValue)
                : null;

            return (
                cookieUtm?["utm_campaign"],
                cookieUtm?["utm_content"],
                cookieUtm?["utm_medium"],
                cookieUtm?["utm_source"],
                cookieUtm?["utm_term"]);
        }
    }
}