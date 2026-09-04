using Castle.Core.Internal;
using CMS.Base;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.UIControls;
using ETG.Core.Extensions;
using ETG.Data.Extensions;
using ETG.Module.Booking.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using ETG.Booking.Pricing.Enums;
using ETG.Data.Settings;
using ETG.Data.Tour;
using ETG.Module.Booking.Admin;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.Constants;
using ETG.Module.Booking.ECommerce;
using ETG.Module.Booking.ECommerce.Payment;

namespace CMSApp.CMSModules.ETGBooking.PageTemplates
{
    [EditedObject(OrderInfo.OBJECT_TYPE, "orderId")]
    public partial class OrderBookingDetails : CMSEcommercePage
    {
        private OrderInfo order;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            order = EditedObject as OrderInfo;
            if (order != null)
            {
                var orderData = order.GetDetails();
                var currentCurrencyPricing =  new CurrentCurrencyPricing(orderData.Currency,
                    orderData.ConversionRate);

                if (!currentCurrencyPricing.CurrentCurrencyIsAUD)
                {
                    pnlExchangeRate.Visible = true;
                    litExchangeRate.Text = $"1 AUD = {orderData.Currency} {orderData.ConversionRate:0.00}";
                }
                
                var quoteId = orderData.SourceQuoteID;

                if (quoteId > 0)
                {
                    plcQuote.Visible = true;
                    litQuoteID.Text = quoteId.ToString();
                }

                if (order.OrderStatusID != BookingConstants.ORDER_STATUS_PAID)
                {
                    btnSendRefund.Visible = false;
                }

                litOrderId.Text = order.OrderID.ToString();
                hidOrderID.Value = order.OrderID.ToString();
                txtTopDog.Text = order.GetStringValue("OrderTopDogNumber", string.Empty);
                litTourCode.Text = orderData.TourCode;
                litTourName.Text = orderData.TourName;
                litCountry.Text = $"{orderData.TourDestination}";
                litOnSaleNow.Text = orderData.IsOnSaleNow.YesOrNo();
                //litFOCEntireFlex.Text = orderData.IsFOCEntireFlex.YesOrNo();
                litDate.Text = orderData.DepartureDate.ToString("dd/MM/yyyy").Substring(0, 10);
                litDaysFromDepartureDate.Text = orderData.DepartureDate.Subtract(orderData.Order.OrderDate.Date).Days.ToString();
                litDoubleRooms.Text = orderData.TwinShareRoomCount.ToString();
                litSingleRooms.Text = orderData.SingleRoomCount.ToString();
                litTwinShareRoomsType.Text = order.GetTwinRoomsTypeHtml();
                if (orderData.RoomOptions != null && !orderData.RoomOptions.RoomOptions.IsNullOrEmpty())
                {
                    repRoomOptions.DataSource = orderData.RoomOptions.RoomOptions;
                    repRoomOptions.DataBind();
                }

                if (orderData.Extras != null)
                {
                    litExtraSelectNow.Text = orderData.Extras.SelectExtrasNow.YesOrNo("Maybe Later", true);
                    repExtras.DataSource = orderData.Extras.Extras;
                    repExtras.DataBind();
                }

                if (orderData.FreedomOfChoices != null)
                {
                    litFocSelectNow.Text = orderData.FreedomOfChoices.SelectNow.YesOrNo("Select Later", true);
                    repFreedomOfChoice.DataSource = orderData.FreedomOfChoices.FreedomOfChoiceItems;
                    repFreedomOfChoice.DataBind();
                }

                /*if (orderData.EntireFlex != null)
                {
                    litEntireFlex.Text = orderData.EntireFlex.AvailEntireFlexOption.YesOrNo();
                }*/

                litInsuranceAssistance.Text = orderData.TravelInsuranceAssistance.YesOrNo();
                litFareAssistance.Text = orderData.InternationalAirfareAssistance.YesOrNo();

                if (orderData.Order.OrderPaymentResult != null)
                {
                    litTravelPaymentReference.Text = orderData.Order.OrderPaymentResult.PaymentTransactionID.ToString();
                    litFundsToMerchant.Text = orderData.Order.GetDoubleValue("OrderAmountPaid", 0).FormatPrice();
                }

                if (orderData.SecondInstalmentDate > DateTime.MinValue && 
                    orderData.SecondInstalmentPercentage > 0)
                {
                    var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

                    if (!orderItems.IsNullOrEmpty())
                    {
                        var bookingDataProvider = ServiceLocator.Current.GetInstance<IBookingDataProvider>();
                        var summary = bookingDataProvider.GetSummaryData(orderItems.FirstOrDefault()?.OrderItemCustomData);

                        //currentCurrencyPricing.GetCurrentCurrencyDisplayPrice()
                        var instalmentAmount =
                            orderData.SecondInstalmentPercentage * 0.01 * ((double)summary.TotalPrice);
                        if (currentCurrencyPricing.CurrentCurrencyIsAUD)
                        {
                            litSecondInstallmentAmount.Text = instalmentAmount.FormatDecimalPrice(true);   
                        }
                        else
                        {
                            litSecondInstallmentAmount.Text = $"{currentCurrencyPricing.GetCurrentCurrencyDisplayPrice(instalmentAmount)} ({instalmentAmount.FormatDecimalPrice(true, CurrencyConstants.CODE_AUD )})";   
                        }   
                    }
                   
                    litSecondInstallmentDate.Text = orderData.SecondInstalmentDate.ToString("dd/MM/yyyy").Substring(0, 10);
                    
                }

                if (orderData.PaymentBalanceDueDate > DateTime.MinValue)
                {
                    litBalanceDueDate.Text = orderData.PaymentBalanceDueDate.ToString("dd/MM/yyyy").Substring(0, 10);   
                }

                litPreNights.Text = orderData.PreNightsDetails?.DateRangeDisplay;
                litPostNights.Text = orderData.PostNightsDetails?.DateRangeDisplay;

                litPromoCode.Text = orderData.PromoCode;
                
                var adminSummaryProvider = new AdminSummaryBreakdownProvider();

                var prePostNightsPrices = new PrePostNightsPriceContainer(orderData.TwinPreNightBasePrice, orderData.TwinPostNightBasePrice, 
                    orderData.SinglePreNightBasePrice, orderData.SinglePostNightBasePrice);
                
                litSummary.Text = adminSummaryProvider.GetOrderSummaryHtml(order, currentCurrencyPricing, prePostNightsPrices); 
                //order.BookingBreakdownAdminHtml(new CurrentCurrencyPricing("AUD", 1));
                /*var addedServicesHtml = order.BookingAddedServicesBreakdownEmailHtml(true);

                if (!addedServicesHtml.IsNullOrEmpty())
                {
                    plcAddedServices.Visible = true;
                    litAddedServices.Text = addedServicesHtml;
                }*/
                SetCustomer(order, order.OrderNote);

            } 
        }

