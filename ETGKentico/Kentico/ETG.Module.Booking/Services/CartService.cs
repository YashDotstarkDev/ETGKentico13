using Castle.Core.Internal;
using CMS.EventLog;
using ETG.Booking.Pricing.Enums;
using ETG.Booking.Pricing.Repositories;
using ETG.Data.Extensions;
using ETG.Module.Booking.Helpers;
using ETG.Module.Booking.Models;
using ETG.Module.Booking.Models.Cart;
using ETG.Module.Booking.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using ETG.Booking.Pricing.Services;
using ETG.Core.PageTypes;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.Tour;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Repositories;
using ETG.Module.Booking.Models.Steps;

namespace ETG.Module.Booking.Services
{
    public class CartService :ICartService
    {
        private readonly IBookingPriceRepository _bookingPriceRepository;
        private readonly IBookingCart _bookingCart;
        private readonly ETGSettings _settings;
        private readonly ITourProductRepository _tourProductRepository;
        private readonly CurrentCurrencyPricing _currentCurrencyPricing;
        public CartService(IBookingPriceRepository bookingPriceRepository, IBookingCart bookingCart, IETGSettingsService etgSettingsService, ITourProductRepository tourProductRepository, ICurrencyService currencyService)
        {
            _bookingPriceRepository = bookingPriceRepository;
            _bookingCart = bookingCart;
            _tourProductRepository = tourProductRepository;
            _settings = etgSettingsService.GetSettings();
            _currentCurrencyPricing = new CurrentCurrencyPricing(currencyService);
        }

        public BookNowStepsDetailsModel GetCartStepsData()
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
            
            if (_bookingCart.CartIsEmpty)
            {

                return new BookNowStepsDetailsModel();
            }

