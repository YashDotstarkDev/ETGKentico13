using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CMS;
using CMS.Ecommerce;
using ETG.Data.Settings;
using ETG.Module.Booking.Shopping;

[assembly: RegisterCustomProvider(typeof(ETGBookingOrderInfoItemProvider))]

public class ETGBookingOrderInfoItemProvider : OrderItemInfoProvider
{
    protected override void SetOrderItemInfoInternal(OrderItemInfo orderItem)
    {

        var _bookingCart = DependencyResolver.Current.GetService<IBookingCart>();
        
        bool newOrder = (orderItem != null) && (orderItem.OrderItemID <= 0);
        if (newOrder)
        {
            var totalPrice = (decimal) _bookingCart.GetCartTotalPrice();
            orderItem.OrderItemTotalPrice = totalPrice;
            orderItem.OrderItemTotalPriceInMainCurrency = totalPrice;
            orderItem.OrderItemUnitPrice = totalPrice;
            orderItem.OrderItemUnitCount = 1;
        }
        // Updates the order or creates a new order using the default API
        base.SetOrderItemInfoInternal(orderItem);

    }
}

