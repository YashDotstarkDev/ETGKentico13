using ETG.Booking.Pricing.Classes.Info;
using ETG.Booking.Pricing.Models;
using System;
using System.Collections.Generic;

namespace ETG.Booking.Pricing.Repositories
{
    public interface IBookingPriceRepository
    {
        List<DateBookingPrice> GetDepartureBookingPrices(string tourCode, DateTime? from, DateTime? to, bool includePast = false);
        DateBookingPrice GetDepartureBookingPrice(string tourCode, DateTime departureDate);
        DepartureDateBookingOptions GetBookingOptions(string tourCode, DateTime departureDate);
        List<BookingOption> GetBookingOptions(List<Guid> roomOptionsGuids);

        int GetLowestPriceFromBookNowPricing(string tourCode);
        List<KeyValuePair<string, int>> GetLowestPricesFromBookNowPricing(List<string> tourCodes);
    }
}
