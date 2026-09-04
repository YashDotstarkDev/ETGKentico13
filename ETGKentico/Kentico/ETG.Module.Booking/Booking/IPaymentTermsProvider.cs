using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Module.Booking.Booking
{
    public interface IPaymentTermsProvider
    {
        string GetBookingPaymentTerms(CartBookingSummary bookingSummary, string tourPaymentTerms = "");
    }
}
