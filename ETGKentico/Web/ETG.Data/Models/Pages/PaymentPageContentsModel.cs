using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Models.Booking;

namespace ETG.Data.Models.Pages
{
    public class PaymentPageContentsModel : IDataModel
    {
        public int PaymentPageDocumentID { get; set; }
        public string Heading { get; set; }
        public string PageBodyContent { get; set; }
        public string ThankyouPageBodyContent { get; set; }
        public string ThankyouPageHeroImage { get; set; }
        public List<PaymentOptionModel> PaymentOptions { get; set; }
    }
}
