using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CMS;
using CMS.Ecommerce;
using CMS.EventLog;
using ETG.Data.Settings;
using ETG.Data.Tour;
using ETG.Module.Booking.Extensions;
using ETG.Module.Booking.Shopping;
using Newtonsoft.Json;

[assembly: RegisterCustomProvider(typeof(ETGBookingOrderInfoProvider))]

public class ETGBookingOrderInfoProvider : OrderInfoProvider
{
    protected override void SetOrderInfoInternal(OrderInfo order)
    {
        
        var _bookingCart = DependencyResolver.Current.GetService<IBookingCart>();
     
        bool newOrder = ((order != null) && (order.OrderID <= 0));
        if (newOrder)
        {
            var totalPrice = (decimal) _bookingCart.GetCartTotalPrice();
            order.OrderTotalPrice = totalPrice;
            order.OrderTotalPriceInMainCurrency = totalPrice;
            
            var agentPrice = _bookingCart.GetAgentPrice();
            if (agentPrice > 0)
            {
                var currencyPricing = new CurrentCurrencyPricing(_bookingCart.CurrentCurrency, _bookingCart.ConversionRate);
                order.OrderGrandTotal = currencyPricing.ConvertCurrentCurrencyToAUD(agentPrice);
                order.OrderGrandTotalInMainCurrency = currencyPricing.ConvertCurrentCurrencyToAUD(agentPrice);
            }
            else
            {
                order.OrderGrandTotal = totalPrice;
                order.OrderGrandTotalInMainCurrency = totalPrice;
            }
            
            order.OrderNote = _bookingCart.GetShoppingCartNote();
            order.SetValue("OrderTourCode", _bookingCart.CurrentTourCode);
            order.SetValue("OrderTourDepartureDate", _bookingCart.CurrentDepartureDate);
        }
        // Updates the order or creates a new order using the default API
        base.SetOrderInfoInternal(order);

    }
}

