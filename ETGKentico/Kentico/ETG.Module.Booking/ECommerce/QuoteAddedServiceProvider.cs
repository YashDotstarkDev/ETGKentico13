using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.Models.Cart;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Module.Booking.ECommerce
{
    public class QuoteAddedServiceProvider : IQuoteAddedServiceProvider
    {
        public List<ItemBreakdown> GetAddedServices(int quoteId)
        {
            return BookingServiceInfoProvider.GetBookingServices()
                .WhereEquals(nameof(BookingServiceInfo.BookingServiceQuoteID), quoteId)
                .Select(s=>new ItemBreakdown
                {
                    ItemLabel = s.BookingServiceName,
                    ItemSubLabel = s.BookingServiceDescription,
                    ItemDate = s.BookingServiceStartDate,
                    UnitPrice = s.BookingServiceUnitPrice,
                    Count = s.BookingServiceQuantity
                }).ToList();
        }
    }
}