        private PassengerInfo GetSecondPassenger(int customerId)
        {
            return PassengerInfoProvider.GetPassengers()
                .WhereEquals(nameof(PassengerInfo.PassengerCustomerID), customerId).FirstOrDefault();
        }
        private void SetCustomer(OrderInfo order, string customerComments)
        {
            var customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
            
            
            if (customer != null)
            {
                var customerDateOfBirth = customer.GetDateTimeValue("CustomerDateOfBirth", DateTime.MinValue);

                var passenger = GetSecondPassenger(customer.CustomerID);
                litCustomerTitle.Text = customer.GetStringValue("CustomerTitle", string.Empty);
                litCustomerFirstName.Text = customer.CustomerFirstName;
                litCustomerMiddleName.Text = customer.GetStringValue("CustomerMiddleName", string.Empty);
                litCustomerLastName.Text = customer.CustomerLastName;
                litCustomerEmail.Text = customer.CustomerEmail;
                litCustomerPhone.Text = customer.CustomerPhone;
                litCustomerState.Text = customer.GetStringValue("CustomerState", string.Empty);
                litCustomerComments.Text = customerComments;
                litCustomerDateOfBirth.Text = customerDateOfBirth.ToString("dd MMM yyyy");

                if (customer.GetStringValue("CustomerAgencyName", string.Empty) != string.Empty)
                {
                    litHeading.Text = "Travel Agent Booking";
                    plcAgent.Visible = true;
                    litAgentTradingName.Text = customer.GetStringValue("CustomerAgencyName", string.Empty);
                    litAgentAdvisorName.Text = customer.GetStringValue("CustomerAgentName", string.Empty);
                    litAgentEmail.Text = customer.GetStringValue("CustomerAgentEmail", string.Empty);
                    litAgentPhone.Text = customer.GetStringValue("CustomerAgentPhone", string.Empty);
                    litAgentPostcode.Text = customer.GetStringValue("CustomerAgencyPostcode", string.Empty);
                    litAgencyComments.Text = customer.GetStringValue("CustomerAgentComment", string.Empty);

                    plcAgentCustomerDetails.Visible = true;
                    litAgentCustomerTitle.Text = customer.GetStringValue("CustomerTitle", string.Empty);
                    litAgentCustomerFirstName.Text = customer.CustomerFirstName;
                    litAgentCustomerMiddleName.Text = customer.GetStringValue("CustomerMiddleName", string.Empty);
                    litAgentCustomerLastName.Text = customer.CustomerLastName;
                    litAgentCustomerEmail.Text = customer.CustomerEmail;
                    litAgentCustomerPhone.Text = customer.CustomerPhone;

                    if (customerDateOfBirth > DateTime.MinValue)
                    {
                        litAgentCustomerDateOfBirth.Text = customerDateOfBirth.ToString("dd MMM yyyy");
                    }
                    litAgentCustomerState.Text = customer.GetStringValue("CustomerState", string.Empty);
                    litAgentCustomerComments.Text = customerComments;
                    if (passenger != null)
                    {
                        litAgentSecondPassengerTitle.Text = passenger.PassengerTitle;
                        litAgentSecondPassengerFirstName.Text = passenger.PassengerFirstName;
                        litAgentSecondPassengerLastName.Text = passenger.PassengerLastName;
                        litAgentSecondPassengerMiddleName.Text = passenger.PassengerMiddleName;

                        if (passenger.PassengerDateOfBirth > DateTime.MinValue)
                        {
                            litAgentSecondPassengerDateOfBirth.Text = passenger.PassengerDateOfBirth.ToString("dd MMM yyyy");
                        }
                        
                        plcAgentSecondPassenger.Visible = true;
                    }
                }
                else
                {
                    plcCustomer.Visible = true;
                    if (passenger != null)
                    {
                        litSecondPassengerTitle.Text = passenger.PassengerTitle;
                        litSecondPassengerFirstName.Text = passenger.PassengerFirstName;
                        litSecondPassengerLastName.Text = passenger.PassengerLastName;
                        litSecondPassengerMiddleName.Text = passenger.PassengerMiddleName;
                        if (passenger.PassengerDateOfBirth > DateTime.MinValue)
                        {
                            litSecondPassengerDateOfBirth.Text = passenger.PassengerDateOfBirth.ToString("dd MMM yyyy");
                        }

                        plcSecondPassenger.Visible = true;
                    }
                }
            }
        }

