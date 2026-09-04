using System.Linq;
using System.Text;
using AngleSharp.Dom;
using Castle.Core.Internal;
using CMS.Ecommerce;
using CMS.Helpers;
using CommonServiceLocator;
using ETG.Booking.Pricing.Enums;
using ETG.Data.Extensions;
using ETG.Data.Tour;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.Models.Cart;
using Microsoft.Office.SharePoint.Tools;
using WebSupergoo.ABCpdf10;

namespace ETG.Module.Booking.Admin
{
    public class AdminSummaryBreakdownProvider 
    {
        private string GetBreakdownRow(ItemBreakdown breakdown, CurrentCurrencyPricing currentCurrencyPricing, bool priceInCurrentCurrency = false)
        {
            var itemLabel = breakdown.ItemLabel;

            if (!breakdown.ItemSubLabel.IsNullOrEmpty())
            {
                itemLabel += "<br>" + breakdown.ItemSubLabel;
            }

            var html = new StringBuilder();
            html.Append($"<tr><td>{itemLabel}</td><td style=\"text-align:right\">{breakdown.Count}</td>");

            
            html.Append($"<td style=\"text-align:right\">{GetDisplayPrice(breakdown.UnitPrice, currentCurrencyPricing, priceInCurrentCurrency)}</td>");
            html.Append(
                $"<td  style=\"text-align:right\">{GetDisplayPrice(breakdown.TotalPrice, currentCurrencyPricing, priceInCurrentCurrency)}</td>");

            html.Append("</tr>");
            return html.ToString();
        }
        
        
        
        
        private string GetPackagesBreakdownRowExcludingPrePostNights(ItemBreakdown breakdown, double totalAdjustmentPrice, CurrentCurrencyPricing currentCurrencyPricing)
        {
            var itemLabel = breakdown.ItemLabel;

            if (!breakdown.ItemSubLabel.IsNullOrEmpty())
            {
                itemLabel += "<br>" + breakdown.ItemSubLabel;
            }

            var html = new StringBuilder();
            html.Append($"<tr><td>{itemLabel}</td><td style=\"text-align:right\">{breakdown.Count}</td>");
            html.Append($"<td  style=\"text-align:right\">{GetDisplayPrice(breakdown.GetAdjustedUnitPrice(totalAdjustmentPrice), currentCurrencyPricing)}</td>");
            html.Append($"<td  style=\"text-align:right\">{GetDisplayPrice(breakdown.GetAdjustedTotalPrice(totalAdjustmentPrice), currentCurrencyPricing)}</td>");
            html.Append("</tr>");

            return html.ToString();
        }
        
