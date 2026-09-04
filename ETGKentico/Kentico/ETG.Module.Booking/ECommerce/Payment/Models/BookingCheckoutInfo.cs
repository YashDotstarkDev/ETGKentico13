using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Ecommerce;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class BookingCheckoutInfo
    {
        public CustomerInfo Customer { get; set; }

        public string CreditCardNumber { get; set; }
        public string CreditCardExpiry { get; set; }
        public double PaymentAmount { get; set; }
    }
}
