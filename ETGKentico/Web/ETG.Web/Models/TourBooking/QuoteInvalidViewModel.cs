using CMS.Ecommerce;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Models.Cart;

namespace ETG.Web.Models.TourBooking
{
    public class QuoteInvalidViewModel
    {
        public string TourCode { get; set; }
        public string TourName { get; set; }
        public string HeroImage { get; set; }
        public string Message { get; set; }
        public BookingQuoteInfo Quote { get; set; }
        public CustomerInfo Customer{ get; set; }

    }
}