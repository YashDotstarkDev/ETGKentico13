using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Ecommerce;

namespace ETG.Web.Models.Forms
{
    public class BookingCustomerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string State { get; set; }
        public string Comments { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardExpiryDate { get; set; }
        public bool Subscribe { get; set; }

        public void ApplyToCustomer(CustomerInfo customer)
        {
            customer.CustomerFirstName = FirstName;
            customer.CustomerLastName = LastName;
            customer.CustomerEmail = Email;
            customer.CustomerPhone = MobilePhone;
            customer.SetValue("CustomerState", State);
            
        }
    }
}