        public string GetQuoteSummaryHtml(BookingQuoteInfo quote, CurrentCurrencyPricing quoteCurrencyPricing,
            PrePostNightsPriceContainer prePostNightsPrices)
        {
            if (quote == null)
            {
                return string.Empty;
            }
            
            var bookingDataProvider = ServiceLocator.Current.GetInstance<IBookingDataProvider>();
            
            var customData = new ContainerCustomData();
            customData.LoadData(quote.BookingQuoteCustomData);

            var summary = bookingDataProvider.GetSummaryData(customData);

            if (summary == null)
            {
                return string.Empty;
            }

            summary.ConvertPricesToCurrentCurrency(quoteCurrencyPricing, new CurrentCurrencyPricing("AUD", 1));

            
            var html = new StringBuilder();
            html.AppendLine("<table>");
            html.AppendLine("<tr><td></td><td style=\"padding-left:50px;text-align:right\">Quantity</td><td style=\"padding-left:50px;text-align:right\">Unit Price</td><td style=\"padding-left:50px;text-align:right\">Sub-total</td></tr>");
            if (summary.Packages != null && summary.Packages.Count > 0)
            {
                html.AppendLine(GetBreakdownRow(summary.Packages, quoteCurrencyPricing));
                
                if (summary.PreNightsDetails != null)
                {
                    html.AppendLine(GetPrePostNightsRow("- Pre Nights", summary.Packages.Count, summary.PreNightsDetails, prePostNightsPrices.TwinPrePrice, quoteCurrencyPricing));
                }
            
                if (summary.PostNightsDetails != null)
                {
                    html.AppendLine(GetPrePostNightsRow("- Post Nights", summary.Packages.Count, summary.PostNightsDetails, prePostNightsPrices.TwinPostPrice, quoteCurrencyPricing));
                }
            }

            if (summary.SinglePackages != null && summary.SinglePackages.Count > 0)
            {
                html.AppendLine(GetBreakdownRow(summary.SinglePackages, quoteCurrencyPricing));
                
                if (summary.PreNightsDetails != null)
                {
                    html.AppendLine(GetPrePostNightsRow("- Pre Nights", summary.SinglePackages.Count, summary.PreNightsDetails, prePostNightsPrices.SinglePrePrice, quoteCurrencyPricing));
                }
            
                if (summary.PostNightsDetails != null)
                {
                    html.AppendLine(GetPrePostNightsRow("- Post Nights",summary.SinglePackages.Count,  summary.PostNightsDetails, prePostNightsPrices.SinglePostPrice, quoteCurrencyPricing));
                }
            }

            if (!summary.RoomOptions.IsNullOrEmpty())
            {
                foreach (var roomOption in summary.RoomOptions)
                {
                    html.AppendLine(GetBreakdownRow(roomOption, quoteCurrencyPricing));
                    
                    if (summary.Packages != null && summary.Packages.Count > 0)
                    {
                        if (summary.PreNightsDetails != null)
                        {
                            html.AppendLine(GetPrePostNightsRow("- Pre Nights (Twin)", 2, summary.PreNightsDetails, roomOption.PreNightUnitPrice, quoteCurrencyPricing));
                        }
            
                        if (summary.PostNightsDetails != null)
                        {
                            html.AppendLine(GetPrePostNightsRow("- Post Nights (Twin)",2,  summary.PostNightsDetails, roomOption.PostNightUnitPrice, quoteCurrencyPricing));
                        }   
                    }
                    
                    /*
                    if (summary.SinglePackages != null && summary.SinglePackages.Count > 0)
                    {
                        if (summary.PreNightsDetails != null)
                        {
                            html.AppendLine(GetPrePostNightsRow("- Pre Nights (Twin)", 1, summary.PreNightsDetails, roomOption.PreNightUnitPrice, quoteCurrencyPricing));
                        }
            
                        if (summary.PostNightsDetails != null)
                        {
                            html.AppendLine(GetPrePostNightsRow("- Post Nights (Twin)",1,  summary.PostNightsDetails, roomOption.PostNightUnitPrice, quoteCurrencyPricing));
                        }   
                    }*/
                }
            }

            if (!summary.ExtraOptions.IsNullOrEmpty())
            {
                foreach (var roomOption in summary.ExtraOptions)
                {
                    html.AppendLine(GetBreakdownRow(roomOption, quoteCurrencyPricing));
                }
            }

            var quoteAddedServiceProvider = ServiceLocator.Current.GetInstance<IQuoteAddedServiceProvider>();
            
            var addedServices = quoteAddedServiceProvider.GetAddedServices(quote.BookingQuoteID);

            var addedServicesTotalPrice = 0.0;
            if (!addedServices.IsNullOrEmpty())
            {
                foreach (var service in addedServices)
                {
                    html.AppendLine(GetBreakdownRow(service, quoteCurrencyPricing, true));
                }
                
                addedServicesTotalPrice = addedServices.Select(a => a.TotalPrice).Sum();
            }
            if (summary.Promotion != null)
            {
                html.AppendLine($"<tr><td>{summary.Promotion.PromotionName}</td>");
                html.AppendLine($"<td colspan=\"3\" style=\"text-align:right\">-{GetDisplayPrice(summary.Discount, quoteCurrencyPricing)}</td>");
                
                html.AppendLine($"</tr>");
                html.AppendLine($"<tr><td>{summary.Promotion.BookByText}</td><td colspan=\"3\" style=\"text-align:right\">&nbsp;</td><tr>");

            }
            var totalPriceIsInAUD = true;
            var totalPrice = summary.TotalPrice;

            if (!quoteCurrencyPricing.CurrentCurrencyIsAUD)
            {
                if (addedServicesTotalPrice > 0)
                {
                    totalPriceIsInAUD = false;
                    totalPrice = quoteCurrencyPricing.ConvertAUDToCurrentCurrency(summary.TotalPriceMinusAddedServices) + addedServicesTotalPrice;   
                }
            }
            else
            {
                totalPrice += addedServicesTotalPrice;
            }

            html.AppendLine($"<tr><td>ENTIRE Gross Price</td><td colspan=\"3\" style=\"text-align:right\">{GetDisplayPrice(totalPrice, quoteCurrencyPricing, !totalPriceIsInAUD)}</td><tr>");
            if (summary.TotalNetPrice > 0)
            {
                html.AppendLine(
                    $"<tr><td>Agent Net Price</td><td colspan=\"3\" style=\"text-align:right\">{GetDisplayPrice(summary.GetTotalNetPrice(totalPrice), quoteCurrencyPricing, !totalPriceIsInAUD)}</td><tr>");
            }

            var agentSellPrice = summary.AgentDefinedPrice == 0
                ? totalPrice
                : summary.AgentDefinedPrice + addedServicesTotalPrice; // Agent define price and added service price are in current currency

            html.AppendLine($"<tr><td>Agent Sell Price</td><td colspan=\"3\" style=\"text-align:right\">{GetDisplayPrice(agentSellPrice , quoteCurrencyPricing, !quoteCurrencyPricing.CurrentCurrencyIsAUD)}</td><tr>");
            html.AppendLine($"<tr><td>Agent Commission</td><td colspan=\"3\" style=\"text-align:right\">{GetDisplayPrice(summary.AgentCommissionAmount, quoteCurrencyPricing, !quoteCurrencyPricing.CurrentCurrencyIsAUD)}</td><tr>");
            html.AppendLine("</table>");

            return html.ToString();
        }

