
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Castle.Core.Internal;
using CMS.Ecommerce;
using CMS.EventLog;
using ETG.Booking.Pricing.Enums;
using ETG.Core.Encryption;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Services;
using ETG.Data.Extensions;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.Tour;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Constants;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.ECommerce.Payment.Models;
using ETG.Module.Booking.Extensions;
using ETG.Module.Booking.Models.Cart;
using Newtonsoft.Json;

namespace ETG.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IEmailService _emailService;
        private readonly IBookingDataProvider _bookingDataProvider;
        private readonly ETGSettings _settings;
        public OrderService(IEmailService emailService, IBookingDataProvider bookingDataProvider, IETGSettingsService etgSettingsService)
        {
            _emailService = emailService;
            _bookingDataProvider = bookingDataProvider;
            _settings = etgSettingsService.GetSettings();
        }
        public void SendConfirmationEmail(OrderInfo order, string refundMemberId)
        {
            
            var subject = $"Entire Travel Group - Customer Website Booking CWB{order.OrderID}";
            var parameters = new Dictionary<string, string>();
            var orderDetails = order.GetDetails();
            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
            var toEmail = customer.CustomerEmail;
            var firstName = customer.CustomerFirstName;

            var currentCurrencyPricing = new CurrentCurrencyPricing(orderDetails.Currency, orderDetails.ConversionRate);
            
            if (customer.GetStringValue("CustomerAgentEmail", string.Empty) != string.Empty)
            {
                firstName = customer.GetStringValue("CustomerAgentName", string.Empty);
                toEmail = customer.GetStringValue("CustomerAgentEmail", string.Empty);
                parameters.Add("leadpassenger", $"Lead passenger name: {customer.CustomerInfoName}");
                subject = $"Entire Travel Group - Agent Website Booking AWB{order.OrderID}";
            }

            parameters.Add("firstname", firstName);
            parameters.Add("orderid", order.OrderID.ToString());
            parameters.Add("orderdate", order.OrderDate.ToString("dd MMMM yyyy h:mm tt"));
            parameters.Add("packagecode", orderDetails.TourCode);
            parameters.Add("packagecountry",orderDetails.TourDestination);
            parameters.Add("packagename", orderDetails.TourName);
            parameters.Add("packageurl", $"http{(HttpContext.Current.Request.IsSecureConnection ? "s" : "")}://{HttpContext.Current.Request.Url.Host}{orderDetails.TourUrl}");
            parameters.Add("departuredate", orderDetails.DepartureDate.ToString("dd MMMM yyyy"));
            parameters.Add("summary", order.BookingBreakdownEmailHtml(currentCurrencyPricing));
            parameters.Add("prepostnightsdetails", order.BookingPrePostNightsEmailHtml());
            parameters.Add("refundmemberid", refundMemberId);
            if (order.OrderPaymentResult != null)
            {
                var travelPayResult =
                    JsonConvert.DeserializeObject<TravelPayJQueryPaymentResult>(order.OrderPaymentResult
                        .PaymentDescription);

                parameters.Add("baseamountpaid", travelPayResult.BaseAmount.FormatDecimalPrice(false, "AUD"));
                parameters.Add("transactionfee", travelPayResult.CustomerFee.FormatDecimalPrice(false, "AUD"));
                parameters.Add("totalpaid", travelPayResult.ProcessedAmount.FormatDecimalPrice(false, "AUD"));
                parameters.Add("cardno", travelPayResult.CardNo);
                
                parameters.Add("additionaldetails", GetEmailAdditionalDetails(orderDetails));
            }
            
            var url = $"/apply-refund?id={HttpContext.Current.Server.UrlEncode(ETGEncryptor.EncryptString(order.OrderID.ToString()))}";
            parameters.Add("applyrefundlink", url);
            _emailService.SendEmail("BookNowConfirmation", toEmail, parameters, true, string.Empty, string.Empty, string.Empty, subject);
        }

        public void SendNotificationEmail(OrderInfo order)
        {
            var subject = $"Entire Travel Group - Customer Website Booking CWB{order.OrderID}";
            var notificationEmail = _settings.BookNowNotificationEmail;
            var parameters = new Dictionary<string, string>();
            var orderDetails = order.GetDetails();
            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
            
            var currentCurrencyPricing = new CurrentCurrencyPricing(orderDetails.Currency, orderDetails.ConversionRate);

            if (customer.GetStringValue("CustomerAgentEmail", string.Empty) != string.Empty)
            {
                subject = $"Entire Travel Group - Agent Website Booking AWB{order.OrderID}";
            }

            parameters.Add("orderid", order.OrderID.ToString());
            parameters.Add("orderdate", order.OrderDate.ToString("dd MMMM yyyy h:mm tt"));
            parameters.Add("packagecode", orderDetails.TourCode);
            parameters.Add("packagecountry",orderDetails.TourDestination);
            parameters.Add("packagename", orderDetails.TourName);
            parameters.Add("packageurl", $"http{(HttpContext.Current.Request.IsSecureConnection ? "s" : "")}://{HttpContext.Current.Request.Url.Host}{orderDetails.TourUrl}" );
            parameters.Add("departuredate", orderDetails.DepartureDate.ToString("dd MMMM yyyy"));
            parameters.Add("summary", order.BookingBreakdownEmailHtml(currentCurrencyPricing));
            parameters.Add("customer", order.BookingCustomerInfoAdminHtml());
            parameters.Add("prepostnightsdetails", order.BookingPrePostNightsEmailHtml());
            if (order.OrderPaymentResult != null)
            {
                var travelPayResult =
                    JsonConvert.DeserializeObject<TravelPayJQueryPaymentResult>(order.OrderPaymentResult
                        .PaymentDescription);
                parameters.Add("baseamountpaid", travelPayResult.BaseAmount.FormatDecimalPrice(false, "AUD"));
                parameters.Add("transactionfee", travelPayResult.CustomerFee.FormatDecimalPrice(false, "AUD"));
                parameters.Add("totalpaid", travelPayResult.ProcessedAmount.FormatDecimalPrice(false, "AUD"));
                parameters.Add("cardno", travelPayResult.CardNo);
                
                parameters.Add("additionaldetails", GetEmailAdditionalDetails(orderDetails));
            }
            _emailService.SendEmail("BookNowNotification", notificationEmail, parameters, true, string.Empty, string.Empty, string.Empty, subject);
        }

        private string GetEmailAdditionalDetails(DetailedPackageBooking orderDetails)
        {
            if (orderDetails == null)
            {
                return string.Empty;
            }

            var html = new StringBuilder();
            if (orderDetails.SecondInstalmentDate > DateTime.MinValue && orderDetails.SecondInstalmentPercentage > 0)
            {
                var amount = (double) orderDetails.Order.OrderGrandTotalInMainCurrency *
                             orderDetails.SecondInstalmentPercentage * 0.01;
                html.AppendLine(
                    $"Second deposit instalment of {amount.FormatDecimalPrice()} is due on {orderDetails.SecondInstalmentDate:dd/MM/yyyy}<br>");
            }
            
            if (orderDetails.PaymentBalanceDueDate > DateTime.MinValue)
            {
                
                html.AppendLine(
                    $"Final balance will be due on {orderDetails.PaymentBalanceDueDate:dd/MM/yyyy}<br><br>");
            }

            return html.ToString();
        }

        public void SendConfirmationEmail(PaymentInfo payment)
        {
            var subject = $"Entire Travel Group - Payment Confirmation - {payment.PaymentInvoiceReference}";
            var parameters = new Dictionary<string, string>();
            
            parameters.Add("paymentid", payment.PaymentID.ToString());
            parameters.Add("firstname", payment.PaymentFirstName);
            parameters.Add("lastname", payment.PaymentLastName);
            parameters.Add("email", payment.PaymentEmail);
            parameters.Add("contactnumber", payment.PaymentContactNumber);
            parameters.Add("referenceno", payment.PaymentInvoiceReference);

            var travelPayResult =
                JsonConvert.DeserializeObject<TravelPayJQueryPaymentResult>(payment.PaymentDetails);
            parameters.Add("baseamountpaid", travelPayResult.BaseAmount.FormatDecimalPrice());
            parameters.Add("transactionfee", travelPayResult.CustomerFee.FormatDecimalPrice());
            parameters.Add("totalpaid", travelPayResult.ProcessedAmount.FormatDecimalPrice());
            parameters.Add("cardno", travelPayResult.CardNo);
    
            _emailService.SendEmail("PaymentConfirmation", payment.PaymentEmail, parameters, true, string.Empty, string.Empty, string.Empty, subject);
        }

        public void SendNotificationEmail(PaymentInfo payment)
        {
            var subject = $"Entire Travel Group - Payment Notification - {payment.PaymentInvoiceReference}";
            var notificationEmail = _settings.BookNowNotificationEmail;
            var parameters = new Dictionary<string, string>();


            parameters.Add("paymentid", payment.PaymentID.ToString());
            parameters.Add("firstname", payment.PaymentFirstName);
            parameters.Add("lastname", payment.PaymentLastName);
            parameters.Add("email", payment.PaymentEmail);
            parameters.Add("contactnumber", payment.PaymentContactNumber);
            parameters.Add("referenceno", payment.PaymentInvoiceReference);
            
            var travelPayResult =
                    JsonConvert.DeserializeObject<TravelPayJQueryPaymentResult>(payment.PaymentDetails);
                parameters.Add("baseamountpaid", travelPayResult.BaseAmount.FormatDecimalPrice());
                parameters.Add("transactionfee", travelPayResult.CustomerFee.FormatDecimalPrice());
                parameters.Add("totalpaid", travelPayResult.ProcessedAmount.FormatDecimalPrice());
                parameters.Add("cardno", travelPayResult.CardNo);
        
            _emailService.SendEmail("PaymentNotification", notificationEmail, parameters, true, string.Empty, string.Empty, "operations@entiretravel.com.au", subject);
        }

        
    }
}