        protected string GetRoomTypeLabel(int roomNumber, int optionTypeId)
        {
            switch (optionTypeId)
            {
                case 1:
                    var roomTypes = order.GetTwinRoomsType();

                    if (roomTypes.IsNullOrEmpty())
                    {
                        return "Twin share room";
                    }
                    var arr = roomTypes.Split(',');

                    if (roomNumber <= arr.Length)
                    {
                        return arr[roomNumber - 1];
                    }
                    return "Twin share room";
                case 2:
                    return "Single room";
            }

            return string.Empty;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var order = OrderInfoProvider.GetOrderInfo(hidOrderID.Value.ToInteger(0));
            if (order != null)
            {
                order.SetValue("OrderTopDogNumber", txtTopDog.Text);
                order.Update();
                litMessage.Text = "Changes has been saved.";
            }
        }
        
        protected void btnSendRefund_Click(object sender, EventArgs e)
        {
            var order = OrderInfoProvider.GetOrderInfo(hidOrderID.Value.ToInteger(0));
            if (order != null)
            {
                var settingsRepository = ServiceLocator.Current.GetInstance<IETGSettingsService>();
                var settings = settingsRepository.GetSettings();
                var refundProtectService = ServiceLocator.Current.GetInstance<IRefundProtectService>();

                var result = refundProtectService.SendApplyRefundEmail(order, settings.RefundProtectMemberID);

                if (result.Success)
                {
                    litMessage.Text = "Apply refund email has been sent";   
                }
                else
                {
                    litMessage.Text = $"<span color=\"red\">{result.Message}</span>";   

                }
            }
        }
    }
}