        public string GetOrderSummaryHtml(OrderInfo order, CurrentCurrencyPricing orderCurrencyPricing,
            PrePostNightsPriceContainer prePostNightsPrices)
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
            var bookingDataProvider = ServiceLocator.Current.GetInstance<IBookingDataProvider>();

            var summary = bookingDataProvider.GetSummaryData(orderItems.FirstOrDefault()?.OrderItemCustomData);

            if (summary == null)
            {
                return string.Empty;
            }
            summary.ConvertPricesToCurrentCurrency(orderCurrencyPricing, new CurrentCurrencyPricing("AUD", 1));

            var html = new StringBuilder();
            html.AppendLine("<table>");
            html.AppendLine("<tr><td></td><td style=\"padding-left:50px;text-align:right\">Quantity</td><td style=\"padding-left:50px;text-align:right\">Unit Price</td><td style=\"padding-left:50px;text-align:right\">Sub-total</td></tr>");


            if (summary.Packages != null && summary.Packages.Count > 0)
            {
                html.AppendLine(GetPackagesBreakdownRowExcludingPrePostNights(summary.Packages, summary.GetAgentPriceDifference(summary.AgentDefinedPriceInAUD), orderCurrencyPricing));
                
                if (summary.PreNightsDetails != null)
                {
                    html.AppendLine(GetPrePostNightsRow("- Pre Nights", summary.Packages.Count, summary.PreNightsDetails, prePostNightsPrices.TwinPrePrice,   orderCurrencyPricing));
                }
            
                if (summary.PostNightsDetails != null)
                {
                    html.AppendLine(GetPrePostNightsRow("- Post Nights", summary.Packages.Count, summary.PostNightsDetails, prePostNightsPrices.TwinPostPrice,  orderCurrencyPricing));
                }
            }
            
