using ETG.Data.Models.Booking;
using System.Collections.Generic;
using ETG.Data.Settings.Models;

namespace ETG.Web.Models.Pages
{
    public class PaymentPageContentsViewModel : IViewModel
    {
        public int PaymentPageDocumentID { get; set; }
        public ETGSettings ETGSettings { get; set; }
        public string Heading { get; set; }
        public string PageBodyContent { get; set; }
        public string ThankyouPageBodyContent { get; set; }
        public string ThankyouPageHeroImage { get; set; }
        public List<PaymentOptionModel> PaymentOptions { get; set; }
    }
}
