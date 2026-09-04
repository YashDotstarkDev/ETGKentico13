using Devotion.Automapper.Common;

namespace ETG.Data.Models.Pages
{
    public class BookingCheckoutPageContentsModel : IDataModel
    {
        public string ThankYouPageCopy { get; set; }
        public string OtherPaymentThankYouPageCopy { get; set; }
        public string CopyBelowCheckoutForm { get; set; }
        public string PaymentTermsCopyOnSaleNow { get; set; }
        public string PaymentTermsCopyWithoutPeaceOfMind { get; set; }
        public string PaymentTermsCopyWithPeaceOfMind { get; set; }
        public string PaymentTermsCopyWithPeaceOfMindAndFlex { get; set; }
        public string PaymentTermsFullPayment { get; set; }
        public string DueCopyWithoutPeaceOfMind { get; set; }
        public string DueCopyWithPeaceOfMind { get; set; }
        public string DueCopyWithPeaceOfMindAndFlex { get; set; }
        public string QuoteThankYouPageCopy { get; set; }
        public string QuoteExpiredMessage { get; set; }
        public string QuoteExpiredEntireFlexMessage { get; set; }
        public string QuoteTourRemovedMessage { get; set; }
        public string QuoteGenericMessage { get; set; }
    }
}
