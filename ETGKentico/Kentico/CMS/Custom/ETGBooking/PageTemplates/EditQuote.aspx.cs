using Castle.Core.Internal;
using CMS.Helpers;
using ETG.Core.Extensions;
using ETG.Data.Extensions;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.Extensions;
using System;
using CMS.Base;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.UIControls;
using ETG.Module.Booking.Classes.Info;
using CMS.Modules;
using CommonServiceLocator;
using ETG.Data.Tour;
using ETG.Module.Booking.Admin;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Services;

namespace CMSApp.Custom.ETGBooking.PageTemplates
{
    [EditedObject(BookingQuoteInfo.OBJECT_TYPE, "BookingQuoteId")]
    public partial class EditQuote : CMSEcommercePage
    {
        private BookingQuoteInfo quote;
        protected string GetRoomTypeLabel(int roomNumber, int optionTypeId)
        {
            switch (optionTypeId)
            {
                case 1:
                    var roomTypes = quote.GetTwinRoomsType();

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
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                return;
            }
            var id = QueryHelper.GetInteger("objectid", 0);
            quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(id);

            if (quote.HasExpired())
            {
                plcEmail.Visible = false;
            }
            
            txtTopDog.Text = quote.BookingQuoteTopdogNumber;
            var quoteData = quote.GetDetails();
            litQuoteCreated.Text = quote.BookingQuoteCreated.ToString("dd/MM/yyyy HH:mm");
            litValidDays.Text = quote.BookingQuoteValidDays.ToString();
            litQuoteId.Text = quote.BookingQuoteID.ToString();
            hidQuoteID.Value = quote.BookingQuoteID.ToString();
            litTourCode.Text = quote.BookingQuoteTourCode;
            litTourName.Text = quoteData.TourName;
            litCountry.Text = $"{quoteData.TourDestination}";
            litOnSaleNow.Text = quoteData.IsOnSaleNow.YesOrNo();
            litDate.Text = quoteData.DepartureDate.ToString("dd/MM/yyyy").Substring(0, 10);
            litDaysFromDepartureDate.Text = quoteData.DepartureDate.Subtract(quoteData.Quote.BookingQuoteLastModified.Date).Days.ToString();
            litDoubleRooms.Text = quoteData.TwinShareRoomCount.ToString();
            litSingleRooms.Text = quoteData.SingleRoomCount.ToString();
            txtEmailComments.Text = quote.BookingQuoteLastEmail;
            litTwinShareRoomsType.Text = quote.GetTwinRoomsTypeHtml();
            if (quoteData.RoomOptions != null && !quoteData.RoomOptions.RoomOptions.IsNullOrEmpty())
            {
                repRoomOptions.DataSource = quoteData.RoomOptions.RoomOptions;
                repRoomOptions.DataBind();
            }

            if (quoteData.Extras != null)
            {
                litExtraSelectNow.Text = quoteData.Extras.SelectExtrasNow.YesOrNo("Maybe Later", true);
                repExtras.DataSource = quoteData.Extras.Extras;
                repExtras.DataBind();
            }

            if (quoteData.FreedomOfChoices != null)
            {
                litFocSelectNow.Text = quoteData.FreedomOfChoices.SelectNow.YesOrNo("Select Later", true);
                repFreedomOfChoice.DataSource = quoteData.FreedomOfChoices.FreedomOfChoiceItems;
                repFreedomOfChoice.DataBind();
            }
            
            litInsuranceAssistance.Text = quoteData.TravelInsuranceAssistance.YesOrNo();
            litFareAssistance.Text = quoteData.InternationalAirfareAssistance.YesOrNo();

            var adminSummaryProvider = new AdminSummaryBreakdownProvider();
            var currentCurrencyPricing =  new CurrentCurrencyPricing(quoteData.Currency,
                quoteData.ConversionRate);

            if (!currentCurrencyPricing.CurrentCurrencyIsAUD)
            {
                pnlExchangeRate.Visible = true;
                litExchangeRate.Text = $"1 AUD = {quoteData.Currency} {quoteData.ConversionRate:0.00}";
            }
            
            litPreNights.Text = quoteData.PreNightsDetails?.DateRangeDisplay;
            litPostNights.Text = quoteData.PostNightsDetails?.DateRangeDisplay;
            
            var prePostNightsPrices = new PrePostNightsPriceContainer(quoteData.TwinPreNightBasePrice, quoteData.TwinPreNightBasePrice, 
                quoteData.SinglePreNightBasePrice, quoteData.SinglePostNightBasePrice);
            
            litSummary.Text = adminSummaryProvider.GetQuoteSummaryHtml(quote, currentCurrencyPricing, prePostNightsPrices);

            /*var addedServicesHtml = quote.QuoteAddedServiceBreakdownEmailHtml(true);

            if (!addedServicesHtml.IsNullOrEmpty())
            {
                plcAddedServices.Visible = true;
                litAddedServices.Text = addedServicesHtml;
            }*/

            //litGrandTotal.Text = quote.QuoteGrandTotal(true).ToString("$#,###.00");
            SetCustomer(quote);
        }
        protected string GetRoomTypeLabel(int optionTypeId)
        {
            switch (optionTypeId)
            {
                case 1:
                    return "Twin share room";
                case 2:
                    return "Single room";
            }

            return string.Empty;
        }

