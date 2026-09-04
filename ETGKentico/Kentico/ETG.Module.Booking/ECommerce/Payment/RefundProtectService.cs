using CMS.Ecommerce;
using CMS.Helpers;
using ETG.Core.Services;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Module.Booking.ECommerce.Payment.Models;
using ETG.Module.Booking.Extensions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using ETG.Core.Encryption;
using ETG.Data.Models.Base;
using ETG.Module.Booking.Constants;

namespace ETG.Module.Booking.ECommerce.Payment
{
    public class RefundProtectService : IRefundProtectService
    {
        private readonly ETGSettings _settings;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        public RefundProtectService(IETGSettingsService etgSettingsService, IEmailService emailService,
            ILogger logger)
        {
            _logger = logger;
            _emailService = emailService;
            _settings = etgSettingsService.GetSettings();
        }

        private void LogError(IRestResponse response, string eventCode, string url, string request)
        {
            _logger.LogInformation("RefundProtect", eventCode, $"Url:{url}\r\nRequest:{request}\r\nResponse:{response.Content}\r\nStatusCode: {response.StatusCode}\r\nError:{response.ErrorMessage}\r\nError Exception:{response.ErrorException}");

        }

        private RestRequest GetRequestHeader(Method method)
        {
            var request = new RestRequest("", method);

            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Api-Key", _settings.ETGPaymentAPIKey);
            request.AddHeader("X-RefundProtect-VendorId", _settings.RefundProtectVendorID);
            request.AddHeader("X-RefundProtect-AuthToken", _settings.RefundProtectAPIKey);
            return request;
        }

