using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Ecommerce;
using ETG.Module.Booking.ECommerce.Payment.Models;

namespace ETG.Module.Booking.ECommerce.Payment
{
    public interface ITravelPayService
    {
        TravelPayResult CreateCustomer(CustomerInfo customer);
        TravelPayResult MakePayment(CustomerInfo customer, string cardProxy, double amount);

        TravelPayResult CreateCardProxy(string customerUniqueId, string cardNumber, string cardExpiry,
            string cardHolderName);
    }
}
