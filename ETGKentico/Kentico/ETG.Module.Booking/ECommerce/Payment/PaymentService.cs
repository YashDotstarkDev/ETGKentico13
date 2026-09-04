using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.Ecommerce;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Module.Booking.ECommerce.Payment.Models;
using ETG.Module.Booking.Shopping;
using Newtonsoft.Json;
using RestSharp;

namespace ETG.Module.Booking.ECommerce.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly ITravelPayService _travelPayService;
        private readonly ETGSettings _settings;
        public PaymentService(ITravelPayService travelPayService, IETGSettingsService etgSettingsService)
        {
            _travelPayService = travelPayService;
            _settings = etgSettingsService.GetSettings();
        }

        public string GetPaymentUrl(OrderInfo order, CustomerInfo customer)
        {
            //
            // https://payuat.travelpay.com.au/entiretrg?paymentAmount=100&CustomerName=Ian&customerReference=Menethil&contactNumber=&companyName=[VALUE]&additionalReference=[VALUE]&abn=[VALUE]&customerEmail=[VALUE]
             return
                $"{_settings.ETGPaymentAPIBaseUrl}?customerName={customer.CustomerInfoName}&customerEmail={customer.CustomerEmail}&contactNumber={customer.CustomerPhone}&paymentAmount={order.OrderGrandTotalInMainCurrency}&customerReference={order.OrderID}";
        }

        
        public PaymentResultInfo MakePayment(BookingCheckoutInfo bookingCheckoutInfo)
        {
            if (bookingCheckoutInfo?.Customer == null || bookingCheckoutInfo.PaymentAmount < 0)
            {
                return new PaymentResultInfo
                {
                    PaymentIsFailed = true,
                    PaymentDescription = "Invalid parameter"
                };
            }

            var result = _travelPayService.CreateCardProxy(bookingCheckoutInfo.Customer.CustomerGUID.ToString(),
                bookingCheckoutInfo.CreditCardNumber, bookingCheckoutInfo.CreditCardExpiry, 
                $"{bookingCheckoutInfo.Customer?.CustomerFirstName} {bookingCheckoutInfo.Customer?.CustomerLastName}");

            if (result.StatusCode != HttpStatusCode.Created)
            {
                return new PaymentResultInfo
                {
                    PaymentIsFailed = true,
                    PaymentDescription = "Invalid credit card details.",
                };
            }

            var cardProxyResponse = JsonConvert.DeserializeObject<TravelPayCardProxyResponse>(result.JsonResponse);

            result =_travelPayService.MakePayment(bookingCheckoutInfo.Customer, cardProxyResponse.CardProxy,
                bookingCheckoutInfo.PaymentAmount);

            if (result.StatusCode != HttpStatusCode.Created)
            {
                return new PaymentResultInfo
                {
                    PaymentIsFailed = true,
                    PaymentDescription = "Payment failed. Something went wrong."
                };
            }

            var paymentResult = JsonConvert.DeserializeObject<TravelPayPaymentResult>(result.JsonResponse);
            return new PaymentResultInfo
            {
                PaymentTransactionID = paymentResult.PaymentReference,
                PaymentDescription = result.JsonResponse,
                PaymentIsCompleted = paymentResult.FailureCode.IsNullOrEmpty(),
                PaymentDate = DateTime.Now,
                PaymentMethodID = 1,
                PaymentIsFailed = !paymentResult.FailureCode.IsNullOrEmpty(),
                
            };

            
        }
    }
}
