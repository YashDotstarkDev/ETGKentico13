using Castle.Core.Internal;
using CMS.Base;
using CMS.Ecommerce;
using ETG.Booking.Pricing.Repositories;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.Tour.Services;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Constants;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.Models;
using ETG.Module.Booking.Models.Cart;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CMS.EventLog;
using CMS.Helpers;
using ETG.Booking.Pricing.Models;
using ETG.Booking.Pricing.Services;
using ETG.Core.Promotion;
using ETG.Data.Models.Booking;
using ETG.Data.Promotion;
using ETG.Data.Tour;
using ETG.Module.Booking.Extensions;
using ETG.Module.Booking.Helpers;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.Shopping
{
    public class BookingCart : IBookingCart
    {
        
        private readonly IShoppingService _shoppingService;
        private readonly ITourService _tourService;
        private IBookingPriceRepository _bookingPriceRepository;
        private readonly ETGSettings _settings;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly IDiscountService _discountService;
        private readonly IQuoteAddedServiceProvider _quoteAddedServiceProvider;
        private readonly IPromotionRepository _promotionRepository;
        private readonly CurrentCurrencyPricing _currentCurrencyPricing;
        private readonly ITourExtraDetailService _tourExtraDetailService;
        public BookingCart(ITourService tourService, IShoppingService shoppingService, 
            IBookingPriceRepository bookingPriceRepository, IETGSettingsService etgSettingsService, 
            IBookingDataProvider bookingDataProvider, IDiscountService discountService,
            IQuoteAddedServiceProvider quoteAddedServiceProvider,IPromotionRepository promotionRepository,
            ICurrencyService currencyService, ITourExtraDetailService tourExtraDetailService)
        {
            _shoppingService = shoppingService;
            _tourService = tourService;
            _bookingPriceRepository = bookingPriceRepository;
            _settings = etgSettingsService.GetSettings();
            _bookingDataProvider = bookingDataProvider;
            _discountService = discountService;
            _quoteAddedServiceProvider = quoteAddedServiceProvider;
            _promotionRepository = promotionRepository;
            _tourExtraDetailService = tourExtraDetailService;
            _currentCurrencyPricing = new CurrentCurrencyPricing(currencyService);
        }

        public bool CartIsEmpty => _shoppingService.GetCurrentShoppingCart().IsEmpty;

        public ContainerCustomData CurrentCartItemData
        {
            get
            {
                if (CartIsEmpty)
                {
                    return null;
                }
                return CurrentCart.CartItems[0].CartItemCustomData;
            }
        }
        private ShoppingCartInfo CurrentCart => _shoppingService.GetCurrentShoppingCart();
        public string CurrentTourCode => CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURCODE);
        public string CurrentTourName => CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURNAME);
        public bool CurrentTourHasPeaceOfMind => CurrentCartItemData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_PEACEOFMIND);
        public bool CurrentTourIsOnSaleNow => CurrentCartItemData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_ONSALENOW);
        public bool CurrentTourOnSaleFullPaymentRequired => CurrentCartItemData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_ONSALE_FULLPAYMENTREQUIRED);
        //public bool CurrentTourHasEntireFlex => CurrentCartItemData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_HASENTIREFLEX);
        //public bool CurrentTourFOCEntireFlex => CurrentCartItemData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_FOCENTIREFLEX);
        public bool CurrentCartComplete => CurrentCartItemData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_CARTCOMPLETE);
        public bool CurrentCartRequiredFareAssistance => CurrentCartItemData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_REQUIREFAREASSISTANCE);
        public bool CurrentCartRequiredInsuranceAssistance => CurrentCartItemData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_REQUIREINSURANCEASSISTANCE);
        public int CurrentTourChangeOfMindThreshold => CurrentCartItemData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_CHANGEOFMINDTHRESHOLD);
        public int CurrentTourNumberOfNights  => CurrentCartItemData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURNOOFNIGHTS);

        public bool CurrentCartAvailFlexOption
        {
            get
            {
                var entireFlex =
                    CurrentCartItemData.GetObject<EntireFlexSummaryCartData>(BookingConstants
                        .CUSTOM_COLUMN_ENTIREFLEXSUMMARY);

                if (entireFlex != null)
                {
                    return entireFlex.AvailEntireFlexOption;
                }

                return false;
            }
        }

        public string SessionID => CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_SESSION_ID);
        public int TotalTwinShareRooms => CurrentCartItemData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT);
        public int TotalSingleRooms => CurrentCartItemData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEROOMCOUNT);
        public int TotalRooms => TotalTwinShareRooms +
                                 TotalSingleRooms;

        public bool IsSoloBooking => TotalTwinShareRooms == 0 && TotalSingleRooms == 1;
        public DateTime CurrentDepartureDate
        {
            get
            {
                if (CartIsEmpty)
                {
                    return DateTime.MinValue;
                }
                var date = CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_DEPARTUREDATE);


                if (date == null)
                {
                    return DateTime.MinValue;
                }
                DateTime departureDate;
                if (DateTime.TryParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out departureDate))
                {
                    return departureDate;
                }

                return DateTime.MinValue;
            }
        }

        public int TotalPersons => (CurrentCartItemData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT) * 2) + CurrentCartItemData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEROOMCOUNT);

        public string CurrentCurrency => CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_CURRENCY);
        public decimal ConversionRate =>  (decimal) CurrentCartItemData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_CONVERSION_RATE);
        public void SetPromoCode(string promoCode)
        {
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_PROMOCODE, promoCode);
        }

        public double GetCartTotalPrice()
        {
            var summary = GetBookingSummary();
            return summary.TotalPrice;

        }

       
        public string CurrentTourAliasPath => CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURALIASPATH);
        private ShoppingCartItemInfo CurrentTourCartItem
        {
            get
            {
                if (CartIsEmpty)
                {
                    return null;
                }

                return CurrentCart.CartItems[0];
            }
        }


        private void SetCartItemValue(string columnName, object value)
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }
            var cart = _shoppingService.GetCurrentShoppingCart();
            cart.CartItems[0].CartItemCustomData.SetValue(columnName, value);
            cart.CartItems[0].Update();
        }

        private void SetCartItemObjectValue(string columnName, object value)
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }
            var cart = _shoppingService.GetCurrentShoppingCart();
            cart.CartItems[0].CartItemCustomData.SetValue(columnName, JsonConvert.SerializeObject(value) );
            cart.CartItems[0].Update();
            
        }
        private void RemoveItemFromCartItem(string columnName)
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }
            var cart = _shoppingService.GetCurrentShoppingCart();
            
            cart.CartItems[0].CartItemCustomData.Remove(columnName);
            cart.CartItems[0].Update();
            
        }
        
        private void CreateInitialCartItem(string sessionId, string tourCode, int skuId)
        {
            var tour = _tourService.GetTourByTourCode(tourCode);

            if (tour == null)
            {
                throw new Exception("Invalid tour");
            }
            ClearCart();
            ShoppingCartItemParameters param = new ShoppingCartItemParameters(skuId,1);

            _shoppingService.AddItemToCart(param);
            var changeOfMindThresholdDays = 0;

            if (tour.HasPeaceOfMindGuarantee)
            {
                changeOfMindThresholdDays = tour.ChangeOfMindThresholdDays > 0
                    ? tour.ChangeOfMindThresholdDays
                    : _settings.EntireFlexThresholdDays;
            }
            var cartItem = CurrentTourCartItem;
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_CURRENCY, _currentCurrencyPricing.CurrentCurrency);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_CONVERSION_RATE, _currentCurrencyPricing.ConversionRate);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_SESSION_ID, sessionId);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURCODE, tourCode);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURNOOFNIGHTS, tour.TourSummaryInfo.NoOfNights);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURURL, tour.TourSummaryInfo.Path);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURNAME, tour.TourSummaryInfo.Name);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURDESTINATION, tour.TourSummaryInfo.PrimaryCountryName);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURALIASPATH, tour.TourSummaryInfo.NodeAliasPath);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_PEACEOFMIND, tour.HasPeaceOfMindGuarantee);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_CHANGEOFMINDTHRESHOLD, changeOfMindThresholdDays);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_FREEDOMOFCHOICE, tour.HasFreedomOfChoice);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURDEPOSITVALUE, tour.DepositValue.ToString());
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURDEPOSITPCT, tour.DepositPercentage.ToString());
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_COMISSIONPCT, tour.SaleComissionPercent);
            
            var tourIsOnSale = false;
            var onSaleFullPaymentRequired = false;
            if (tour.TourSummaryInfo.DiscountGuid != Guid.Empty)
            {
                var discount = _discountService.GetDiscount(tour.TourSummaryInfo.DiscountGuid);

                if (discount != null)
                {
                    tourIsOnSale = discount.IsOnSaleNow;
                    onSaleFullPaymentRequired = discount.FullPaymentRequired;
                }
            }
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_ONSALENOW, tourIsOnSale);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_ONSALE_FULLPAYMENTREQUIRED, onSaleFullPaymentRequired);
            
            var hasMainHotel = false;
            var hotels = _tourExtraDetailService.GetHotelsByParentAliasPath(tour.TourSummaryInfo.NodeAliasPath);
            if (!hotels.IsNullOrEmpty())
            {
                cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_TOURHASMAINHOTEL, hotels.Any(h => h.IsMainHotel));
            }
            
            cartItem.Update();

            if (_currentCurrencyPricing.CurrencyAppliesDiscounts)
            {
                var promotion = _promotionRepository.GetPromotionInfoForNonAgent(tour, DateTime.Today);

                if (promotion != null)
                {
                    SetPromotion(promotion);
                }
            }
        }
        
        
        public void AddNonAgentBookingToCart(string sessionId, string tourCode, int skuId, DateTime departureDate, DateBookingPrice dateBookingPrice)
        {
            CreateInitialCartItem(sessionId, tourCode, skuId);
            SetDepartureDateAndPrice(departureDate, dateBookingPrice);
        }
        
        public void AddTravelAgentBookingToCart(string sessionId, string tourCode, int skuId, BookingAgentDetails agentDetails)
        {
            CreateInitialCartItem(sessionId, tourCode, skuId);
            SetAgentDetails(agentDetails);
            
        }

        public double GetBookingPriceSingleSupplementPrice()
        {
            if (CartIsEmpty)
            {
                return 0;
            }
            return CurrentCartItemData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLESUPPLEMENTPRICE);
        }
        
        public void SetDepartureDateAndPrice(DateTime departureDate, DateBookingPrice dateBookingPrice)
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }

            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_DEPARTUREDATE, departureDate.ToString("dd/MM/yyyy"));
            if (dateBookingPrice != null)
            {
                SetCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHAREPRICE, dateBookingPrice.TwinSharePrice);
                SetCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLESUPPLEMENTPRICE, dateBookingPrice.SingleSupplementalCost);
                SetCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINPRENIGHTS_BASEPRICE, dateBookingPrice.TwinSharePreNightPrice);
                SetCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINPOSTNIGHTS_BASEPRICE, dateBookingPrice.TwinSharePostNightPrice);
                SetCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEPRENIGHTS_BASEPRICE, dateBookingPrice.SingleSupplementalPreNightCost);
                SetCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEPOSTNIGHTS_BASEPRICE, dateBookingPrice.SingleSupplementalPostNightCost);
            }

            SetBalanceDueDate(0, dateBookingPrice.DaysToSecondInstalment, dateBookingPrice.DaysToBalanceDueDate,
                departureDate, dateBookingPrice.SecondInstalmentPercentage);

        }


        public void SetPrePostNights(int preNights, int postNights)
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }

            var details = new PrePostNightsDetails
            {
                NumberOfNights = preNights,
                DateFrom = CurrentDepartureDate.AddDays(-preNights),
                DateTo = CurrentDepartureDate
            };

            if (preNights == 0)
            {
                SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_PRENIGHTS, null);   
            }
            else
            {
                SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_PRENIGHTS, details);   
            }

            if (postNights == 0)
            {
                SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_POSTNIGHTS, null);   
            }
            else
            {
                var finalDate = CurrentDepartureDate.AddDays(CurrentTourNumberOfNights);
                details = new PrePostNightsDetails
                {
                    NumberOfNights = postNights,
                    DateFrom = finalDate,
                    DateTo = finalDate.AddDays(postNights),
                };
                SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_POSTNIGHTS, details);
            }

            SetBalanceDueDate(preNights);

        }

        private void SetBalanceDueDate(int preNights)
        {
            var daysToSecondInstalment = CurrentCartItemData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_DAYSTOSECONDINSTALMENT);
            var daysToBalanceDueDate = CurrentCartItemData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_DAYSTOBALANCEDUEDATE);
            var secondInstalmentPercentage =
                CurrentCartItemData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SECONDINSTALMENTPERCENTAGE);

            SetBalanceDueDate(preNights,daysToSecondInstalment, daysToBalanceDueDate, CurrentDepartureDate, secondInstalmentPercentage);
        }

        private void SetBalanceDueDate(int preNights, int daysToSecondInstalment, int daysToBalanceDueDate, 
            DateTime departureDate, double secondInstalmentPercentage)
        {
            
            if (CurrentTourHasPeaceOfMind)
            {
                var instalmentDays = daysToSecondInstalment;
                if (instalmentDays <= 0)
                {
                    instalmentDays = _settings.SecondInstalmentDays;
                }

                var balanceDueDateDays = daysToBalanceDueDate + preNights;

                if (daysToBalanceDueDate == 0)
                {
                    balanceDueDateDays = CurrentTourChangeOfMindThreshold;
                }
                
                var balanceDueDate = departureDate.AddDays(-balanceDueDateDays);
                var instalmentDate = DateTime.Now.Date.AddDays(instalmentDays);
                var secondInstalmenPercentage = secondInstalmentPercentage;

                if (secondInstalmenPercentage <= 0)
                {
                    secondInstalmenPercentage = _settings.SecondInstalmentPercentage;
                }
                if (instalmentDate < balanceDueDate && secondInstalmenPercentage > 0)
                {
                    SetCartItemValue(BookingConstants.CUSTOM_COLUMN_DAYSTOSECONDINSTALMENT, instalmentDays);
                    SetCartItemValue(BookingConstants.CUSTOM_COLUMN_INSTALMENTDATE,
                        instalmentDate.ToString("dd/MM/yyyy"));
                    SetCartItemValue(BookingConstants.CUSTOM_COLUMN_SECONDINSTALMENTPERCENTAGE,
                        secondInstalmenPercentage);
                    
                }

                
                SetCartItemValue(BookingConstants.CUSTOM_COLUMN_DAYSTOBALANCEDUEDATE, balanceDueDateDays);

                if (balanceDueDate > DateTime.Today)
                {
                    SetCartItemValue(BookingConstants.CUSTOM_COLUMN_BALANCEDUEDATE, balanceDueDate.ToString("dd/MM/yyyy")); 
                }    
            }
        }

        public PrePostNightsDetails GetPreNightsDetails()
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }
            
            var details = CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_PRENIGHTS);

            if (details.IsNullOrEmpty())
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject<PrePostNightsDetails>(details);
            
        }

        public PrePostNightsDetails GetPostNightsDetails()
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }
            
            var details = CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_POSTNIGHTS);

            if (details.IsNullOrEmpty())
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject<PrePostNightsDetails>(details);
        }

        public PrePostNightsPriceContainer GetPrePostNightsBasePrices()
        {
            return new PrePostNightsPriceContainer(
                CurrentCartItemData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINPRENIGHTS_BASEPRICE),
                CurrentCartItemData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINPOSTNIGHTS_BASEPRICE),
                CurrentCartItemData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEPRENIGHTS_BASEPRICE),
                CurrentCartItemData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEPOSTNIGHTS_BASEPRICE)
                );
        }

        public string GetPrePostNightsDescription()
        {
            var preNightsDetails = GetPreNightsDetails();
            var postNights = GetPostNightsDetails();
            
            var nightsDetails = new StringBuilder();

            if (preNightsDetails != null)
            {
                nightsDetails.Append(
                    $"+{preNightsDetails.NumberOfNights} Pre Night{(preNightsDetails.NumberOfNights == 1 ? "" : "s")} ({preNightsDetails.DateRangeDisplay})");
            }
        
            if (postNights != null)
            {
                if (preNightsDetails != null)
                {
                    nightsDetails.Append("<br>");
                }
                nightsDetails.Append(
                    $"+{postNights.NumberOfNights} Post Night{(postNights.NumberOfNights == 1 ? "" : "s")} ({postNights.DateRangeDisplay})");
            }

            return nightsDetails.ToString();
        }

        public void SetNoOfRooms(int twinShareRoomCount, int singleRoomCount, string twinShareRoomsType)
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHAREROOMSTYPE, twinShareRoomsType);
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT, twinShareRoomCount);
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEROOMCOUNT, singleRoomCount);
        }

        public double CalculateBasicTotalPrice()
        {
            var tourCode = CurrentTourCode;

            if (tourCode.IsNullOrEmpty())
            {
                throw new Exception("Invalid tour code.");
            }

            var departureDate = CurrentDepartureDate;

            if (departureDate == DateTime.MinValue)
            {
                throw new Exception("Invalid departure date.");
            }

            var datePrice = _bookingPriceRepository.GetDepartureBookingPrice(tourCode, departureDate);

            if (datePrice == null)
            {
                throw new Exception("Cannot find tour code and departure date.");
            }

            var twinSharePrice = datePrice.TwinSharePrice;
            var singleSupplementalCost = datePrice.SingleSupplementalCost;

            if (!_currentCurrencyPricing.CurrentCurrencyIsAUD)
            {
                twinSharePrice = _currentCurrencyPricing.ConvertAUDToCurrentCurrency(datePrice.TwinSharePrice);
                singleSupplementalCost = _currentCurrencyPricing.ConvertAUDToCurrentCurrency(datePrice.SingleSupplementalCost);
            }

            return (TotalTwinShareRooms * 2 * twinSharePrice) +
                   (TotalSingleRooms * (twinSharePrice + singleSupplementalCost));
        }

        public void ClearCart()
        {
            _shoppingService.GetCurrentShoppingCart().ClearData();
        }

        public void SetRoomOptions(List<Guid> roomOptionGuids)
        {
            var options = _bookingPriceRepository.GetBookingOptions(roomOptionGuids);
            var summary = new RoomOptionsSummaryCartData();
            for (var i=0;i < roomOptionGuids.Count;i++)
            {
                if (roomOptionGuids[i] == Guid.Empty)
                {
                    summary.RoomOptions.Add(new RoomOptionCartItem());
                    continue;
                }

                var roomOption = options.FirstOrDefault(a => a.OptionGuid == roomOptionGuids[i]);

                if (roomOption != null)
                {
                    summary.RoomOptions.Add(new RoomOptionCartItem
                    {
                        OptionDescription = roomOption.OptionLabel,
                        OptionPricePerPerson = roomOption.PricePerPerson,
                        OptionType = (int)roomOption.OptionType,
                        PreNightPricePerPerson = roomOption.PreNightPricePerPerson,
                        PostNightPricePerPerson = roomOption.PostNightPricePerPerson
                    });
                }
            }
            SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_ROOMOPTIONSUMMARY, summary);

        }
        
        public double GetRoomOptionsSubTotal()
        {
            var roomData = CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_ROOMOPTIONSUMMARY);

            if (roomData.IsNullOrEmpty())
            {
                return 0;
            }
            
            var roomOptionData = JsonConvert.DeserializeObject<RoomOptionsSummaryCartData>(roomData);

            roomOptionData?.RoomOptions.ForEach(a=>a.OptionPricePerPerson = _currentCurrencyPricing.ConvertAUDToCurrentCurrency(a.OptionPricePerPerson));
            /*var preNightsDetails = GetPreNightsDetails();
            var postNightsDetails = GetPostNightsDetails();
            
            roomOptionData.RoomOptions.ForEach(a=>a.OptionPricePerPerson = GetRoomOptionPricePerPerson(a, preNightsDetails, postNightsDetails));
            */
            return (roomOptionData != null) ? roomOptionData.RoomOptionsSubTotalPrice : 0;
        }

        private double GetRoomOptionPricePerPerson(RoomOptionCartItem roomOption, PrePostNightsDetails preNightsDetails, PrePostNightsDetails postNightsDetails)
        {
            if (roomOption == null)
            {
                return 0;
            }

            var price = roomOption.OptionPricePerPerson;
            if (preNightsDetails != null && preNightsDetails.NumberOfNights > 0)
            {
                price += roomOption.PreNightPricePerPerson * preNightsDetails.NumberOfNights;
            }

            if (postNightsDetails != null && postNightsDetails.NumberOfNights > 0)
            {
                price += roomOption.PostNightPricePerPerson * postNightsDetails.NumberOfNights;
            }
            return _currentCurrencyPricing.ConvertAUDToCurrentCurrency(price);
        }

        public void SetExtras(bool selectExtrasNow, List<Guid> extraOptionGuids)
        {
            var options = _bookingPriceRepository.GetBookingOptions(extraOptionGuids);

            var summary = new ExtrasSummaryCartData
            {
                SelectExtrasNow = selectExtrasNow,
                Extras = !selectExtrasNow ? null : options.Select(option=> new RoomOptionCartItem
                {
                    OptionDescription = !IsSoloBooking ? option.OptionLabel : option.SoloOptionLabel,
                    OptionPricePerPerson = !IsSoloBooking ? option.PricePerPerson : option.SoloPricePerPerson,
                    OptionType = (int)option.OptionType,
                }).ToList()
            };
            if (!selectExtrasNow)
            {
                summary.ExtrasSubTotalPrice = 0;
            }
            else
            {
                summary.ExtrasSubTotalPrice = summary.Extras.Select(a => a.OptionPricePerPerson * TotalPersons).Sum();
            }
            SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_EXTRASSUMMARY, summary);
        }
        
        public double GetExtrasSubTotal()
        {
            var extrasData = CurrentCartItemData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_EXTRASSUMMARY);

            if (extrasData.IsNullOrEmpty())
            {
                return 0;
            }
            
            var extras = JsonConvert.DeserializeObject<ExtrasSummaryCartData>(extrasData);
            
            extras.Extras.ForEach(a=>a.OptionPricePerPerson = _currentCurrencyPricing.ConvertAUDToCurrentCurrency(a.OptionPricePerPerson));

            return extras.Extras.Select(a => a.OptionPricePerPerson * TotalPersons).Sum();

        }

        public void SetShoppingCartNote(string notes)
        {
            CurrentCart.ShoppingCartNote = notes;
            CurrentCart.Update();
        }

        public string GetShoppingCartNote()
        {
            return CurrentCart.ShoppingCartNote;
        }

        public void SetBookNowShoppingCartNotes(string customerComments)
        {
            var notes = new StringBuilder();
            notes.AppendLine(customerComments);
            
            SetShoppingCartNote(notes.ToString().Trim());
            
        }


        public void ClearEntireFlex()
        {
            if (CartIsEmpty)
            {
                throw new Exception("Cart is empty");
            }
            var cart = _shoppingService.GetCurrentShoppingCart();
            cart.CartItems[0].CartItemCustomData.Remove(BookingConstants.CUSTOM_COLUMN_ENTIREFLEXSUMMARY);
            cart.CartItems[0].Update();
        }

        public double SetEntireFlexOption(bool requestAvailEntireFlexOption)
        {
            var data = new EntireFlexSummaryCartData
            {
                AvailEntireFlexOption = requestAvailEntireFlexOption,
                EntireFlexOptionSubTotalPrice = !requestAvailEntireFlexOption
                    ? 0
                    : TotalPersons *  _settings.EntireFlexCostPerPerson
            };
            SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_ENTIREFLEXSUMMARY, data);

            return data.EntireFlexOptionSubTotalPrice;
        }

        public void SetCartComplete(bool cartComplete)
        {
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_CARTCOMPLETE, cartComplete);
            CurrentCart.Update();
        }

        public void SetOtherOptions(bool requireFareAssistance, bool requireTravelInsuranceAssistance)
        {
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_REQUIREFAREASSISTANCE, requireFareAssistance);
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_REQUIREINSURANCEASSISTANCE, requireTravelInsuranceAssistance);
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_CARTCOMPLETE, true);

        }

        public CartBookingSummary GetBookingSummary(int quoteId = 0)
        {
            if (CartIsEmpty)
            {
                return null;
            }
            
            var summary = _bookingDataProvider.GetSummaryData(CurrentCart.CartItems[0].CartItemCustomData);

            if (quoteId > 0)
            {
                summary.AdditionalServices = _quoteAddedServiceProvider.GetAddedServices(quoteId).ToList();
            }

            return summary;
        }

        public OrderInfo CreateOrder(CustomerInfo customer)
        {
            _shoppingService.SetCustomer(customer);
            _shoppingService.SetBillingAddress(new AddressInfo
            {
                AddressLine1 = customer.GetStringValue("CustomerState", string.Empty),
                AddressName = $"{customer.CustomerFirstName} {customer.CustomerLastName}",
                AddressCity = "Online",
                AddressZip = "0000",
                AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}",
                AddressCountryID = 284,
            });
            
            _shoppingService.SetShippingOption(1);
            var cart =_shoppingService.GetCurrentShoppingCart();

            EventLogProvider.ProviderObject.Set(new EventLogInfo("I", "CreateOrder", $"CartEmpty={cart.IsEmpty}" )
            {
                EventDescription = $"{cart.CartItems?.Count}|{cart.Order}|{cart.ShoppingCartID}"
            });
            //_shoppingService.SetPaymentOption(1);
            return _shoppingService.CreateOrder();
        }

        public ExtrasSummaryCartData GetSelectedExtras()
        {
            return CurrentCartItemData.GetObject<ExtrasSummaryCartData>(BookingConstants.CUSTOM_COLUMN_EXTRASSUMMARY);
        }
        
        public PromotionItem GetPromotion()
        {
            return CurrentCartItemData.GetObject<PromotionItem>(BookingConstants.CUSTOM_COLUMN_PROMOTION);
        }

        public void SetPromotion(PromotionInfo promotion)
        {
            var promotionItem = promotion.MapToPromotionItem();
            if (promotionItem != null)
            {
                SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_PROMOTION, promotionItem);
            }
            else
            {
                RemoveItemFromCartItem(BookingConstants.CUSTOM_COLUMN_PROMOTION);
            }
        }

        public void AddAgentPrice(double price)
        {
            SetCartItemValue(BookingConstants.CUSTOM_COLUMN_AGENTPRICE, price);
        }
        
        public double GetAgentPrice()
        {
            return CurrentCartItemData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_AGENTPRICE);
        }


        public FreedomOfChoicesSummaryCartData GetSelectedFreedomOfChoices()
        {
            return CurrentCartItemData.GetObject<FreedomOfChoicesSummaryCartData>(BookingConstants
                .CUSTOM_COLUMN_FREEDOMOFCHOICESSUMMARY);
        }
        public double GetEntireFlexDueAmount()
        {
            var flexData = CurrentCartItemData.GetObject<EntireFlexSummaryCartData>(BookingConstants
                .CUSTOM_COLUMN_ENTIREFLEXSUMMARY);
        
            if (flexData == null)
            {
                return 0;
            }
          
            if (!flexData.AvailEntireFlexOption)
            {
                return 0;
            }

            return _settings.EntireFlexCostPerPerson * TotalPersons;
        }

        public BookingAgentDetails GetAgentDetails()
        {
            return CurrentCartItemData.GetObject<BookingAgentDetails>(BookingConstants.CUSTOM_COLUMN_AGENTDETAILS);
        }

        public void SetAgentDetails(BookingAgentDetails form)
        {
            if (form == null)
            {
                return;
            }
            SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_AGENTDETAILS, form);  
        }

        public void SetFreedomOfChoices(bool selectNow, List<FreedomOfChoiceItem> freedomOfChoices)
        {
            var summary = new FreedomOfChoicesSummaryCartData
            {
                SelectNow = selectNow,
                FreedomOfChoiceItems = freedomOfChoices
            };
            SetCartItemObjectValue(BookingConstants.CUSTOM_COLUMN_FREEDOMOFCHOICESSUMMARY, summary);
            
        }

        /*public bool CheckIfEntireFlexIsOffered()
        {
            return _bookingDataProvider.EntireFlexIsOffered();
        }*/

        public void AddQuoteToCart(BookingQuoteInfo quote)
        {
            var tour = _tourService.GetTourByTourCode(quote.BookingQuoteTourCode);

            if (tour == null)
            {
                throw new Exception("Tour not found");
            }
            
            ClearCart();
            ShoppingCartItemParameters param = new ShoppingCartItemParameters(tour.SkuId, 1);

            _shoppingService.AddItemToCart(param);

            var cartItem = CurrentTourCartItem;
            cartItem.CartItemCustomData.LoadData(quote.BookingQuoteCustomData);
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_QUOTEID, quote.BookingQuoteID);


            var additionalServices = _quoteAddedServiceProvider.GetAddedServices(quote.BookingQuoteID).ToList();

            
            var addedServiceCartData = new AddedServicesCartData
            {
                Services = additionalServices
            };
            cartItem.CartItemCustomData.SetValue(BookingConstants.CUSTOM_COLUMN_ADDITIONALSERVICES,
                JsonConvert.SerializeObject(addedServiceCartData));
            
            cartItem.Update();
            if (!additionalServices.IsNullOrEmpty())
            {
                var agentPrice = GetAgentPrice();

                if (agentPrice > 0)
                {
                    AddAgentPrice(agentPrice + additionalServices.Select(a=>a.TotalPrice).Sum());
                }
            }


        }
        public void AddPromotionForAgent(string agentEmail)
        {
            var promotion = GetPromotion();

            if (promotion == null)
            {
                var tour = _tourService.GetTourByTourCode(CurrentTourCode);
                var promotionInfo = _promotionRepository.GetPromotionInfoForAgent(tour, agentEmail, DateTime.Today);
                if (promotionInfo != null)
                {
                    SetPromotion(promotionInfo);
                }
            }
        }
    }
}
