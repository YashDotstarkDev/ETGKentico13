using System;
using CMS.Ecommerce;
using ETG.Core.Services;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Module.Booking.ECommerce.Payment.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.IO;
using System.Text;

namespace ETG.Module.Booking.ECommerce.Payment
{
    public class TravelPayService : ITravelPayService
    {
        private readonly ETGSettings settings;
        private readonly ILogger _logger;
        public TravelPayService(IETGSettingsService etgSettingsService, ILogger logger)
        {
            _logger = logger;
            settings = etgSettingsService.GetSettings();
        }

        private void LogError(IRestResponse response, string eventCode, string url, string request)
        {
            _logger.LogInformation("TRAVELPAY", eventCode, $"Url:{url}\r\nRequest:{request}\r\nStatusCode: {response.StatusCode}\r\nError:{response.ErrorMessage}\r\nError Exception:{response.ErrorException}");

        }

        private string PostData(string url, string json = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = "POST";
            request.Headers.Add("Api-Key", settings.ETGPaymentAPIKey);

            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                .GetBytes(settings.ETGPaymentAPIUsername + ":" + settings.ETGPaymentAPIPassword));
            request.Headers.Add("Authorization", "Basic " + encoded);
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            return reader.ReadToEnd();
        }
        public TravelPayResult CreateCardProxy(string customerUniqueId, string cardNumber, string cardExpiry, string cardHolderName)
        {
            var cardProxyRequest = new TravelPayCardProxyRequest
            {
                cardHolderName = cardHolderName,
                cardNumber = cardNumber,
                expiry = cardExpiry,
                customerUniqueId = customerUniqueId,
                paymentAmount = 0
            };

            
            var client = new RestClient($"{settings.ETGPaymentAPIBaseUrl}");
            var request = new RestRequest("v2/cardproxies", Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(settings.ETGPaymentAPIUsername, settings.ETGPaymentAPIPassword);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Api-Key", settings.ETGPaymentAPIKey);
            request.AddJsonBody(cardProxyRequest);
            
            //request.AddHeader("content-type", "application/x-www-form-urlencoded");
            //request.AddParameter("application/x-www-form-urlencoded", $"customerUniqueId={cardProxyRequest.CustomerUniqueId}&cardNumber={cardProxyRequest.CardNumber}&expiry={cardProxyRequest.Expiry}", ParameterType.RequestBody);

            //PostData($"{settings.ETGPaymentAPIBaseUrl}/v2/cardproxies", JsonConvert.SerializeObject(cardProxyRequest));

            //client.Execute(request);
            var response = client.Post(request);
            if (!response.IsSuccessful && response.StatusCode != HttpStatusCode.Created)
            {
                LogError(response, "CreateCardProxy", $"{settings.ETGPaymentAPIBaseUrl}/v2/cardproxies",
                    JsonConvert.SerializeObject(cardProxyRequest));
            }
            return new TravelPayResult
            {
                StatusCode = response.StatusCode,
                JsonResponse = response.Content
            };
        }

        public TravelPayResult CreateCustomer(CustomerInfo customer)
        {
            var customerRequest = new TravelPayCustomerRequest
            {
                FirstName = customer.CustomerFirstName,
                LastName = customer.CustomerLastName,
                Email = customer.CustomerEmail,
                Mobile = customer.CustomerPhone
            };
            var client = new RestClient($"{settings.ETGPaymentAPIBaseUrl}/v2/customers");
            var request = new RestRequest(JsonConvert.SerializeObject(customerRequest), DataFormat.Json);
            var response = client.Post(request);

            if (!response.IsSuccessful && response.StatusCode != HttpStatusCode.Created)
            {
                _logger.LogInformation("TRAVELPAY", "CreateCustomer", $"StatusCode: {response.StatusCode}\r\nError:{response.ErrorMessage}\r\nError Exception:{response.ErrorException}");
            }
            return new TravelPayResult
            {
                StatusCode = response.StatusCode,
                JsonResponse = response.Content
            };
        }

        public TravelPayResult MakePayment(CustomerInfo customer, string cardProxy, double amount)
        {
            var paymentRequest = new TravelPayPaymentRequest
            {
                CustomerReference = customer.CustomerGUID.ToString(),
                CustomerName = $"{customer?.CustomerFirstName} {customer?.CustomerLastName}",
                CustomerEmail = customer?.CustomerEmail,
                ContactNumber = customer.CustomerPhone,
                CardProxy = cardProxy,
                PaymentAmount = amount,
            };

            var client = new RestClient($"{settings.ETGPaymentAPIBaseUrl}");
            var request = new RestRequest("v2/payments", Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(settings.ETGPaymentAPIUsername, settings.ETGPaymentAPIPassword);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Api-Key", settings.ETGPaymentAPIKey);
            request.AddJsonBody(paymentRequest);
            var response = client.Post(request);

            if (!response.IsSuccessful && response.StatusCode != HttpStatusCode.Created)
            { 
                LogError(response, "MakePayment", $"{settings.ETGPaymentAPIBaseUrl}/v2/payments",
                    JsonConvert.SerializeObject(paymentRequest));
            }
            return new TravelPayResult
            {
                StatusCode = response.StatusCode,
                JsonResponse = response.Content
            };
        }
        
    }
}
