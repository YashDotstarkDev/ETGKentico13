using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Address
    {
        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("suburb")]
        public string Suburb { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }

    public class AdditionalReference
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("customerReference")]
        public string CustomerReference { get; set; }
    }

    public class BankAccount
    {
        [JsonProperty("bsb")]
        public string Bsb { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("bankBranch")]
        public string BankBranch { get; set; }

        [JsonProperty("bankName")]
        public string BankName { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class Card
    {
        [JsonProperty("cardProxy")]
        public string CardProxy { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
    

    public class AdditionalPaymentAccountProxy
    {
        [JsonProperty("proxy")]
        public string Proxy { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class TravelPayCustomerRequest
    {
        [JsonProperty("customerReference")]
        public string CustomerReference { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("customerCompanyName")]
        public string CustomerCompanyName { get; set; }

        [JsonProperty("abn")]
        public string Abn { get; set; }

        [JsonProperty("merchantStaffMember")]
        public string MerchantStaffMember { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("promotionCode")]
        public string PromotionCode { get; set; }

        [JsonProperty("paymentOption")]
        public int PaymentOption { get; set; }

        [JsonProperty("paymentAmount")]
        public int PaymentAmount { get; set; }

        [JsonProperty("paymentFrequency")]
        public int PaymentFrequency { get; set; }

        [JsonProperty("paymentStartDate")]
        public DateTime PaymentStartDate { get; set; }

        [JsonProperty("paymentEndDate")]
        public DateTime PaymentEndDate { get; set; }

        [JsonProperty("notificationMethod")]
        public int NotificationMethod { get; set; }

        [JsonProperty("additionalReferences")]
        public List<AdditionalReference> AdditionalReferences { get; set; }

        [JsonProperty("bankAccount")]
        public BankAccount BankAccount { get; set; }

        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("enableBPayOption")]
        public bool EnableBPayOption { get; set; }

        [JsonProperty("sendWelcomeEmail")]
        public bool SendWelcomeEmail { get; set; }

        [JsonProperty("paymentAccountProxy")]
        public PaymentAccountProxy PaymentAccountProxy { get; set; }

        [JsonProperty("additionalPaymentAccountProxies")]
        public List<AdditionalPaymentAccountProxy> AdditionalPaymentAccountProxies { get; set; }
    }


}
