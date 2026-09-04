using ETG.Core.Services;
using ETG.Data.Settings;
using ETG.Module.Booking.ECommerce.Payment;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using CMS.Ecommerce;
using CMS.Tests;
using ETG.Module.Booking.ECommerce.Payment.Models;
using Newtonsoft.Json;

namespace ETG.Booking.Test
{
    [TestFixture]
    public class TravelpayTest : UnitTests
    {
        private IETGSettingsService _etgSettingsService;
        private ITravelPayService _travelPayService;
        private ILogger _logger;
        [SetUp]
        public void Setup()
        {

            _logger = Substitute.For<ILogger>();
            _etgSettingsService = Substitute.For<IETGSettingsService>();
            _etgSettingsService.GetSettings().Returns(new Data.Settings.Models.ETGSettings
            {
                ETGPaymentAPIBaseUrl = "https://apiuat.travelpay.com.au",
                ETGPaymentAPIKey = "7d2c49e3-94a7-441d-8e21-671ecc12faca",
                ETGPaymentAPIUsername = "entiretrg",
                ETGPaymentAPIPassword = "!FR$#f2z"
            });

            // Prepares faked data for the UserInfoProvider
            Fake<CustomerInfo, CustomerInfoProvider>().WithData(
                new CustomerInfo
                {
                    CustomerGUID = Guid.NewGuid(),
                    CustomerFirstName = "Jowen",
                    CustomerLastName = "See",
                    CustomerEmail = "jowen@devotion.com.au"
                });


            _travelPayService = new TravelPayService(_etgSettingsService, _logger);
        }
        
        [Test]
        public void Test_CardProxies()
        {
            var result = _travelPayService.CreateCardProxy(Guid.NewGuid().ToString().Replace("-",""), "4000000000001091",  "11/23", "");

            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_Payment()
        {
            var cardResult = _travelPayService.CreateCardProxy(Guid.NewGuid().ToString(), "4111111111111111", "11/23", "Jowen See");

            if (cardResult.StatusCode != HttpStatusCode.Created)
            {
                return;
            }

            var cardProxyResponse = JsonConvert.DeserializeObject<TravelPayCardProxyResponse>(cardResult.JsonResponse);

            var customerInfo = CustomerInfoProvider.GetCustomers().FirstOrDefault();
            var result = _travelPayService.MakePayment(customerInfo, cardProxyResponse.CardProxy, 2000);
            Assert.IsNotNull(result);
        }
    }
}
