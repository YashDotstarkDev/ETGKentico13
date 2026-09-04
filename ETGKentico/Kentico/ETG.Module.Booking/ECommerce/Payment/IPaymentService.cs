using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Ecommerce;
using ETG.Module.Booking.ECommerce.Payment.Models;

namespace ETG.Module.Booking.ECommerce.Payment
{
    public interface IPaymentService
    {
        PaymentResultInfo MakePayment(BookingCheckoutInfo bookingCheckoutInfo);
        string GetPaymentUrl(OrderInfo order, CustomerInfo customer);
    }
}
