using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class RefundProtectProduct
    {
        [JsonProperty("productCode")]
        public string ProductCode { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("productPrice")]
        public double ProductPrice { get; set; }

        [JsonProperty("premiumRate")]
        public double PremiumRate { get; set; }

        [JsonProperty("offeringMethod")]
        public string OfferingMethod { get; set; }

        [JsonProperty("sold")]
        public bool Sold { get; set; }

        [JsonProperty("insuranceEndDate")]
        public string InsuranceEndDate { get; set; }
    }

    public class RefundProtectRequest
    {
        [JsonProperty("vendorCode")]
        public string VendorCode { get; set; }

        [JsonProperty("vendorSalesReferenceId")]
        public string VendorSalesReferenceId { get; set; }

        [JsonProperty("VendorSalesDate")]
        public string VendorSalesDate { get; set; }

        [JsonProperty("customerFirstName")]
        public string CustomerFirstName { get; set; }

        [JsonProperty("customerLastName")]
        public string CustomerLastName { get; set; }

        [JsonProperty("products")]
        public List<RefundProtectProduct> Products { get; set; }
    }
}
