namespace ETG.Data.Models.eCommerce
{
    public class TravelPayParameters
    {
        public int OrderId { get; set; }
        public string APIUrl { get; set; }
        public string ApiKey { get; set; }
        public string Fingerprint { get; set; }
        public string CustomerName { get; set; }
        public string CustomerReference { get; set; }
        public string CustomerEmail { get; set; }
        public double PaymentAmount { get; set; }
        public int Mode { get; set; }
        public string Timestamp { get; set; }
        public string MerchantCode { get; set; }

    }
}
