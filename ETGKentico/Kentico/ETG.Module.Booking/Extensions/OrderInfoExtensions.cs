using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Internal;
using Castle.Windsor.Diagnostics;
using CMS.Base;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CommonServiceLocator;
using ETG.Data.Extensions;
using ETG.Data.Tour;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Constants;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.Models.Cart;

namespace ETG.Module.Booking.Extensions
{
    public static class OrderInfoExtensions
    {
        private static string GetBreakdownRow(ItemBreakdown breakdown, CurrentCurrencyPricing currentCurrencyPricing)
        {
            var itemLabel = breakdown.ItemLabel;

            if (!breakdown.ItemSubLabel.IsNullOrEmpty())
            {
                itemLabel += "<br>" + breakdown.ItemSubLabel;
            }

            return
                  $"<tr><td>{itemLabel}</td><td style=\"text-align:right\">{breakdown.Count}</td><td  style=\"text-align:right\">{breakdown.UnitPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrencySymbol)}</td><td  style=\"text-align:right\">{breakdown.TotalPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrencySymbol)}</td></tr>";   
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

        public static string BookingPrePostNightsEmailHtml(this OrderInfo order)
        {
            if (order == null)
            {
                return string.Empty;
            }
            
            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems.IsNullOrEmpty())
            {
                return string.Empty;
            }
            
            var bookingDataProvider = DependencyResolver.Current.GetService<IBookingDataProvider>();
            
            var summary = bookingDataProvider.GetSummaryData(orderItems.FirstOrDefault()?.OrderItemCustomData);

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
        
        public static string BookingBreakdownEmailHtml(this OrderInfo order, CurrentCurrencyPricing currentCurrencyPricing)
        {
            if (order == null)
            {
                return string.Empty;
            }

            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems.IsNullOrEmpty())
            {
                return string.Empty;
            }
            var bookingDataProvider = DependencyResolver.Current.GetService<IBookingDataProvider>();
            
            var summary = bookingDataProvider.GetSummaryData(orderItems.FirstOrDefault()?.OrderItemCustomData);

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
                //No adjustment for single packages prices when twin packages is included
                //Adjustment already done in twin packages
                var agentPriceDifference = 0.0;

                if (summary.Packages == null || summary.Packages.Count == 0)
                {
                    agentPriceDifference = summary.AgentPriceDifference;
                }
                
                html.AppendLine(GetPackagesBreakdownRow(summary.GetSinglePackageAdjustedTotalPrice(agentPriceDifference),
                    summary.GetSinglePackageAdjustedUnitPrice(agentPriceDifference),
                    summary.SinglePackages, currentCurrencyPricing));
                
            }

            if (!summary.RoomOptions.IsNullOrEmpty())
            {
                foreach (var roomOption in summary.RoomOptions)
                {
                    if (roomOption.ItemLabel.IsNullOrEmpty())
                    {
                        continue;
                    }
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

            if (!summary.AdditionalServices.IsNullOrEmpty())
            {
                foreach (var service in summary.AdditionalServices)
                {
                    html.AppendLine(GetBreakdownRow(service, currentCurrencyPricing));
                }
            }

            if (summary.Promotion != null)
            {
                html.AppendLine($"<tr><td>{summary.Promotion.PromotionName}</td><td colspan=\"3\" style=\"text-align:right\">-{summary.Discount.FormatPrice(true, currentCurrencyPricing.CurrentCurrencySymbol)}</td><tr>");
                html.AppendLine($"<tr><td>{summary.Promotion.BookByText}</td><td colspan=\"3\" style=\"text-align:right\">&nbsp;</td><tr>");

            }

            var agentSellPrice = summary.AgentSellPrice;
            var totalPrice = summary.TotalPrice;
            var totalNetPrice = summary.TotalNetPrice;

            var formattedAgentSellPrice = agentSellPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrency);
            var formattedTotalPrice = totalPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrency);
            var formattedTotalNetPrice = totalNetPrice.FormatPrice(true, currentCurrencyPricing.CurrentCurrency);
            
            if (summary.AgentDefinedPrice == 0)
            {
                html.AppendLine(
                    $"<tr><td>Total</td><td colspan=\"3\" style=\"text-align:right\">{formattedAgentSellPrice}</td><tr>");
            }
            else
            {
                html.AppendLine($"<tr><td>ENTIRE Gross Price</td><td colspan=\"3\" style=\"text-align:right\">{formattedTotalPrice}</td><tr>");
                if (summary.TotalNetPrice > 0)
                {
                    html.AppendLine(
                        $"<tr><td>Agent Net Price</td><td colspan=\"3\" style=\"text-align:right\">{formattedTotalNetPrice}</td><tr>");
                }

                html.AppendLine($"<tr><td>Agent Sell Price</td><td colspan=\"3\" style=\"text-align:right\">{formattedAgentSellPrice}</td><tr>");

            }

