using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.UIControls;
using CommonServiceLocator;
using ETG.Data.Extensions;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.Extensions;
using ETG.Module.Booking.Models.Cart;

namespace ETG.Module.Booking.Extenders.Grid
{
    public class BookingQuoteGridExtender : ControlExtender<UniGrid>
    {
        public override void OnInit()
        {
            // Registers a method that handles the functionality of header actions
            Control.OnExternalDataBound += Control_OnExternalDataBound;
        }

        private CustomerInfo _currentCustomer;
        CartBookingSummary cartBookingSummary = null;
        private int _currentQuoteID = 0;
        private double packageQuotePrice = 0;
        private object Control_OnExternalDataBound(object sender, string sourceName, object parameter)
        {
            var quoteAddedServiceProvider = ServiceLocator.Current.GetInstance<IQuoteAddedServiceProvider>();
            List<ItemBreakdown> addedServices = null;
            switch (sourceName)
            {
                default:
                    return parameter;
                case "quoteid":
                    _currentQuoteID = parameter.ToInteger(0);
                    var quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(_currentQuoteID);
                    var customData = new ContainerCustomData();
                    customData.LoadData(quote.BookingQuoteCustomData);
                    
                    var  bookingDataProvider = ServiceLocator.Current.GetInstance<IBookingDataProvider>();
                    cartBookingSummary = bookingDataProvider.GetSummaryData(customData);
               
                    return parameter;
                case "customerid":
                    _currentCustomer = CustomerInfoProvider.GetCustomerInfo(parameter.ToInteger(0));

                    return parameter;
                case "quoteprice":

                    addedServices = quoteAddedServiceProvider.GetAddedServices(_currentQuoteID).ToList();
                    packageQuotePrice = cartBookingSummary.TotalPrice;
                    packageQuotePrice += addedServices.Select(a => a.TotalPrice).Sum();

                    return packageQuotePrice.ToString($"$#,##0.00");
                case "quotenetprice":
                    return cartBookingSummary.GetTotalNetPrice(packageQuotePrice).ToString($"$#,##0.00");
                case "quoteagentprice":
                    /*var price = parameter.ToDouble(0, CultureInfo.CurrentCulture.Name);
                    addedServices = quoteAddedServiceProvider.GetAddedServices(_currentQuoteID).ToList();
                    if (price == 0)
                    {
                        return string.Empty;
                    }
                    //price += addedServices.Select(a => a.TotalPrice).Sum();
                    return price.ToString($"$#,##0.00");
                    */

                    if (cartBookingSummary.CurrentCurrencyPricing == null ||
                        cartBookingSummary.CurrentCurrencyPricing.CurrentCurrencyIsAUD)
                    {
                        return cartBookingSummary.AgentDefinedPrice > 0
                            ? cartBookingSummary.AgentDefinedPrice.ToString($"$#,##0.00")
                            : "";   
                    }

                    if (cartBookingSummary.AgentDefinedPrice <= 0)
                    {
                        return string.Empty;
                    }

                    return cartBookingSummary.CurrentCurrencyPricing.ConvertCurrentCurrencyToAUD(cartBookingSummary
                        .AgentDefinedPrice).FormatPrice(false);
                
                case "agencyname":
                    if (_currentCustomer != null)
                    {
                        return _currentCustomer.GetStringValue("CustomerAgencyName", "");
                    }
                    return "";
                case "advisorname":
                    if (_currentCustomer != null)
                    {
                        return _currentCustomer.GetStringValue("CustomerAgentName", "");
                    }
                    return "";
                case "clientname":

                    if (_currentCustomer != null)
                    {
                        return $"{_currentCustomer.CustomerFirstName} {_currentCustomer.CustomerLastName}";
                    }

                    return "";
            }
        }
    }
}
