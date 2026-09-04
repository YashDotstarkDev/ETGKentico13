using CMS.Ecommerce;
using DocumentFormat.OpenXml.Drawing.Charts;
using ETG.Data.Tour;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.ECommerce.Payment.Models;
using ETG.Web.Tour.Models.DataLayer;
using Newtonsoft.Json;

namespace ETG.Web.Models.TourBooking
{
    
    public class BookingSuccessViewModel
    {
        public bool IsFullPayment { get; set; }
        public TravelPayJQueryPaymentResult TravelPayPaymentResult
        {
            get
            {
                if (Order == null)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<TravelPayJQueryPaymentResult>(Order.OrderPaymentResult
                    .PaymentDescription);
            }
        }
        
        public OrderInfo Order { get; set; }

        public double CardTransactionFee
        {
            get
            {
                if (TravelPayPaymentResult == null)
                {
                    return 0;
                }

                return TravelPayPaymentResult.CustomerFee;
            }
            
        }
        public double TotalPaid
        {
            get
            {
                if (TravelPayPaymentResult == null)
                {
                    return 0;
                }

                return TravelPayPaymentResult.ProcessedAmount;
            }
        }

        public double BaseAmountPaid
        {
            get
            {
                if (TravelPayPaymentResult == null)
                {
                    return 0;
                }

                return TravelPayPaymentResult.BaseAmount;
            }
        }

        public double BaseAmountPaidInCurrentCurrency => IsFullPayment ? OrderTotalPriceInCurrentCurrency : CurrentCurrencyPricing.ConvertAUDToCurrentCurrency(BaseAmountPaid);
        public double BalanceDueInCurrentCurrency => IsFullPayment ? 0 : OrderTotalPriceInCurrentCurrency - BaseAmountPaidInCurrentCurrency;

        public string CardNo
        {
            get
            {
                if (TravelPayPaymentResult == null)
                {
                    return "";
                }

                return TravelPayPaymentResult.CardNo;
            }
        }

        
        public string TourCode { get; set; }
        public string TourName { get; set; }
        public string HeroImage { get; set; }
        public string ThankYouCopy { get; set; }
        
        public double OrderTotalPriceInCurrentCurrency { get; set; }
        
        public EcommerceDataLayerRoot EcommerceDataLayer { get; set; }
        
        public CurrentCurrencyPricing CurrentCurrencyPricing { get; set; }
    }
}