            try
            {
                var cartSummary = _bookingCart.GetBookingSummary();

                var tourCode = _bookingCart.CurrentTourCode;
                var departureDate = _bookingCart.CurrentDepartureDate;

                var bookingPrice = _bookingPriceRepository.GetDepartureBookingPrice(tourCode, departureDate);

                if (bookingPrice == null)
                {
                    _bookingCart.ClearCart();
                    return new BookNowStepsDetailsModel();
                }
                var allRoomOptions = _bookingPriceRepository.GetBookingOptions(tourCode, departureDate);
                var booknowStepsDetails = new BookNowStepsDetailsModel();
                booknowStepsDetails.StepDateSelect.SelectedDate = departureDate;
                booknowStepsDetails.StepDateSelect.SubLabel= departureDate.ToString("dddd, dd MMM yyyy");
                booknowStepsDetails.StepTraveller.SubLabel = $"{cartSummary.TotalPersons} Traveller{(cartSummary.TotalPersons >1 ? "s" : "")} ({_bookingCart.CalculateBasicTotalPrice().FormatPrice(false, _currentCurrencyPricing.CurrentCurrencySymbol)})";
                
                if (cartSummary != null && cartSummary.RoomOptionsTotalPrice > 0)
                {
                    booknowStepsDetails.StepRoomOptions.SubLabel = $"+{cartSummary.RoomOptions.Select(a=>a.TotalPrice).Sum().FormatPrice(false, _currentCurrencyPricing.CurrentCurrencySymbol)}";
                }
                else
                {
                    booknowStepsDetails.StepRoomOptions.SubLabel = "";
                }

                if (cartSummary != null && (
                        (cartSummary.PreNightsDetails != null && cartSummary.PreNightsDetails.NumberOfNights > 0) 
                        || (cartSummary.PostNightsDetails != null && cartSummary.PostNightsDetails.NumberOfNights > 0)))
                {
                    booknowStepsDetails.StepPrePostNights.SubLabel = _bookingCart.GetPrePostNightsDescription();

                    var selectedPreNight =
                        cartSummary.PreNightsDetails != null && cartSummary.PreNightsDetails.NumberOfNights > 0
                            ? cartSummary.PreNightsDetails.NumberOfNights
                            : 0;
                    
                    var selectedPostNight =
                        cartSummary.PostNightsDetails != null && cartSummary.PostNightsDetails.NumberOfNights > 0
                            ? cartSummary.PostNightsDetails.NumberOfNights
                            : 0;
                    
                    var labels = new List<PrePostNightLabels>();
                    for (var i = 1; i <= 10; i++)
                    {
                        labels.Add(new PrePostNightLabels
                        {
                            Value = i.ToString(),
                            Label = $"{i} night{(i == 1 ? "" : "s")} ({GetDateRangeLabel(departureDate.AddDays(-i), departureDate)})",
                            IsSelected = (i == selectedPreNight)
                        });
                    }

                    booknowStepsDetails.StepPrePostNights.PreNightsOptions = labels;
                    
                    labels = new List<PrePostNightLabels>();
                    var finalDate = departureDate.AddDays(cartSummary.TourNumberOfNights);
                    for (var i = 1; i <= 10; i++)
                    {
                        labels.Add(new PrePostNightLabels
                        {
                            Value = i.ToString(),
                            Label = $"{i} night{(i == 1 ? "" : "s")} ({GetDateRangeLabel(finalDate, finalDate.AddDays(i))})",
                            IsSelected = (i == selectedPostNight)
                        });
                    }
                
                    booknowStepsDetails.StepPrePostNights.PostNightsOptions = labels;
                    
                }
                
                if (cartSummary != null && cartSummary.ExtraOptionsTotalPrice > 0)
                {
                    booknowStepsDetails.StepExtras.SubLabel = $"+{cartSummary.ExtraOptionsTotalPrice.FormatPrice(false, _currentCurrencyPricing.CurrentCurrencySymbol)}";
                }
                else
                {
                    booknowStepsDetails.StepExtras.SubLabel = string.Empty;
                }

                for (var i = 1; i <= 4; i++)
                {
                    booknowStepsDetails.StepTraveller.TwinShareOptions.Add(new TravellersCountOption
                    {
                        Label = $"{i} ({i * 2} people)",
                        NumberOfPersons = i * 2,
                        NumberOfRooms = i,
                        IsSelected = _bookingCart.TotalTwinShareRooms == i
                    });
                }

                booknowStepsDetails.StepTraveller.SelectedTwinShareRoomsType = cartSummary.SelectedTwinShareRoomsType;

                var singleRoomPrice = bookingPrice.SingleSupplementalCost;

                if (singleRoomPrice > 0)
                {
                    for (var i = 1; i <= 9; i++)
                    {
                        booknowStepsDetails.StepTraveller.SingleRoomOptions.Add(new TravellersCountOption
                        {
                            Label = $"{i} (+{((double)_currentCurrencyPricing.ConvertAUDToCurrentCurrency(i * singleRoomPrice)).FormatPrice(false, _currentCurrencyPricing.CurrentCurrencySymbol)} )",
                            NumberOfPersons = i,
                            NumberOfRooms = i,
                            IsSelected = _bookingCart.TotalSingleRooms == i
                        });
                    }
                }
                else
                {
                    booknowStepsDetails.StepTraveller.HideSingleRoomDropdown = true;
                }

                
                if (!allRoomOptions.TwinShareRoomOptions.IsNullOrEmpty())
                {
                    for (var i = 1; i <= _bookingCart.TotalTwinShareRooms; i++)
                    {
                        booknowStepsDetails.StepRoomOptions.RoomOptions.Add(new RoomOptionDropdown
                        {
                            FieldName = $"twinRoomOption{i}",
                            FieldLabel = $"Room option {i}",
                            Options = allRoomOptions.TwinShareRoomOptions.Select(a => new RoomOption
                            {
                                Label = a.OptionLabel,
                                SupplementalCost = a.PricePerPerson,
                                OptionGuid = a.OptionGuid,
                                IsSelected = cartSummary?.RoomOptions == null ? false : IsRoomOptionSelected(i - 1, cartSummary.RoomOptions, a.OptionLabel)

                            }).ToList()
                        });
                    }
                }
                
                if (!allRoomOptions.SingleRoomOptions.IsNullOrEmpty())
                {
                    for (var i = 1 + _bookingCart.TotalTwinShareRooms;
                    i <= _bookingCart.TotalSingleRooms + _bookingCart.TotalTwinShareRooms;
                    i++)
                    {
                        booknowStepsDetails.StepRoomOptions.RoomOptions.Add(new RoomOptionDropdown
                        {
                            FieldName = $"singleRoomOption{i - _bookingCart.TotalTwinShareRooms}",
                            FieldLabel = $"Room option {i} (solo traveller)",
                            Options = allRoomOptions.SingleRoomOptions.Select(a => new RoomOption
                            {
                                Label = a.OptionLabel,
                                SupplementalCost = a.PricePerPerson,
                                OptionGuid = a.OptionGuid,
                                IsSelected = cartSummary?.RoomOptions != null && IsRoomOptionSelected(i - 1, cartSummary.RoomOptions, a.OptionLabel)
                            }).ToList()
                        });
                    }
                }

                booknowStepsDetails.StepRoomOptions.IsHidden = !booknowStepsDetails.StepRoomOptions.HasRoomOptions;
                var extrasSummaryData = _bookingCart.GetSelectedExtras();

                if (allRoomOptions.ExtraOptions.IsNullOrEmpty() || (_bookingCart.CurrentCartComplete && extrasSummaryData == null))
                {
                    booknowStepsDetails.StepExtras.IsHidden = true;
                }
                else
                {
                    if (extrasSummaryData != null)
                    {

                        booknowStepsDetails.StepExtras.ExtrasSelectNow = extrasSummaryData.SelectExtrasNow;
                    }

                    var tourAdditions = _tourProductRepository.GetTourBookingAdditions(new List<string>
                    {
                        _bookingCart.CurrentTourCode
                    });

                    List<OptionalExtras> extrasInCMSTree = null;
                    if (!tourAdditions.IsNullOrEmpty())
                    {
                        extrasInCMSTree = tourAdditions[0].OptionalExtras;
                    }
                    
                    booknowStepsDetails.StepExtras.ExtraOptions = new List<RoomOption>();
                    for (var i = 0; i < allRoomOptions.ExtraOptions.Count; i++)
                    {
                        if (!ExtrasIsAvailable( allRoomOptions.ExtraOptions[i].OptionLabel, extrasInCMSTree))
                        {
                            continue;
                        }
                        
                        booknowStepsDetails.StepExtras.ExtraOptions.Add(new RoomOption
                        {
                            Label = !_bookingCart.IsSoloBooking ? allRoomOptions.ExtraOptions[i].OptionLabel : allRoomOptions.ExtraOptions[i].SoloOptionLabel,
                            SupplementalCost =  !_bookingCart.IsSoloBooking ? allRoomOptions.ExtraOptions[i].PricePerPerson : allRoomOptions.ExtraOptions[i].SoloPricePerPerson,
                            OptionGuid = allRoomOptions.ExtraOptions[i].OptionGuid,
                            FieldName = $"extrasOption{i + 1}",
                            IsSelected = extrasSummaryData == null || extrasSummaryData.Extras.IsNullOrEmpty() ? false : extrasSummaryData.Extras.Any(a =>
                                a.OptionDescription == allRoomOptions.ExtraOptions[i].OptionLabel)
                        });
                    }
                }
                
                var freedomOfChoiceSummaryData = _bookingCart.GetSelectedFreedomOfChoices();

                if (freedomOfChoiceSummaryData != null)
                {
                    booknowStepsDetails.StepFreedomOfChoice.FreedomOfChoiceSelectNow = freedomOfChoiceSummaryData.SelectNow;
                    booknowStepsDetails.StepFreedomOfChoice.SelectedFreedomOfChoices =
                        freedomOfChoiceSummaryData.FreedomOfChoiceItems;
                } 
                booknowStepsDetails.StepOtherOptions.RequireFareAssistance = _bookingCart.CurrentCartRequiredFareAssistance;
                booknowStepsDetails.StepOtherOptions.RequireTravelInsuranceAssistance = _bookingCart.CurrentCartRequiredInsuranceAssistance;
                //booknowStepsDetails.StepEntireFlex.AvailFlexOptions = _bookingCart.CurrentCartAvailFlexOption;

                if (_bookingCart.TotalRooms > 1)
                {
                    booknowStepsDetails.StepExtras.DisableSelectNow = booknowStepsDetails.StepFreedomOfChoice.DisableSelectNow = true;
                    booknowStepsDetails.StepExtras.ExtrasSelectNow = false;
                    booknowStepsDetails.StepFreedomOfChoice.FreedomOfChoiceSelectNow = false;
                }

                
                //booknowStepsDetails.StepEntireFlex.DisableCheckbox = !ETGBookingHelper.EntireFlexIsOffered(departureDate, _settings.EntireFlexThresholdDays);

                /*if (cartSummary?.EntireFlexPricing != null &&  cartSummary.EntireFlexPricing.Count > 0 && cartSummary.EntireFlexPricing.TotalPrice > 0)
                {
                    booknowStepsDetails.StepEntireFlex.SubLabel = $"({cartSummary.EntireFlexPricing.TotalPrice.FormatPrice()})";
                }*/
                return booknowStepsDetails;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Booking", "GetAllBookingOptions", ex);
                return null;
            }
        }

