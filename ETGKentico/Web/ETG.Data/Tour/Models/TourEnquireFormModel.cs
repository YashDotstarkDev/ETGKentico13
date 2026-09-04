using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class TourEnquireFormModel : IDataModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string PreferredContactType { get; set; }

        public string Message { get; set; }
        public bool SubscribeToNewsletter { get; set; }
        public string TourCode { get; set; }
        public string TourName { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureClass { get; set; }
        public string DepartureCity { get; set; }
        public int QuotedPrice { get; set; }
        public string PriceCurrency { get; set; }
    }
}