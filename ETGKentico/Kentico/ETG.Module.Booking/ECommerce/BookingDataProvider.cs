using Castle.Core.Internal;
using CMS.Base;
using CMS.Helpers;
using ETG.Booking.Pricing.Enums;
using ETG.Booking.Pricing.Repositories;
using ETG.Data.Helpers;
using ETG.Data.Repositories.Pages;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.Tour.Services;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Constants;
using ETG.Module.Booking.Models.Cart;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ETG.Data.Models.Booking;
using ETG.Data.Tour;

namespace ETG.Module.Booking.ECommerce
{
    public class BookingDataProvider : IBookingDataProvider
    {
        private readonly ITourService _tourService;
        private readonly IBookingCheckoutPageContentsRepository _bookingCheckoutPageContentsRepository;
        private readonly IBookingPriceRepository _bookingPriceRepository;
        private readonly ETGSettings _settings;
        private int TotalPersons => GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT) * 2 + GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEROOMCOUNT);
        
        public BookingDataProvider(IBookingPriceRepository bookingPriceRepository, IETGSettingsService etgSettingsService, 
            IBookingCheckoutPageContentsRepository bookingCheckoutPageContentsRepository, ITourService tourService)
        {
            _bookingPriceRepository = bookingPriceRepository;
            _settings = etgSettingsService.GetSettings();
            _bookingCheckoutPageContentsRepository = bookingCheckoutPageContentsRepository;
            _tourService = tourService;
        }

        private ContainerCustomData ItemCustomData { get; set; }
        private string GetStringCartItemValue(string columnName)
        {
            return ItemCustomData?.GetValue(columnName)?.ToString();
        }


        private int GetIntCartItemValue(string columnName)
        {

            if (ItemCustomData == null)
            {
                return 0;
            }

            return ItemCustomData.GetValue(columnName).ToInteger(0);
        }
        private double GetDoubleCartItemValue(string columnName)
        {

            if (ItemCustomData == null)
            {
                return 0;
            }

            return ValidationHelper.GetDouble(ItemCustomData.GetValue(columnName), 0);
        }
        
        private bool GetBooleanCartItemValue(string columnName)
        {
            if (ItemCustomData == null)
            {
                return false;
            }

            return ItemCustomData.GetValue(columnName).ToBoolean(false);
        }
        private DateTime GetDateTimeCartItemValue(string columnName)
        {
            if (ItemCustomData == null)
            {
                return DateTime.MinValue;
            }
            
            var date = GetStringCartItemValue(columnName);


            if (date == null)
            {
                return DateTime.MinValue;
            }
            DateTime dt;
            if (DateTime.TryParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dt))
            {
                return dt;
            }

            return DateTime.MinValue;
        }
        private string CurrentTourCode => GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURCODE);
        private string CurrentTourName => GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURNAME);
        private int CurrentTourDepositValue => GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURDEPOSITVALUE);
        private int CurrentTourNumberOfNights => GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURNOOFNIGHTS);
        private double CurrentTourDepositPct => GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURDEPOSITPCT);
        private bool CurrentTourHasPeaceOfMind => GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_PEACEOFMIND);

        private bool CurrentTourIsOnSaleNow => GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_ONSALENOW);
        private bool CurrentTourOnSaleFullPaymentRequired => GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_ONSALE_FULLPAYMENTREQUIRED);
        private int CurrentTourChangeOfMindThreshold => GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_CHANGEOFMINDTHRESHOLD);
        private DateTime CurrentPaymentInstalmentDate => GetDateTimeCartItemValue(BookingConstants.CUSTOM_COLUMN_INSTALMENTDATE);
        private DateTime CurrentPaymentBalanceDueDate => GetDateTimeCartItemValue(BookingConstants.CUSTOM_COLUMN_BALANCEDUEDATE);
        private double CurrentInstalmentPercentage => GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SECONDINSTALMENTPERCENTAGE);
        private int GetChangeOfMindThresholdDays()
        {
            return CurrentTourChangeOfMindThreshold > 0
                ? CurrentTourChangeOfMindThreshold
                : _settings.EntireFlexThresholdDays;
        }
        private DateTime CurrentDepartureDate
        {
            get
            {
                if (ItemCustomData == null)
                {
                    return DateTime.MinValue;
                }
                var date = GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_DEPARTUREDATE);


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

        
        private PrePostNightsDetails GetPrePostNightsDetails(string itemKeyName)
        {
            var jsonString = GetStringCartItemValue(itemKeyName);

            if (jsonString.IsNullOrEmpty())
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject<PrePostNightsDetails>(jsonString);
            
        }

        private List<RoomOptionItemBreakdown> GetRoomOptionsSummary()
        {
            var roomData = GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_ROOMOPTIONSUMMARY);

            if (roomData.IsNullOrEmpty())
            {
                return null;
            }
            var roomOptionData = JsonConvert.DeserializeObject<RoomOptionsSummaryCartData>(roomData);
            if (!roomOptionData.RoomOptions.IsNullOrEmpty())
            {
                var roomOptions = new List<RoomOptionItemBreakdown>();
                foreach (var option in roomOptionData.RoomOptions)
                {
                    if (option == null || option.OptionDescription.IsNullOrEmpty())
                    {
                        continue;
                    }
                    var roomOption = roomOptions.FirstOrDefault(a =>
                        a.ItemLabel == option.OptionDescription && a.ItemTypeId == option.OptionType);
                    if (roomOption == null)
                    {
                        roomOptions.Add(new RoomOptionItemBreakdown
                        {
                            Count = option.OptionType == (int)RoomOptionTypeEnum.TwinShareRoomOption ? 2 : 1,
                            ItemLabel = option.OptionDescription,
                            ItemTypeId = option.OptionType,
                            UnitPrice = option.OptionPricePerPerson,
                            PreNightUnitPrice = option.PreNightPricePerPerson,
                            PostNightUnitPrice = option.PostNightPricePerPerson
                        });
                    }
                    else
                    {
                        if (option.OptionType == (int)RoomOptionTypeEnum.TwinShareRoomOption)
                        {

                            roomOption.Count += 2;
                        }
                        else
                        {
                            roomOption.Count++;
                        }
                    }

                }
                

                return roomOptions;
            }

            return null;
        }

        private List<ItemBreakdown> GetExtraOptionsSummary()
        {
            var extrasData = GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_EXTRASSUMMARY);

            if (extrasData.IsNullOrEmpty())
            {
                return null;
            }
            var extras = JsonConvert.DeserializeObject<ExtrasSummaryCartData>(extrasData);
            if (extras.SelectExtrasNow && !extras.Extras.IsNullOrEmpty())
            {
                var extraOptions = new List<ItemBreakdown>();
                foreach (var option in extras.Extras)
                {
                    extraOptions.Add(new ItemBreakdown
                    {
                        Count = TotalPersons,
                        ItemLabel = option.OptionDescription,
                        ItemTypeId = (int) RoomOptionTypeEnum.Extras,
                        UnitPrice = option.OptionPricePerPerson,
                        
                    });

                }

                return extraOptions;
            }

            return null;
        }

        private List<ItemBreakdown> GetAdditionalServicesSummary()
        {
            var additionalServicesString = GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_ADDITIONALSERVICES);

            if (additionalServicesString.IsNullOrEmpty())
            {
                return null;
            }
            
            var additionalServices = JsonConvert.DeserializeObject<AddedServicesCartData>(additionalServicesString);
            
            return additionalServices?.Services;
        
        }
        private bool DoesRequireFullPayment(int numberOfPreNights)
        {
            if (CurrentTourIsOnSaleNow && CurrentTourOnSaleFullPaymentRequired)
            {
                return true;
            }

            var daysFromDepartureDate =
                
                CurrentDepartureDate.Subtract(DateTime.Today).Days - numberOfPreNights;

            if (CurrentTourHasPeaceOfMind)
            {
                if (daysFromDepartureDate <= GetChangeOfMindThresholdDays())
                {
                    return true;
                }
                
                return false;
    
            }
            
            var tour = _tourService.GetTourByTourCode(CurrentTourCode);

            if (tour == null)
            {
                return true;
            }

            var fullPaymentDays = tour.FullPaymentDaysFromDepartureDate;

            if (fullPaymentDays > 0)
            {
                return daysFromDepartureDate <= fullPaymentDays;
            }

            return true;
        }

        public CartBookingSummary GetSummaryData(ContainerCustomData customData, bool isOtherPayment=false)
        {
            ItemCustomData = customData;
            if (ItemCustomData == null || CurrentTourCode.IsNullOrEmpty() || CurrentDepartureDate == DateTime.MinValue)
            {
                return null;
            }

            var tourCode = CurrentTourCode;
            var departureDate = CurrentDepartureDate;
            
            var salesComissionPercent = GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_COMISSIONPCT);

            if (salesComissionPercent == 0)
            {
                salesComissionPercent = _settings.SaleComissionPCT;
            }

            var hasMainHotel = GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURHASMAINHOTEL);
            
            var summary = new CartBookingSummary(salesComissionPercent, new CurrentCurrencyPricing(GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_CURRENCY),
                                                                                                    (decimal) GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_CONVERSION_RATE)))
            {
                TourCode = tourCode,
                TourName = CurrentTourName,
                TourNumberOfNights = CurrentTourNumberOfNights,
                DepartureDate = departureDate,
                CurrencySymbol = "$",
                IsOnSaleNow = CurrentTourIsOnSaleNow,
                OnSaleFullPaymentRequired = CurrentTourOnSaleFullPaymentRequired,
                TourHasPeaceOfMind = CurrentTourHasPeaceOfMind,
                PaymentInstalmentDate = CurrentPaymentInstalmentDate,
                PaymentBalanceDueDate = CurrentPaymentBalanceDueDate,
                ChangeOfMindThresholdDays = CurrentTourChangeOfMindThreshold,
                SecondInstalmentPercentage = CurrentInstalmentPercentage,
                HasMainHotel = hasMainHotel
            };


            var twinSharePrice = GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHAREPRICE);
            var singleSuppCost = GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLESUPPLEMENTPRICE);
            if (twinSharePrice == 0)
            {
                var datePrice = _bookingPriceRepository.GetDepartureBookingPrice(CurrentTourCode, CurrentDepartureDate);

                if (datePrice != null)
                {
                    twinSharePrice = datePrice.TwinSharePrice;
                    singleSuppCost = datePrice.SingleSupplementalCost;
                }
            }
            var roomsType = GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHAREROOMSTYPE);
            summary.SelectedTwinShareRoomsType = roomsType?.Split(',').ToList();
                
            summary.Packages = new ItemBreakdown
            {
                ItemLabel = "Package(s)",
                Count = GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT) * 2,
                UnitPrice = twinSharePrice
            };
            
            summary.SinglePackages = new ItemBreakdown
            {
                ItemLabel = "Single Package(s)",
                Count = GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEROOMCOUNT),
                UnitPrice = twinSharePrice + singleSuppCost
            };

            summary.TwinPreNightBasePrice =
                 GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINPRENIGHTS_BASEPRICE);
            summary.TwinPostNightBasePrice =
                GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINPOSTNIGHTS_BASEPRICE);
            
            summary.SinglePreNightBasePrice =
                GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEPRENIGHTS_BASEPRICE);
            summary.SinglePostNightBasePrice =
                GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEPOSTNIGHTS_BASEPRICE);
            
            summary.PreNightsDetails = GetPrePostNightsDetails(BookingConstants.CUSTOM_COLUMN_PRENIGHTS);
            summary.PostNightsDetails = GetPrePostNightsDetails(BookingConstants.CUSTOM_COLUMN_POSTNIGHTS);

            summary.RoomOptions = GetRoomOptionsSummary();

            if (summary.RoomOptions != null)
            {

                summary.RoomOptions.ForEach(room=>room.UnitPriceWithPrePostNights = GetTotalRoomOptionUnitPrice(hasMainHotel, room, summary.PreNightsDetails, summary.PostNightsDetails));

            }
            
            summary.ExtraOptions = GetExtraOptionsSummary();
            summary.AdditionalServices = GetAdditionalServicesSummary();
            summary.Promotion = GetPromotion();
            summary.AgentDefinedPrice = GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_AGENTPRICE);
            
            string dueCopy;
            
            if (!DoesRequireFullPayment(summary.PreNightsDetails?.NumberOfNights ?? 0) || isOtherPayment)
            {
                var checkoutPage = _bookingCheckoutPageContentsRepository.GetBookingCheckoutPageContents();
                if (!CurrentTourHasPeaceOfMind)
                {
                    dueCopy = checkoutPage?.DueCopyWithoutPeaceOfMind;

                }else
                {
                    dueCopy = checkoutPage?.DueCopyWithPeaceOfMind;

                }
                
                summary.Due = new DueDeposit
                {
                    DueCopy = dueCopy,
                    DepositPrice = new ItemBreakdown
                    {
                        UnitPrice =  GetDepositUnitPrice(summary.TotalPrice, _settings.BookingRequiredDeposit),
                        Count = TotalPersons
                    }, 
                
                };
                summary.BookNowDisclaimer = ResourceHelper.GetString("booknow.disclaimer.default");
            }
            else
            {
                summary.BookNowDisclaimer = ResourceHelper.GetString("booknow.disclaimer.fullpayment");
            }

            summary.CurrentCurrencyPricing = new CurrentCurrencyPricing(
                GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_CURRENCY),
                 (decimal)GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_CONVERSION_RATE));
            return summary;
        }

        private double GetTotalRoomOptionUnitPrice(bool hasMainHotel, RoomOptionItemBreakdown room,
            PrePostNightsDetails preNightsDetails, PrePostNightsDetails postNightsDetails)
        {
            if (room == null)
            {
                return 0;
            }

            //do not consider prepost nights cost for upgrades when a main hotel is ticked, usually for 2 or more hotels in a tour
            if (hasMainHotel)
            {
                return room.UnitPrice;
            }
            
            var price = room.UnitPrice;
            if (preNightsDetails != null && preNightsDetails.NumberOfNights > 0)
            {
                price += room.PreNightUnitPrice * preNightsDetails.NumberOfNights;
            }

            if (postNightsDetails != null && postNightsDetails.NumberOfNights > 0)
            {
                price += room.PostNightUnitPrice * postNightsDetails.NumberOfNights;
            }
            return price;
        }


        private PromotionItem GetPromotion()
        {
            var promotionString = GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_PROMOTION);

            if (promotionString.IsNullOrEmpty())
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject<PromotionItem>(promotionString);
            
        }

        public AddedServicesCartData GetAddedServicesData(ContainerCustomData customData)
        {
            ItemCustomData = customData;
            if (ItemCustomData == null || CurrentTourCode.IsNullOrEmpty() || CurrentDepartureDate == DateTime.MinValue)
            {
                return null;
            }

            var summary = new AddedServicesCartData();
            summary.Services = GetAdditionalServicesSummary();
            return summary;
        }
        private double GetDepositUnitPrice(double totalPrice, double defaultDeposit)
        {
            var fixedDeposit = CurrentTourDepositValue;
            var percentage = CurrentTourDepositPct * .01;
            
            if (fixedDeposit > 0)
            {

                return fixedDeposit;
            }


            if (percentage != 0)
            {
                return Math.Ceiling(totalPrice / TotalPersons * percentage);
            }

            return defaultDeposit;
        }

    }
}
