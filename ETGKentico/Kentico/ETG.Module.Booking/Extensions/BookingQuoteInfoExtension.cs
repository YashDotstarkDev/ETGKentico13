using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Internal;
using CMS.Base;
using CMS.Ecommerce;
using CMS.Helpers;
using CommonServiceLocator;
using ETG.Data.Extensions;
using ETG.Data.Tour;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.Constants;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.Models.Cart;

namespace ETG.Module.Booking.Extensions
{
    public static class BookingQuoteInfoExtension
    {
        public static bool HasExpired(this BookingQuoteInfo quoteInfo)
        {
            if (DateTime.Today > quoteInfo.BookingQuoteCreated.Date.AddDays(quoteInfo.BookingQuoteValidDays))
            {
                return true;
            }

            return false;
        }
        public static ContainerCustomData GetQuoteCustomData(this BookingQuoteInfo quoteInfo)
        {
            var customData = new ContainerCustomData();
            customData.LoadData(quoteInfo.BookingQuoteCustomData);

            return customData;
        }
        public static DetailedPackageBooking GetDetails(this BookingQuoteInfo quote)
        {
            if (quote == null)
            {
                return null;
            }
            

            var customData = GetQuoteCustomData(quote);

            if (customData == null)
            {
                return null;
            }

            var bookingQuote = new DetailedPackageBooking
            {
                Quote = quote,
                TourCode = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURCODE),
                TourName = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURNAME),
                TourDestination = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURDESTINATION),
                TourUrl = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURURL),
                IsOnSaleNow = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_ONSALENOW),
                OnSaleFullPaymentRequired = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_ONSALE_FULLPAYMENTREQUIRED),
                //IsFOCEntireFlex = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_FOCENTIREFLEX),
                SingleRoomCount = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEROOMCOUNT).ToInteger(0),
                TwinShareRoomCount = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT).ToInteger(0),
                RoomOptions = customData.GetObject<RoomOptionsSummaryCartData>(BookingConstants.CUSTOM_COLUMN_ROOMOPTIONSUMMARY),
                Extras = customData.GetObject<ExtrasSummaryCartData>(BookingConstants.CUSTOM_COLUMN_EXTRASSUMMARY),
                FreedomOfChoices = customData.GetObject<FreedomOfChoicesSummaryCartData>(BookingConstants.CUSTOM_COLUMN_FREEDOMOFCHOICESSUMMARY),
                //EntireFlex = customData.GetObject<EntireFlexSummaryCartData>(BookingConstants.CUSTOM_COLUMN_ENTIREFLEXSUMMARY),
                InternationalAirfareAssistance = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_REQUIREFAREASSISTANCE),
                TravelInsuranceAssistance = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_REQUIREINSURANCEASSISTANCE),
                DepartureDate = customData.GetDateCartItemValue(BookingConstants.CUSTOM_COLUMN_DEPARTUREDATE),
                TourHasPeaceOfMind = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_PEACEOFMIND),
                Currency = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_CURRENCY),
                ConversionRate = (decimal) customData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_CONVERSION_RATE),
                PreNightsDetails = customData.GetObject<PrePostNightsDetails>(BookingConstants.CUSTOM_COLUMN_PRENIGHTS),
                PostNightsDetails = customData.GetObject<PrePostNightsDetails>(BookingConstants.CUSTOM_COLUMN_POSTNIGHTS),
                TwinPreNightBasePrice = customData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINPRENIGHTS_BASEPRICE),
                TwinPostNightBasePrice = customData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINPOSTNIGHTS_BASEPRICE),
                SinglePreNightBasePrice = customData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEPRENIGHTS_BASEPRICE),
                SinglePostNightBasePrice = customData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEPOSTNIGHTS_BASEPRICE),
                PromoCode = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_PROMOCODE),
            };

            return bookingQuote;
        }

        public static string QuotePrePostNightsEmailHtml(this BookingQuoteInfo quote, bool isAdmin = false)
        {
            if (quote == null)
            {
                return string.Empty;
            }
            
            var customData = new ContainerCustomData();
            customData.LoadData(quote.BookingQuoteCustomData);
            
            
            IBookingDataProvider bookingDataProvider;
            if (!isAdmin)
            {
                bookingDataProvider = DependencyResolver.Current.GetService<IBookingDataProvider>();
            }
            else
            {
                bookingDataProvider = ServiceLocator.Current.GetInstance<IBookingDataProvider>();
            }
                
            
            var summary = bookingDataProvider.GetSummaryData(customData);

            if (summary == null)
            {
                return string.Empty;
            }

            if (summary.PreNightsDetails == null && summary.PostNightsDetails == null)
            {
                return string.Empty;
            }

            var html = new StringBuilder();
            if (summary.PreNightsDetails != null)
            {
                html.Append($"Pre Nights: {summary.PreNightsDetails.DateRangeDisplay}<br>");
            }
            
            if (summary.PostNightsDetails != null)
            {
                html.Append($"Post Nights: {summary.PostNightsDetails.DateRangeDisplay}<br>");
            }

            return html.ToString();
        }

        private static string GetPackagesBreakdownRow(double totalPrice, double unitPrice, ItemBreakdown breakdown, CurrentCurrencyPricing currentCurrencyPricing)
        {
            var itemLabel = breakdown.ItemLabel;

            if (!breakdown.ItemSubLabel.IsNullOrEmpty())
            {
                itemLabel += "<br>" + breakdown.ItemSubLabel;
            }

            return
                $"<tr><td>{itemLabel}</td><td style=\"text-align:right\">{breakdown.Count}</td><td  style=\"text-align:right\">{unitPrice.FormatPrice(true,currentCurrencyPricing.CurrentCurrencySymbol)}</td><td  style=\"text-align:right\">{totalPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrencySymbol)}</td></tr>";
        }
        
        public static string QuoteBreakdownEmailHtml(this BookingQuoteInfo quote, CurrentCurrencyPricing currentCurrencyPricing, bool isAdmin = false)
        {
            if (quote == null)
            {
                return string.Empty;
            }
            
            IBookingDataProvider bookingDataProvider;
            if (!isAdmin)
            {
                bookingDataProvider = DependencyResolver.Current.GetService<IBookingDataProvider>();
            }
            else
            {
                bookingDataProvider = ServiceLocator.Current.GetInstance<IBookingDataProvider>();
            }
            
            var customData = new ContainerCustomData();
            customData.LoadData(quote.BookingQuoteCustomData);

            var summary = bookingDataProvider.GetSummaryData(customData);

            if (summary == null)
            {
                return string.Empty;
            }

            summary.ConvertPricesToCurrentCurrency(currentCurrencyPricing);

            var html = new StringBuilder();
            html.AppendLine("<table>");
            html.AppendLine("<tr><td></td><td style=\"padding-left:50px;text-align:right\">Quantity</td><td style=\"padding-left:50px;text-align:right\">Unit Price</td><td style=\"padding-left:50px;text-align:right\">Sub-total</td></tr>");
            if (summary.Packages != null && summary.Packages.Count > 0)
            {
                html.AppendLine(GetPackagesBreakdownRow(summary.GetPackageAdjustedTotalPrice(summary.AgentPriceDifference),
                    summary.GetPackageAdjustedUnitPrice(summary.AgentPriceDifference), summary.Packages, currentCurrencyPricing));
            }

            if (summary.SinglePackages != null && summary.SinglePackages.Count > 0)
            {
                var agentPriceDifference = 0.0;

                if (summary.Packages == null || summary.Packages.Count == 0)
                {
                    agentPriceDifference = summary.AgentPriceDifference;
                }

                html.AppendLine(GetPackagesBreakdownRow(summary.GetSinglePackageAdjustedTotalPrice(agentPriceDifference),
                    summary.GetSinglePackageAdjustedUnitPrice(agentPriceDifference), summary.Packages, currentCurrencyPricing));
            }

            if (!summary.RoomOptions.IsNullOrEmpty())
            {
                foreach (var roomOption in summary.RoomOptions)
                {
                    html.AppendLine(GetRoomOptionBreakdownRow(roomOption, currentCurrencyPricing));
                }
            }

            if (!summary.ExtraOptions.IsNullOrEmpty())
            {
                foreach (var roomOption in summary.ExtraOptions)
                {
                    html.AppendLine(GetBreakdownRow(roomOption, currentCurrencyPricing));
                }
            }

            IQuoteAddedServiceProvider quoteAddedServiceProvider;
            if (!isAdmin)
            {
                quoteAddedServiceProvider = DependencyResolver.Current.GetService<IQuoteAddedServiceProvider>();
            }
            else
            {
                quoteAddedServiceProvider = ServiceLocator.Current.GetInstance<IQuoteAddedServiceProvider>();
            }
            
            var addedServices = quoteAddedServiceProvider.GetAddedServices(quote.BookingQuoteID);

            var addedServicesTotalPrice = 0.0;
            if (!addedServices.IsNullOrEmpty())
            {
                foreach (var service in addedServices)
                {
                    html.AppendLine(GetBreakdownRow(service, currentCurrencyPricing));
                }
                
                addedServicesTotalPrice = addedServices.Select(a => a.TotalPrice).Sum();
            }
            if (summary.Promotion != null)
            {
                html.AppendLine($"<tr><td>{summary.Promotion.PromotionName}</td><td colspan=\"3\" style=\"text-align:right\">-{summary.Discount.FormatPrice(true)}</td><tr>");
                html.AppendLine($"<tr><td>{summary.Promotion.BookByText}</td><td colspan=\"3\" style=\"text-align:right\">&nbsp;</td><tr>");

            }

            var totalPrice = summary.TotalPrice + addedServicesTotalPrice;
            html.AppendLine($"<tr><td>ENTIRE Gross Price</td><td colspan=\"3\" style=\"text-align:right\">{totalPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrency)}</td><tr>");
            if (summary.TotalNetPrice > 0)
            {
                html.AppendLine(
                    $"<tr><td>Agent Net Price</td><td colspan=\"3\" style=\"text-align:right\">{summary.GetTotalNetPrice(totalPrice).FormatPrice(true, currentCurrencyPricing.CurrentCurrency)}</td><tr>");
            }

            html.AppendLine($"<tr><td>Agent Sell Price</td><td colspan=\"3\" style=\"text-align:right\">{( summary.AgentSellPrice + addedServicesTotalPrice).FormatPrice(true, currentCurrencyPricing.CurrentCurrency)}</td><tr>");

            html.AppendLine("</table>");

            return html.ToString();
        }
        
        /*
        public static string QuoteAddedServiceBreakdownEmailHtml(this BookingQuoteInfo quote, bool isAdmin = false)
        {
            if (quote == null)
            {
                return string.Empty;
            }
            IQuoteAddedServiceProvider quoteAddedServiceProvider;
            if (!isAdmin)
            {
                quoteAddedServiceProvider = DependencyResolver.Current.GetService<IQuoteAddedServiceProvider>();
            }
            else
            {
                quoteAddedServiceProvider = ServiceLocator.Current.GetInstance<IQuoteAddedServiceProvider>();
            }
            var addedServices = quoteAddedServiceProvider.GetAddedServices(quote.BookingQuoteID);

            if (addedServices.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var html = new StringBuilder();
            html.AppendLine("<table>");
            html.AppendLine("<tr><td></td><td style=\"padding-left:50px;text-align:right\">Quantity</td><td style=\"padding-left:50px;text-align:right\">Unit Price</td><td style=\"padding-left:50px;text-align:right\">Sub-total</td></tr>");

            foreach (var service in addedServices)
            {
                html.AppendLine(GetBreakdownRow(service));
            }

            var totalPrice = addedServices.Select(a => a.TotalPrice).Sum();
            
            html.AppendLine($"<tr><td>Total</td><td colspan=\"3\" style=\"text-align:right\">{totalPrice.FormatPrice()}</td><tr>");
            html.AppendLine("</table>");

            return html.ToString();
        }*/

        public static double QuoteGrandTotal(this BookingQuoteInfo quote, bool isAdmin = false)
        {
            IBookingDataProvider bookingDataProvider;
            if (!isAdmin)
            {
                bookingDataProvider = DependencyResolver.Current.GetService<IBookingDataProvider>();
            }
            else
            {
                bookingDataProvider = ServiceLocator.Current.GetInstance<IBookingDataProvider>();
            }
            var customData = new ContainerCustomData();
            customData.LoadData(quote.BookingQuoteCustomData);

            if (customData == null)
            {
                return 0;
            }
            var summary = bookingDataProvider.GetSummaryData(customData);

            var grandTotal = summary.TotalPrice;

            IQuoteAddedServiceProvider quoteAddedServiceProvider;
            if (!isAdmin)
            {
                quoteAddedServiceProvider = DependencyResolver.Current.GetService<IQuoteAddedServiceProvider>();
            }
            else
            {
                quoteAddedServiceProvider = ServiceLocator.Current.GetInstance<IQuoteAddedServiceProvider>();
            }
            var addedServices = quoteAddedServiceProvider.GetAddedServices(quote.BookingQuoteID);

            if (addedServices.IsNullOrEmpty())
            {
                return grandTotal;
            }

            grandTotal += addedServices.Select(a => a.TotalPrice).Sum();

            return grandTotal;

        }

        public static string QuoteCustomerInfoEmailHtml(this BookingQuoteInfo quote)
        {
            if (quote == null)
            {
                return string.Empty;
            }

            var customer = CustomerInfoProvider.GetCustomerInfo(quote.BookingQuoteCustomerID);

            var html = new StringBuilder();

            var agencyName = customer.GetStringValue("CustomerAgencyName", string.Empty);
            if (!agencyName.IsNullOrEmpty())
            {
                html.AppendLine($"Travel Agency - Trading Name: {agencyName}<br>");
                html.AppendLine($"Travel Agency - Postcode: {customer.GetStringValue("CustomerAgencyPostcode", string.Empty)}<br>");
                html.AppendLine($"Travel Advisor - Full Name: {customer.GetStringValue("CustomerAgentName", string.Empty)}<br>");
                html.AppendLine($"Travel Advisor - Email: {customer.GetStringValue("CustomerAgentEmail", string.Empty)}<br>");
                html.AppendLine($"Travel Agency - Phone Number: {customer.GetStringValue("CustomerAgentPhone", string.Empty)}<br>");
                html.AppendLine($"Agency comments:<br>{customer.GetStringValue("CustomerAgentComment", string.Empty)}<br>");
            }

            html.AppendLine($"Lead Passenger First name: {customer.CustomerFirstName}<br>");
            html.AppendLine($"Lead Passenger Last name: {customer.CustomerLastName}<br>");
            html.AppendLine($"Lead Passenger Email: {customer.CustomerEmail}<br>");
            html.AppendLine($"Lead Passenger Phone: {customer.CustomerPhone}<br>");
            html.AppendLine($"Lead Passenger State: {customer.GetStringValue("CustomerState", string.Empty)}<br>");
            return html.ToString();
        }
        private static string GetBreakdownRow(ItemBreakdown breakdown, CurrentCurrencyPricing currentCurrencyPricing)
        {
            var itemLabel = breakdown.ItemLabel;

            if (!breakdown.ItemSubLabel.IsNullOrEmpty())
            {
                itemLabel += "<br>" + breakdown.ItemSubLabel;
            }


            return $"<tr><td>{itemLabel}</td><td style=\"text-align:right\">{breakdown.Count}</td><td  style=\"text-align:right\">{breakdown.UnitPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrencySymbol)}</td><td  style=\"text-align:right\">{breakdown.TotalPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrencySymbol)}</td></tr>";
        }
        
        private static string GetRoomOptionBreakdownRow(RoomOptionItemBreakdown breakdown, CurrentCurrencyPricing currentCurrencyPricing)
        {
            var itemLabel = breakdown.ItemLabel;

            if (!breakdown.ItemSubLabel.IsNullOrEmpty())
            {
                itemLabel += "<br>" + breakdown.ItemSubLabel;
            }

            return
                $"<tr><td>{itemLabel}</td><td style=\"text-align:right\">{breakdown.Count}</td><td  style=\"text-align:right\">{breakdown.UnitPriceWithPrePostNights.FormatPrice(true, currentCurrencyPricing.CurrentCurrencySymbol)}</td><td  style=\"text-align:right\">{breakdown.TotalPriceWithPrePostNights.FormatPrice(true, currentCurrencyPricing.CurrentCurrencySymbol)}</td></tr>";   
        }


        public static string GetTwinRoomsTypeHtml(this BookingQuoteInfo quote)
        {
            if (quote == null || quote.BookingQuoteCustomData.IsNullOrEmpty())
            {
                return null;
            }

            var customData = new ContainerCustomData();
            customData.LoadData(quote.BookingQuoteCustomData);

            var twinShareCount = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT)
                .ToInteger(0);

            if (twinShareCount <= 0)
            {
                return string.Empty;
            }

            var roomTypes = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHAREROOMSTYPE);

            if (roomTypes.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var html = new StringBuilder();
            var arr = roomTypes.Split(',');
            for (int i = 1; i <= arr.Length; i++)
            {
                html.Append($"Room {i}: {arr[i - 1]}<br>");
            }

            return html.ToString();
        }
        
        public static string GetTwinRoomsType(this BookingQuoteInfo quote)
        {
            if (quote == null || quote.BookingQuoteCustomData.IsNullOrEmpty())
            {
                return null;
            }

            var customData = new ContainerCustomData();
            customData.LoadData(quote.BookingQuoteCustomData);
            var twinShareCount = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT)
                .ToInteger(0);

            if (twinShareCount <= 0)
            {
                return string.Empty;
            }

            return customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHAREROOMSTYPE);
        }

    }
}
