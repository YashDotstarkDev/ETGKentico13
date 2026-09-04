using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.Services
{
    public interface IQuoteService
    {
        void SendQuoteConfirmationEmail(BookingQuoteInfo bookingQuote, string additionalComments = "", string toEmail = "");
        void SendQuoteNotificationEmail(BookingQuoteInfo bookingQuote, string additionalComments = "", bool isAdmin = false);
    }
}
