using ETG.Data.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Cache;
using ETG.Core.Kentico;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Tour.Services;

namespace ETG.Data.Repositories.Pages
{
    public class BookingCheckoutPageContentsRepository : IBookingCheckoutPageContentsRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ISiteContext _siteContext;
        public BookingCheckoutPageContentsRepository(ICacheProvider cacheProvider, ISiteContext siteContext)
        {
            _cacheProvider = cacheProvider;
            _siteContext = siteContext;
        }

        private BookingCheckoutPageContentsModel GetBookingCheckoutPageInternal()
        {
            
            return BookingCheckoutPageProvider.GetBookingCheckoutPages().OnCurrentSite().Select(page => new BookingCheckoutPageContentsModel
            {
                ThankYouPageCopy = page.ThankyouPageCopy,
                OtherPaymentThankYouPageCopy = page.OtherPaymentThankYouPageCopy,
                QuoteThankYouPageCopy = page.QuoteThankyouPageCopy,
                CopyBelowCheckoutForm = page.CopyBelowCheckoutForm,
                PaymentTermsCopyOnSaleNow = page.PaymentTermsOnSaleNow,
                PaymentTermsCopyWithPeaceOfMind = page.PaymentTermsWithPeaceOfMind,
                PaymentTermsCopyWithPeaceOfMindAndFlex = page.PaymentTermsWithPeaceOfMindAndFlex,
                PaymentTermsFullPayment =  page.PaymentTermsFullPayment,
                DueCopyWithPeaceOfMind = page.DueCopyWithPeaceOfMind,
                DueCopyWithoutPeaceOfMind = page.DueCopyWithoutPeaceOfMind,
                DueCopyWithPeaceOfMindAndFlex = page.DueCopyWithPeaceOfMindAndFlex,
                QuoteExpiredMessage = page.QuoteExpiredInvalidMessage,
                QuoteExpiredEntireFlexMessage = page.QuoteEntireFlexInvalidMessage,
                QuoteTourRemovedMessage = page.QuoteTourRemovedInvalidMessage,
                QuoteGenericMessage = page.QuoteGenericInvalidMessage

            }).FirstOrDefault();
        }
        public BookingCheckoutPageContentsModel GetBookingCheckoutPageContents()
        {
            return _cacheProvider.GetCached(() => GetBookingCheckoutPageInternal(),
                "getbookingcheckoutpage",
                new GenericDependencyBuilder<BookingCheckoutPage>(_siteContext.SiteName).DependsOnAllNodesOfPageType());
        }
    }
}