            html.AppendLine("</table>");

            return html.ToString();
        }

        /*
        public static string BookingAddedServicesBreakdownEmailHtml(this OrderInfo order, bool isAdmin = false)
        {
            if (order == null)
            {
                return string.Empty;
            }


            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems.IsNullOrEmpty())
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

            var summary = bookingDataProvider.GetAddedServicesData(orderItems.FirstOrDefault()?.OrderItemCustomData);

            if (summary == null || summary.Services.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var html = new StringBuilder();
            html.AppendLine("<table>");
            html.AppendLine("<tr><td></td><td style=\"padding-left:50px;text-align:right\">Quantity</td><td style=\"padding-left:50px;text-align:right\">Unit Price</td><td style=\"padding-left:50px;text-align:right\">Sub-total</td></tr>");
            if (!summary.Services.IsNullOrEmpty())
            {
                foreach (var service in summary.Services)
                {
                    html.AppendLine(GetBreakdownRow(service));
                }
            }

            html.AppendLine($"<tr><td>Total</td><td colspan=\"3\" style=\"text-align:right\">{summary.TotalPrice.FormatPrice()}</td><tr>");
            html.AppendLine("</table>");

            return html.ToString();
        }*/

        public static string BookingTourInfoEmailHtml(this OrderInfo order, bool isAdmin = false)
        {
            if (order == null)
            {
                return string.Empty;
            }


            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems == null || orderItems.IsNullOrEmpty())
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
            var summary = bookingDataProvider.GetSummaryData(orderItems.FirstOrDefault()?.OrderItemCustomData);

            if (summary == null)
            {
                return string.Empty;
            }

            var html = new StringBuilder();

