using System.Collections.Generic;
using CMS.Ecommerce;
using ETG.Data.Models.Booking;
using ETG.Data.Tour;
using ETG.Module.Booking.ECommerce.Payment.Models;
using ETG.Web.Tour.Models.DataLayer;
using Newtonsoft.Json;

namespace ETG.Web.Models.TourBooking
{
    public class OtherPaymentSuccessViewModel
    {
        public string TourCode { get; set; }
        public string TourName { get; set; }
        public string HeroImage { get; set; }
        public string ThankYouCopy { get; set; }

        public double TotalDepositDue { get; set; }
        public string  CustomerName { get; set; }
        public OrderInfo Order { get; set; }
        
        public EcommerceDataLayerRoot EcommerceDataLayer { get; set; }
        
        public List<PaymentOptionModel> PaymentOptions { get; set; }
        public CurrentCurrencyPricing CurrentCurrencyPricing { get; set; }
    }
}