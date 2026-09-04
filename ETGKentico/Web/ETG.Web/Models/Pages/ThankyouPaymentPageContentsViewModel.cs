using ETG.Data.Models.Booking;
using System.Collections.Generic;
using ETG.Data.Settings.Models;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.ECommerce.Payment.Models;
using ETG.Web.Tour.Models.DataLayer;
using Newtonsoft.Json;

namespace ETG.Web.Models.Pages
{
    public class ThankyouPaymentPageContentsViewModel : IViewModel
    {
        private TravelPayJQueryPaymentResult _travelPayJQueryPaymentResult;

        public TravelPayJQueryPaymentResult TravelPayPaymentResult
        {
            get
            {
                if (_travelPayJQueryPaymentResult != null)
                {
                    return _travelPayJQueryPaymentResult;
                }

                if (Payment == null)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<TravelPayJQueryPaymentResult>(Payment.PaymentAdditionalInfo);
            }
        }

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

        
        public string HeroImage { get; set; }
        public string ThankYouCopy { get; set; }
        public PaymentInfo Payment { get; set; }
        

    }
}
