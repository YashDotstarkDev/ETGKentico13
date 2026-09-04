namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class TravelPayJQueryPaymentResult
    {
        public string PaymentReference { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusString { get; set; }
        public string FailureCode { get; set; }
        public string FailureReason { get; set; }
        public double BaseAmount { get; set; }
        public double CustomerFee { get; set; }
        public double ProcessedAmount { get; set; }
        public string CardNo { get; set; }
        public string CardType { get; set; }
        public double FundsToMerchant { get; set; }
    }
}