        private void SetCustomer(BookingQuoteInfo quote)
        {
            var customer = CustomerInfoProvider.GetCustomerInfo(quote.BookingQuoteCustomerID);

            if (customer != null)
            {

                plcAgent.Visible = true;
                litAgentTradingName.Text = customer.GetStringValue("CustomerAgencyName", string.Empty);
                litAgentAdvisorName.Text = customer.GetStringValue("CustomerAgentName", string.Empty);
                txtAdvisorEmail.Text = customer.GetStringValue("CustomerAgentEmail", string.Empty);
                litAgentPhone.Text = customer.GetStringValue("CustomerAgentPhone", string.Empty);
                litAgentPostcode.Text = customer.GetStringValue("CustomerAgencyPostcode", string.Empty);
                litAgencyComments.Text = customer.GetStringValue("CustomerAgentComment", string.Empty);

                plcAgentCustomerDetails.Visible = true;
                litAgentCustomerFirstName.Text = customer.CustomerFirstName;
                litAgentCustomerLastName.Text = customer.CustomerLastName;
                txtLeadPassengerEmail.Text = customer.CustomerEmail;
                litAgentCustomerPhone.Text = customer.CustomerPhone;
                litAgentCustomerState.Text = customer.GetStringValue("CustomerState", string.Empty);
                litAgentCustomerComments.Text = quote.BookingQuoteCustomerComments?.Replace("\n", "<br>");

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(hidQuoteID.Value.ToInteger(0));
            if (quote != null)
            {
                using (var scope = new CMSTransactionScope())
                {
                    quote.BookingQuoteTopdogNumber = txtTopDog.Text;
                    if (!string.IsNullOrEmpty(txtEmailComments.Text.Trim())){
                        quote.BookingQuoteLastEmail = txtEmailComments.Text;
                    }
                    var customer = CustomerInfoProvider.GetCustomerInfo(quote.BookingQuoteCustomerID);

                    if (customer != null)
                    {
                        customer.CustomerEmail = txtLeadPassengerEmail.Text;
                        customer.SetValue("CustomerAgentEmail", txtAdvisorEmail.Text);
                        customer.Update();
                    }

                    quote.Update();
                    scope.Commit();
                    litMessage.Text = "Changes has been saved.";
                }
            }
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            var quote = BookingQuoteInfoProvider.GetBookingQuoteInfo(hidQuoteID.Value.ToInteger(0));
            if (quote != null)
            {
                if (quote.HasExpired())
                {
                    litEmailMessage.Text = "<span style=\"color:red\">Cannot send an expired quote.</span>";
                    return;
                }
                var quoteService = ServiceLocator.Current.GetInstance<IQuoteService>();

                quoteService.SendQuoteNotificationEmail(quote, txtEmailComments.Text, true);
                quoteService.SendQuoteConfirmationEmail(quote, txtEmailComments.Text, txtLeadPassengerEmail.Text);
                litEmailMessage.Text = $"Quote Sent.";
            }
        }

    }
}