            if (summary.SinglePackages != null && summary.SinglePackages.Count > 0)
            {
                if (summary.Packages != null && summary.Packages.Count > 0)
                {
                    html.AppendLine(GetBreakdownRow(summary.SinglePackages, orderCurrencyPricing));
                }
                else
                {
                    html.AppendLine(GetPackagesBreakdownRowExcludingPrePostNights(summary.SinglePackages, summary.GetAgentPriceDifference(summary.AgentDefinedPriceInAUD), orderCurrencyPricing));
                }
                
                if (summary.PreNightsDetails != null)
                {
                    html.AppendLine(GetPrePostNightsRow("- Pre Nights", summary.SinglePackages.Count, summary.PreNightsDetails, prePostNightsPrices.SinglePrePrice,   orderCurrencyPricing));
                }
            
                if (summary.PostNightsDetails != null)
                {
                    html.AppendLine(GetPrePostNightsRow("- Post Nights",summary.SinglePackages.Count,  summary.PostNightsDetails, prePostNightsPrices.SinglePostPrice,  orderCurrencyPricing));
                }
            }

            if (!summary.RoomOptions.IsNullOrEmpty())
            {
                foreach (var roomOption in summary.RoomOptions)
                {
                    if (roomOption.ItemLabel.IsNullOrEmpty())
                    {
                        continue;
                    }
                    html.AppendLine(GetBreakdownRow(roomOption, orderCurrencyPricing));

                    if (summary.Packages != null && summary.Packages.Count > 0)
                    {
                        if (summary.PreNightsDetails != null)
                        {
                            html.AppendLine(GetPrePostNightsRow("- Pre Nights (Twin)", 2, summary.PreNightsDetails, roomOption.PreNightUnitPrice, orderCurrencyPricing));
                        }
            
                        if (summary.PostNightsDetails != null)
                        {
                            html.AppendLine(GetPrePostNightsRow("- Post Nights (Twin)",2,  summary.PostNightsDetails, roomOption.PostNightUnitPrice,  orderCurrencyPricing));
                        }   
                    }
                    
                    /*
                    if (summary.SinglePackages != null && summary.SinglePackages.Count > 0)
                    {
                        if (summary.PreNightsDetails != null)
                        {
                            html.AppendLine(GetPrePostNightsRow("- Pre Nights (Twin)", 1, summary.PreNightsDetails, roomOption.PreNightUnitPrice, orderCurrencyPricing));
                        }
            
                        if (summary.PostNightsDetails != null)
                        {
                            html.AppendLine(GetPrePostNightsRow("- Post Nights (Twin)",1,  summary.PostNightsDetails, roomOption.PostNightUnitPrice,  orderCurrencyPricing));
                        }   
                    }*/
                }
            }

            if (!summary.ExtraOptions.IsNullOrEmpty())
            {
                foreach (var roomOption in summary.ExtraOptions)
                {
                    html.AppendLine(GetBreakdownRow(roomOption, orderCurrencyPricing));
                }
            }

            var addedServices = summary.AdditionalServices;
            var addedServicesTotalPrice = 0.0;
            if (!addedServices.IsNullOrEmpty())
            {
                foreach (var service in addedServices)
                {
                    html.AppendLine(GetBreakdownRow(service, orderCurrencyPricing, true));
                }
                
                addedServicesTotalPrice = addedServices.Select(a => a.TotalPrice).Sum();
            }


            if (summary.Promotion != null)
            {
                html.AppendLine($"<tr><td>{summary.Promotion.PromotionName}</td><td colspan=\"3\" style=\"text-align:right\">-{GetDisplayPrice(summary.Discount, orderCurrencyPricing)}</td><tr>");
                html.AppendLine($"<tr><td>{summary.Promotion.BookByText}</td><td colspan=\"3\" style=\"text-align:right\">&nbsp;</td><tr>");

            }

