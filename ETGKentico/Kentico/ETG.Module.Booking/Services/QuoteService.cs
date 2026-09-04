using System;
using CMS.Ecommerce;
using ETG.Core.Encryption;
using ETG.Core.Services;
using ETG.Data.Settings;
using ETG.Data.Settings.Models;
using ETG.Data.Tour.Services;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;
using CMS.Helpers;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Tour;
using ETG.Module.Booking.Constants;
using ETG.Module.Booking.ECommerce;

namespace ETG.Module.Booking.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IEmailService _emailService;
        private readonly ITourService _tourService;
        private readonly ETGSettings _settings;
        private readonly IQuoteAddedServiceProvider _quoteAddedServiceProvider;
        public QuoteService(IEmailService emailService, ITourService tourService, 
            IETGSettingsService etgSettingsService, IQuoteAddedServiceProvider quoteAddedServiceProvider)
        {
            _emailService = emailService;
            _tourService = tourService;
            _settings = etgSettingsService.GetSettings();
            _quoteAddedServiceProvider = quoteAddedServiceProvider;
        }
        
        public void SendQuoteConfirmationEmail(BookingQuoteInfo bookingQuote, string additionalComments = "", string toEmail = "")
        {
            if (bookingQuote == null)
            {
                return;
            }
            var subject = $"Entire Travel Group - Website Client Quote {bookingQuote.BookingQuoteID}";
            var parameters = new Dictionary<string, string>();
            
            var customer = CustomerInfoProvider.GetCustomerInfo(bookingQuote.BookingQuoteCustomerID);
            var firstName = "";
            if (toEmail.IsNullOrEmpty())
            {
                toEmail = customer.CustomerEmail;
                firstName = customer.CustomerFirstName;
                if (customer.GetStringValue("CustomerAgentEmail", string.Empty) != string.Empty)
                {
                    firstName = customer.GetStringValue("CustomerAgentName", string.Empty);
                    toEmail = customer.GetStringValue("CustomerAgentEmail", string.Empty);
                }

                var agentTradingName = customer.GetStringValue("CustomerAgencyName", string.Empty);

                if (!agentTradingName.IsNullOrEmpty() && agentTradingName.Equals(BookingConstants.ETG_TradingName,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    toEmail = customer.CustomerEmail;
                }
            }

            var tour = _tourService.GetTourByTourCode(bookingQuote.BookingQuoteTourCode);

            if (tour == null)
            {
                return;
            }

            var quoteData = bookingQuote.GetDetails();
            if (quoteData == null)
            {
                return;
            }

            if (additionalComments.IsNullOrEmpty())
            {
                //additionalComments = "We are delighted you have selected one of our holiday packages for your clients.";
            }
            else
            {
                additionalComments = additionalComments.Replace("\n", "<br>");
            }
            parameters.Add("emailcontent", additionalComments);
            parameters.Add("quotenumber", bookingQuote.BookingQuoteID.ToString());
            parameters.Add("quotedatetime", bookingQuote.BookingQuoteCreated.ToString("dddd, dd MMM yyyy h:mm tt"));
            parameters.Add("agentname", firstName);
            parameters.Add("tourcode", bookingQuote.BookingQuoteTourCode);
            parameters.Add("tourname", quoteData.TourName);
            parameters.Add("validdays", _settings.BookingQuoteValidityDays.ToString());
            parameters.Add("destination", tour.TourSummaryInfo.PrimaryCountryName);
            parameters.Add("traveldeparts", tour.TourSummaryInfo.TourDepartsFromInAustralia );
            parameters.Add("travelends", tour.TourSummaryInfo.TourTravelEnds);
            parameters.Add("startdate", quoteData.DepartureDate.ToString("dddd, dd MMM yyyy"));
            parameters.Add("duration", $"{tour.TourSummaryInfo.NoOfNights + 1} days" );
            parameters.Add("experience", string.Join(", ", tour.TourSummaryInfo.Experiences.Select(a=>a.Name)));
            parameters.Add("travelstyle", string.Join(", ", tour.TourSummaryInfo.TourTypes.Select(a=>a.Name)));
            parameters.Add("passengername", $"{customer.CustomerFirstName} {customer.CustomerLastName}");

            var currentPricing = new CurrentCurrencyPricing(bookingQuote.BookingQuoteCurrency,
                bookingQuote.BookingQuoteConversionRate);
            
            var addedServicesTotalPrice = _quoteAddedServiceProvider.GetAddedServices(bookingQuote.BookingQuoteID).Select(a=>a.TotalPrice).Sum();

            var price = quoteData.Quote.BookingQuoteAgentPrice;
            
            if (price == 0)
            {

                price = currentPricing.ConvertAUDToCurrentCurrency(quoteData.Quote.BookingQuoteTotalPrice);
            }

            price += addedServicesTotalPrice;

            parameters.Add("totalprice", price.ToString($"{currentPricing.CurrentCurrency} #,###"));   
            parameters.Add("agentsellprice", bookingQuote.BookingQuoteAgentPrice.ToString($"{currentPricing.CurrentCurrency} #,###"));   
            parameters.Add("agentnetprice", bookingQuote.BookingQuoteNetPrice.ToString($"{currentPricing.CurrentCurrency} #,###"));

            var commission = bookingQuote.BookingQuoteAgentPrice - bookingQuote.BookingQuoteNetPrice;

            if (commission < 0)
            {
                commission = 0;
            }
            
            parameters.Add("agentcommission", commission.ToString($"{currentPricing.CurrentCurrency} #,###"));   
            parameters.Add("downloadparameter", HttpContext.Current.Server.UrlEncode(ETGEncryptor.EncryptString($"{bookingQuote.BookingQuoteID}")));
            
            _emailService.SendEmail("QuoteConfirmation", toEmail, parameters, true, string.Empty, string.Empty, string.Empty, subject);

        }

        public void SendQuoteNotificationEmail(BookingQuoteInfo bookingQuote, string additionalComments = "", bool isAdmin = false)
        {
            var subject = $"Entire Travel Group - Website Client Quote {bookingQuote.BookingQuoteID}";
            var notificationEmail = _settings.BookNowNotificationEmail;
            var parameters = new Dictionary<string, string>();
            var quoteDetails = bookingQuote.GetDetails();
            var currentCurrencyPricing = new CurrentCurrencyPricing(quoteDetails.Currency, quoteDetails.ConversionRate);

            
            parameters.Add("quoteid", bookingQuote.BookingQuoteID.ToString());
            parameters.Add("quotedate", bookingQuote.BookingQuoteLastModified.ToString("dddd, dd MMMM yyyy h:mm tt"));
            parameters.Add("packagecode", bookingQuote.BookingQuoteTourCode);
            parameters.Add("packagecountry",quoteDetails.TourDestination);
            parameters.Add("packagename", quoteDetails.TourName);
            parameters.Add("packageurl", $"http{(HttpContext.Current.Request.IsSecureConnection ? "s" : "")}://{HttpContext.Current.Request.Url.Host}{quoteDetails.TourUrl}");
            parameters.Add("departuredate", quoteDetails.DepartureDate.ToString("dddd, dd MMM yyyy"));
            parameters.Add("summary", bookingQuote.QuoteBreakdownEmailHtml(currentCurrencyPricing, isAdmin));
            parameters.Add("customer", bookingQuote.QuoteCustomerInfoEmailHtml());
            parameters.Add("comments", additionalComments);
            parameters.Add("prepostnightsdetails", bookingQuote.QuotePrePostNightsEmailHtml(isAdmin));
            
            _emailService.SendEmail("QuoteNotification", notificationEmail, parameters, true, string.Empty, string.Empty, string.Empty, subject);

        }
    }
}