using ETG.Module.Booking.Classes.Info;

namespace ETG.Web.Models.TourBooking
{
    public class QuoteSuccessViewModel
    {
        
        public string TourCode { get; set; }
        public string TourName { get; set; }
        public string HeroImage { get; set; }
        public string ThankYouCopy { get; set; }
        public BookingQuoteInfo Quote { get; set; }
        public string DownloadQuoteUrl { get; set; }
        public string BookNowUrl { get; set; }
        
    }
}