            html.AppendLine("<table>");
            html.AppendLine($"<tr><td>Tour code</td><td>{summary.TourCode}</td></tr>");
            html.AppendLine($"<tr><td>Tour name</td><td>{summary.TourName}</td></tr>");
            html.AppendLine($"<tr><td>Tour date</td><td>{summary.DepartureDateDisplay}</td></tr>");
            html.AppendLine("</table>");
            return html.ToString();
        }

        public static string BookingCustomerInfoEmailHtml(this OrderInfo order)
        {
            if (order == null)
            {
                return string.Empty;
            }

            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);

            var html = new StringBuilder();
            
            html.AppendLine($"First name: {customer.CustomerFirstName}<br>");
            html.AppendLine($"Last name: {customer.CustomerLastName}<br>");
            html.AppendLine($"Email: {customer.CustomerEmail}<br>");
            html.AppendLine($"Phone: {customer.CustomerPhone}<br>");
            html.AppendLine($"State: {customer.GetStringValue("CustomerState", string.Empty)}<br>");

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
            return html.ToString();
        }

        public static string BookingTourInfoAdminHtml(this OrderInfo order)
        {
            return BookingTourInfoEmailHtml(order, true);
        }

        public static string BookingCustomerInfoAdminHtml(this OrderInfo order)
        {
            return BookingCustomerInfoEmailHtml(order);
        }

        public static ContainerCustomData GetOrderItemCustomData(this OrderInfo order)
        {
            if (order == null)
            {
                return null;
            }

            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems.IsNullOrEmpty())
            {
                return null;
            }
            
            return orderItems.FirstOrDefault()?.OrderItemCustomData;

        }

        public static DetailedPackageBooking GetDetails(this OrderInfo order)
        {
            if (order == null)
            {
                return null;
            }

            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems.IsNullOrEmpty())
            {
                return null;
            }

            var customData = orderItems.FirstOrDefault()?.OrderItemCustomData;

            if (customData == null)
            {
                return null;
            }

            var booking = new DetailedPackageBooking
            {
                Order = order,
                TourCode = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURCODE),
                TourName = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURNAME),
                TourDestination = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURDESTINATION),
                TourUrl = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TOURURL),
                IsOnSaleNow = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_ONSALENOW),
                OnSaleFullPaymentRequired = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_ONSALE_FULLPAYMENTREQUIRED),
                //IsFOCEntireFlex = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_FOCENTIREFLEX),
                SingleRoomCount = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_SINGLEROOMCOUNT).ToInteger(0),
                TwinShareRoomCount = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT).ToInteger(0),
                TwinShareRoomsType = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHAREROOMSTYPE),
                RoomOptions = customData.GetObject<RoomOptionsSummaryCartData>(BookingConstants.CUSTOM_COLUMN_ROOMOPTIONSUMMARY),
                Extras = customData.GetObject<ExtrasSummaryCartData>(BookingConstants.CUSTOM_COLUMN_EXTRASSUMMARY),
                FreedomOfChoices = customData.GetObject<FreedomOfChoicesSummaryCartData>(BookingConstants.CUSTOM_COLUMN_FREEDOMOFCHOICESSUMMARY),
                //EntireFlex = customData.GetObject<EntireFlexSummaryCartData>(BookingConstants.CUSTOM_COLUMN_ENTIREFLEXSUMMARY),
                InternationalAirfareAssistance = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_REQUIREFAREASSISTANCE),
                TravelInsuranceAssistance = customData.GetBooleanCartItemValue(BookingConstants.CUSTOM_COLUMN_REQUIREINSURANCEASSISTANCE),
                DepartureDate = customData.GetDateCartItemValue(BookingConstants.CUSTOM_COLUMN_DEPARTUREDATE),
                SecondInstalmentDate = customData.GetDateCartItemValue(BookingConstants.CUSTOM_COLUMN_INSTALMENTDATE),
                SecondInstalmentPercentage =  customData.GetDoubleCartItemValue(BookingConstants.CUSTOM_COLUMN_SECONDINSTALMENTPERCENTAGE),
                PaymentBalanceDueDate = customData.GetDateCartItemValue(BookingConstants.CUSTOM_COLUMN_BALANCEDUEDATE),
                SourceQuoteID = customData.GetIntCartItemValue(BookingConstants.CUSTOM_COLUMN_QUOTEID),
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
            
            return booking;
        }

        public static string GetTwinRoomsType(this OrderInfo order)
        {
            if (order == null)
            {
                return string.Empty;
            }

            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems.IsNullOrEmpty())
            {
                return null;
            }

            var customData = orderItems.FirstOrDefault()?.OrderItemCustomData;

            if (customData == null)
            {
                return null;
            }

            var twinShareCount = customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHARECOUNT)
                .ToInteger(0);

            if (twinShareCount <= 0)
            {
                return string.Empty;
            }

            return customData.GetStringCartItemValue(BookingConstants.CUSTOM_COLUMN_TWINSHAREROOMSTYPE);
        }

        public static string GetTwinRoomsTypeHtml(this OrderInfo order)
        {
            if (order == null)
            {
                return string.Empty;
            }

            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems.IsNullOrEmpty())
            {
                return null;
            }

            var customData = orderItems.FirstOrDefault()?.OrderItemCustomData;

            if (customData == null)
            {
                return null;
            }

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
            for (int i= 1; i <= arr.Length; i++)
            {
                html.Append($"Room {i}: {arr[i - 1]}<br>");
            }

            return html.ToString();
        }

        public static CartBookingSummary GetOrderSummary(this OrderInfo order, bool isAdmin = false)
        {
            if (order == null)
            {
                return null;
            }

            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            if (orderItems.IsNullOrEmpty())
            {
                return null;
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

            return bookingDataProvider.GetSummaryData(orderItems.FirstOrDefault()?.OrderItemCustomData);
            
        }

        public static void AddUtmProperties(
            this OrderInfo order, 
            string utmCampaign, 
            string utmContent, 
            string utmMedium,
            string utmSource, 
            string utmTerm)
        {
            if (!string.IsNullOrWhiteSpace(utmCampaign))
            {
                order.SetValue("OrderUtmCampaign", utmCampaign);
            }
            
            if (!string.IsNullOrWhiteSpace(utmContent))
            {
                order.SetValue("OrderUtmContent", utmContent);
            }
            
            if (!string.IsNullOrWhiteSpace(utmMedium))
            {
                order.SetValue("OrderUtmMedium", utmMedium);
            }
            
            if (!string.IsNullOrWhiteSpace(utmSource))
            {
                order.SetValue("OrderUtmSource", utmSource);
            }
            
            if (!string.IsNullOrWhiteSpace(utmTerm))
            {
                order.SetValue("OrderUtmTerm", utmTerm);
            }
        }
    }
}