        private bool ExtrasIsAvailable(string extraLabel, List<OptionalExtras> extrasInCmsTree)
        {
            var extras = extrasInCmsTree.FirstOrDefault(a => a.OptionalExtrasTitle == extraLabel);

            if (extras == null)
            {
                return false;
            }

            if (extras.OptionalExtrasDaysOfWeek.IsNullOrEmpty() &&
                (extras.OptionalExtrasFromDate == DateTime.MinValue ||
                 extras.OptionalExtrasToDate == DateTime.MinValue))
            {
                return true;
            }

            var departureDate = _bookingCart.CurrentDepartureDate;
            var endDate = departureDate.AddDays(_bookingCart.CurrentTourNumberOfNights);

            var extrasIsAvailable = true;
            for (var dt = departureDate; dt < endDate; dt= dt.AddDays(1))
            {
                
                if (extras.OptionalExtrasFromDate > DateTime.MinValue &&
                    extras.OptionalExtrasToDate > DateTime.MinValue)
                {
                    if (dt < extras.OptionalExtrasFromDate || dt > extras.OptionalExtrasToDate)
                    {
                        continue;
                    }    
                }
                
                if (!extras.OptionalExtrasDaysOfWeek.IsNullOrEmpty())
                {
                    if (!extras.OptionalExtrasDaysOfWeek.Contains(((int)dt.DayOfWeek).ToString()))
                    {
                        continue;
                    }
                }

                return true;
            }

            return false;
        }

        private bool IsRoomOptionSelected(int index, List<RoomOptionItemBreakdown> roomOptions, string optionLabel)
        {
            if (roomOptions.IsNullOrEmpty())
            {
                return false;
            }

            if (index > roomOptions.Count - 1)
            {
                var allSameRoomOptions = roomOptions.FirstOrDefault(r => r.ItemLabel == optionLabel);

                if (allSameRoomOptions == null)
                {
                    return false;
                }

                if (allSameRoomOptions.ItemTypeId != (int)RoomOptionTypeEnum.TwinShareRoomOption)
                {
                    return false;
                }

                if ((index + 1) * 2 >= allSameRoomOptions.Count)
                {
                    return true;
                }

                return false;
            }

            if (roomOptions[index].ItemLabel == optionLabel)
            {
                return true;
            }

            return false;

        }
    }
}