        public RefundProtectResult PostRefundProtect(OrderInfo order, CustomerInfo customer, bool customerChooseRefundProtect)
        {

            var summary = order.GetOrderSummary();
            var refundProtectRequest = new RefundProtectRequest
            {
                VendorCode = _settings.RefundProtectVendorID,
                VendorSalesReferenceId = order.OrderID.ToString(),
                VendorSalesDate = order.OrderDate.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFzzz"),
                CustomerFirstName = customer.CustomerFirstName,
                CustomerLastName = customer.CustomerLastName,
                Products = new List<RefundProtectProduct>
                {
                    new RefundProtectProduct
                    {
                        CurrencyCode = "AUD",
                        ProductCode = "PKG",
                        InsuranceEndDate = summary.DepartureDate.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFzzz"),
                        ProductPrice = ValidationHelper.GetDouble(order.OrderGrandTotalInMainCurrency, 0),
                        Sold = customerChooseRefundProtect,
                        PremiumRate = _settings.RefundProtectPremiumRate,
                        OfferingMethod = "OPT-OUT"
                    }
                }

            };

            var client = new RestClient($"{_settings.RefundProtectEndpoint}/salesoffering");
            var request = GetRequestHeader(Method.POST);
            request.AddJsonBody(refundProtectRequest);
            
            var response = client.Post(request);
            if (!response.IsSuccessful && response.StatusCode != HttpStatusCode.OK)
            {
                LogError(response, "PostRefundProtect", $"{_settings.RefundProtectEndpoint}",
                    JsonConvert.SerializeObject(refundProtectRequest));
            }
            else
            {
                try
                {
                    _logger.LogInformation("RefundProtect", "Success",
                        $"Url:{_settings.RefundProtectEndpoint}\r\nRequest:{JsonConvert.SerializeObject(refundProtectRequest)}\r\nStatusCode: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                 _logger.LogException("RefundProtect", "LogError", ex, string.Empty);   
                }
            }
            return new RefundProtectResult
            {
                IsSuccessful = response.IsSuccessful,
                StatusCode = response.StatusCode,
                JsonResponse = response.Content
            };
        }

        public CancelRefundResult CancelOrder(int orderId)
        {
            var client = new RestClient($"{_settings.RefundProtectEndpoint}/{orderId}");
            var request = GetRequestHeader(Method.DELETE);
           
            var response = client.Delete(request);
            if (!response.IsSuccessful && response.StatusCode != HttpStatusCode.OK)
            {
                LogError(response, "CancelOrder", $"{_settings.RefundProtectEndpoint}",
                    orderId.ToString());
            }
            return new CancelRefundResult
            {
                StatusCode = response.StatusCode,
                JsonResponse = response.Content
            };
        }

        public RefundProtectResult PatchRefundProtectValue(int orderId, double value, bool fromAdmin = false)
        {
            var order = OrderInfoProvider.GetOrderInfo(orderId);

            if (order == null)
            {
                return new RefundProtectResult
                {
                    IsSuccessful = false
                };
            }

            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
            var summary = order.GetOrderSummary(fromAdmin);
            var refundProtectRequest = new RefundProtectRequest
            {
                VendorCode = _settings.RefundProtectVendorID,
                VendorSalesReferenceId = orderId.ToString(),
                VendorSalesDate = order.OrderDate.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFzzz"),
                CustomerFirstName = customer.CustomerFirstName,
                CustomerLastName = customer.CustomerLastName,
                Products = new List<RefundProtectProduct>
                {
                    new RefundProtectProduct
                    {
                        CurrencyCode = "AUD",
                        ProductCode = "PKG",
                        InsuranceEndDate = summary.DepartureDate.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFzzz"),
                        ProductPrice = value,
                        Sold = true,
                        PremiumRate = _settings.RefundProtectPremiumRate,
                        OfferingMethod = "OPT-OUT"
                    }
                }

            };

            var client = new RestClient($"{_settings.RefundProtectEndpoint}/salesoffering");
            var request = GetRequestHeader(Method.PUT);
            request.AddJsonBody(refundProtectRequest);

            var response = client.Patch(request);
            if (!response.IsSuccessful && response.StatusCode != HttpStatusCode.OK)
            {
                LogError(response, "PatchRefundProtectValue", $"{_settings.RefundProtectEndpoint}",
                    JsonConvert.SerializeObject(refundProtectRequest));
            }
            return new RefundProtectResult
            {
                IsSuccessful = response.IsSuccessful,
                StatusCode = response.StatusCode,
                JsonResponse = response.Content
            };
        }

        public RefundProtectResult PatchRefundProtectDate(int orderId, DateTime newDate, bool fromAdmin = false)
        {
            var order = OrderInfoProvider.GetOrderInfo(orderId);

            if (order == null)
            {
                return new RefundProtectResult
                {
                    IsSuccessful = false
                };
            }

            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
            var refundProtectRequest = new RefundProtectRequest
            {
                VendorCode = _settings.RefundProtectVendorID,
                VendorSalesReferenceId = orderId.ToString(),
                VendorSalesDate = order.OrderDate.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFzzz"),
                CustomerFirstName = customer.CustomerFirstName,
                CustomerLastName = customer.CustomerLastName,
                Products = new List<RefundProtectProduct>
                {
                    new RefundProtectProduct
                    {
                        CurrencyCode = "AUD",
                        ProductCode = "PKG",
                        InsuranceEndDate = newDate.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFzzz"),
                        ProductPrice = 0,
                        Sold = true,
                        PremiumRate = _settings.RefundProtectPremiumRate,
                        OfferingMethod = "OPT-OUT"
                    }
                }

            };

            var client = new RestClient($"{_settings.RefundProtectEndpoint}/salesoffering");
            var request = GetRequestHeader(Method.PUT);
            request.AddJsonBody(refundProtectRequest);

            var response = client.Patch(request);
            if (!response.IsSuccessful && response.StatusCode != HttpStatusCode.OK)
            {
                LogError(response, "PatchRefundProtectDate", $"{_settings.RefundProtectEndpoint}",
                    JsonConvert.SerializeObject(refundProtectRequest));
            }
            return new RefundProtectResult
            {
                IsSuccessful = response.IsSuccessful,
                StatusCode = response.StatusCode,
                JsonResponse = response.Content
            };
        }
        
        public Result SendApplyRefundEmail(OrderInfo order, string refundMemberId)
        {
            var result = new Result();
            var subject = $"Entire Travel Group - Refund Link - {order.OrderID}";

            if (order.OrderStatusID == BookingConstants.ORDER_STATUS_CANCELLED)
            {
                result.Message = "Cannot send email, order has been cancelled.";
                return result;
            }

            if (order.OrderStatusID != BookingConstants.ORDER_STATUS_PAID)
            {
                result.Message = "Cannot send email for unpaid order.";
                return result;
            }
            
            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);

            if (customer == null)
            {
                result.Message = "Invalid order. Customer info missing";
                return result;
            }
            var parameters = new Dictionary<string, string>();
            
            parameters.Add("firstname", customer.CustomerFirstName);
            parameters.Add("orderid", order.OrderID.ToString());
            var url = $"/apply-refund?id={HttpContext.Current.Server.UrlEncode(ETGEncryptor.EncryptString(order.OrderID.ToString()))}";
            parameters.Add("applyrefundlink", url);
            //parameters.Add("refundmemberid", refundMemberId);
            _emailService.SendEmail("ApplyForRefundLink", customer.CustomerEmail, parameters, 
                true,string.Empty, String.Empty, string.Empty, subject);

            result.Success = true;
            return result;
        }
        
        public Result SendApplyRefundEmailNotificationToAdmin(OrderInfo order)
        {
            var result = new Result();
            var subject = $"Entire Travel Group - Apply Refund - Notification";

            if (order.OrderStatusID == BookingConstants.ORDER_STATUS_CANCELLED)
            {
                result.Message = "This order has been cancelled.";
                return result;
            }

            if (order.OrderStatusID != BookingConstants.ORDER_STATUS_PAID)
            {
                result.Message = "This order is unpaid.";
                return result;
            }
            
            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);

            if (customer == null)
            {
                result.Message = "Invalid order. Customer info missing";
                return result;
            }
            var parameters = new Dictionary<string, string>();
            
            parameters.Add("orderid", order.OrderID.ToString());
            parameters.Add("topdognumber", order.GetStringValue("OrderTopDogNumber", string.Empty));
            _emailService.SendEmail("ApplyForRefundNotificationToAdmin", _settings.BookNowNotificationEmail, parameters, 
                true,string.Empty, String.Empty, string.Empty, subject);

            result.Success = true;
            return result;
        }
    }
}
