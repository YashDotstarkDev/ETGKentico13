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
    public class RefundProtectTest : UnitTests
    {
        private IETGSettingsService _etgSettingsService;
        private IRefundProtectService _refundProtectService;
        private ILogger _logger;
        [SetUp]
        public void Setup()
        {

            _logger = Substitute.For<ILogger>();
            _etgSettingsService = Substitute.For<IETGSettingsService>();
            _etgSettingsService.GetSettings().Returns(new Data.Settings.Models.ETGSettings
            {
                RefundProtectEndpoint = "https://test.api.protectgroup.co/api/v1/refundprotect",
                RefundProtectVendorID = "ven_local_d7b10c895c8f4029ba73f2431735469d",
                RefundProtectAPIKey = "sk_local_de521b4f37fc46e0b8e0ea5d70bd437d",
                RefundProtectPremiumRate = 1,
                
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

            Fake<OrderInfo, OrderInfoProvider>().WithData(
                new OrderInfo
                {
                    OrderID = 1000,
                    OrderGrandTotalInMainCurrency = 1234,
                    OrderTotalPrice = 1234,
                    OrderDate = DateTime.Now
                });

            _refundProtectService = new RefundProtectService(_etgSettingsService, _logger);
        }
        
        [Test]
        public void Test_Post()
        {
            var customerInfo = CustomerInfoProvider.GetCustomers().FirstOrDefault();

            var order = OrderInfoProvider.GetOrders().FirstOrDefault();
            order.Update();
            var result = _refundProtectService.PostRefundProtect(order, customerInfo, true);
            Assert.IsNotNull(result);
        }
        [Test]
        public void Test_Cancel()
        {
            var result = _refundProtectService.CancelOrder(1000);
            Assert.IsNotNull(result);
        }
        [Test]
        public void Test_Patch()
        {
            var result = _refundProtectService.CancelOrder(1000);
            Assert.IsNotNull(result);
        }
    }
}
