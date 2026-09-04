namespace ETG.Data.Settings.Models
{
    public class ETGSettings
    {
        public string GTMID { get; set; }
        public string GoogleAnalyticID { get; set; }
        public string FacebookPixelId { get; set; }
        public bool IsLiveEnvironment { get; set; }
        public double BookingRequiredDeposit { get; set; }
        public double EntireFlexCostPerPerson { get; set; }
        public int EntireFlexThresholdDays { get; set; }
        public string ETGPaymentAPIBaseUrl { get; set; }
        public string ETGPaymentAPIKey { get; set; }
        public string ETGPaymentAPIUsername { get; set; }
        public string ETGPaymentAPIPassword { get; set; }
        public string ETGPaymentResultUrl { get; set; }
        public string BookNowNotificationEmail { get; set; }
        public string TravelpayCSSLink { get; set; }
        public string TravelpayJSLink { get; set; }
        public string RefundProtectEndpoint { get; set; }
        public string RefundProtectVendorID { get; set; }
        public string RefundProtectAPIKey { get; set; }
        public double RefundProtectPremiumRate { get; set; }
        public string RefundProtectMemberID { get; set; }
        public int BookingQuoteValidityDays { get; set; }
        public string ETGPayNowProcessingUrl { get; set; }
        public int SecondInstalmentDays { get; set; }
        public double SecondInstalmentPercentage { get; set; }
        public double SaleComissionPCT { get; set; }
        public string ETGPaymentMerchantCode {get;set;}
        public string TravelpayBS5JSLink { get; set; }
    }
}
