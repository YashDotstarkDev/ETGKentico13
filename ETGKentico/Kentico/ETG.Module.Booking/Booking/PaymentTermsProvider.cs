using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ETG.Booking.Pricing.Enums;
using ETG.Booking.Pricing.Services;
using ETG.Data.Extensions;
using ETG.Data.Repositories.Pages;
using ETG.Data.Tour;

namespace ETG.Module.Booking.Booking
{
    public class PaymentTermsProvider : IPaymentTermsProvider
    {
        private readonly CurrentCurrencyPricing _currentCurrencyPricing;
        private readonly IBookingCheckoutPageContentsRepository _bookingCheckoutPageContentsRepository;
        public PaymentTermsProvider(IBookingCheckoutPageContentsRepository bookingCheckoutPageContentsRepository,
            ICurrencyService currencyService)
        {
            _bookingCheckoutPageContentsRepository = bookingCheckoutPageContentsRepository;
            _currentCurrencyPricing = new CurrentCurrencyPricing(currencyService);
        }
        
        //BookingSummary has been converted
        public string GetBookingPaymentTerms(CartBookingSummary bookingSummary, string tourPaymentTerms = "")
        {
            var pageContents = _bookingCheckoutPageContentsRepository.GetBookingCheckoutPageContents();

            if (bookingSummary == null || pageContents == null)
            {
                return string.Empty;
            }

            var termsCopy = string.Empty;

            if (!tourPaymentTerms.IsNullOrEmpty())
            {
                termsCopy = tourPaymentTerms;
            }

            if ((bookingSummary.IsOnSaleNow && bookingSummary.OnSaleFullPaymentRequired)
                || bookingSummary.Promotion != null)
            {
                termsCopy = pageContents.PaymentTermsCopyOnSaleNow;
            }

            if (bookingSummary.Due == null)
            {
                termsCopy = pageContents.PaymentTermsFullPayment;
            }

            if (termsCopy.IsNullOrEmpty())
            {
                termsCopy = pageContents.PaymentTermsCopyWithPeaceOfMind;
            }
            
            var hasSecondInstalment =
                bookingSummary.SecondInstalmentPrice > 0 && bookingSummary.PaymentInstalmentDate > DateTime.MinValue &&
            bookingSummary.PaymentBalanceDueDate > bookingSummary.PaymentInstalmentDate;
        
            var additionalInfo = new StringBuilder();
            if (hasSecondInstalment)
            {
                additionalInfo.AppendLine($@"<p><strong>Second deposit instalment</strong> of {bookingSummary.SecondInstalmentPrice.FormatPrice(true, _currentCurrencyPricing.CurrentCurrency)} is due on {bookingSummary.PaymentInstalmentDate:d/M/yyyy}</p>");
            }

            additionalInfo.AppendLine(
                bookingSummary.PaymentBalanceDueDate <= DateTime.Today
                    ? $@"<p>We will check package availability and be in touch with you. Full payment will be required prior to confirming your booking</p>"
                    : $@"<p><strong>Final Balance will be due:</strong> {bookingSummary.PaymentBalanceDueDate:d/M/yyyy}</p>");

            return termsCopy.Replace("<p>[instalmenttext]</p>", additionalInfo.ToString());

        }
    }
}