            var totalPriceIsInAUD = true;
            var totalPrice = summary.TotalPrice;
            
            if (!orderCurrencyPricing.CurrentCurrencyIsAUD)
            {
                if (addedServicesTotalPrice > 0)
                {
                    totalPriceIsInAUD = false;
                    totalPrice = orderCurrencyPricing.ConvertAUDToCurrentCurrency(summary.TotalPriceMinusAddedServices) + addedServicesTotalPrice;   
                }
            }
            
            var agentSellPrice = summary.AgentDefinedPrice == 0
                ? totalPrice
                : summary.AgentDefinedPrice; // Agent define price (already includes added service price) are in current currency

            if (summary.AgentDefinedPrice == 0)
            {
                html.AppendLine(
                    $"<tr><td>Total</td><td colspan=\"3\" style=\"text-align:right\">{GetDisplayPrice(totalPrice, orderCurrencyPricing, !totalPriceIsInAUD)}</td><tr>");
            }
            else
            {
                html.AppendLine($"<tr><td>ENTIRE Gross Price</td><td colspan=\"3\" style=\"text-align:right\">{GetDisplayPrice(totalPrice, orderCurrencyPricing,  !totalPriceIsInAUD)}</td><tr>");
                if (summary.TotalNetPrice > 0)
                {
                    html.AppendLine(
                        $"<tr><td>Agent Net Price</td><td colspan=\"3\" style=\"text-align:right\">{GetDisplayPrice(summary.GetTotalNetPrice(totalPrice), orderCurrencyPricing,  !totalPriceIsInAUD)}</td><tr>");
                }

                html.AppendLine($"<tr><td>Agent Sell Price</td><td colspan=\"3\" style=\"text-align:right\">{GetDisplayPrice(agentSellPrice, orderCurrencyPricing,  !orderCurrencyPricing.CurrentCurrencyIsAUD)}</td><tr>");

            }

            html.AppendLine("</table>");

            return html.ToString();
        }

        private string GetPrePostNightsRow(string itemLabel, int packageCount, PrePostNightsDetails prePostNightsDetails, double price, CurrentCurrencyPricing currentCurrencyPricing)
        {
            if (prePostNightsDetails == null || packageCount == 0 || price == 0)
            {
                return string.Empty;
            }
            var html = new StringBuilder();

            var totalPrice = price * prePostNightsDetails.NumberOfNights * packageCount;
            
            html.Append($"<tr><td>{itemLabel}</td><td style=\"text-align:right\">{prePostNightsDetails.NumberOfNights * packageCount}</td>");
            html.Append($"<td  style=\"text-align:right\">{GetDisplayPrice(price, currentCurrencyPricing)}</td>");
            html.Append($"<td  style=\"text-align:right\">{GetDisplayPrice(totalPrice, currentCurrencyPricing)}</td>");
            html.Append("</tr>");

            return html.ToString();
        }

        private string GetDisplayPrice(double price, CurrentCurrencyPricing currentCurrencyPricing, bool priceInCurrentCurrency = false)
        {
            if (currentCurrencyPricing.CurrentCurrencyIsAUD)
            {
                return price.FormatPrice(true);
            }

            if (!priceInCurrentCurrency)
            {
                return
                    $"<span class=\"price-display\">{currentCurrencyPricing.ConvertAUDToCurrentCurrency(price).FormatPrice(true, currentCurrencyPricing.CurrentCurrency)}</span> <span class=\"price-display\">({price.FormatPrice(true, CurrencyConstants.CODE_AUD)}</span>)";
            }
            else
            {
                return
                    $"<span class=\"price-display\">{price.FormatPrice(true, currentCurrencyPricing.CurrentCurrency)}</span> <span class=\"price-display\">({currentCurrencyPricing.ConvertCurrentCurrencyToAUD(price).FormatPrice(true, CurrencyConstants.CODE_AUD)}</span>)";
            }
        }
    }
}