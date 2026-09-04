using CMS.DataEngine;
using ETG.Booking.Pricing.Classes.Info;

namespace ETG.Booking.Pricing.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="BookingPriceInfo"/> management.
    /// </summary>
    public partial class BookingPriceInfoProvider : AbstractInfoProvider<BookingPriceInfo, BookingPriceInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="BookingPriceInfoProvider"/>.
        /// </summary>
        public BookingPriceInfoProvider()
            : base(BookingPriceInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="BookingPriceInfo"/> objects.
        /// </summary>
        public static ObjectQuery<BookingPriceInfo> GetBookingPrices()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="BookingPriceInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingPriceInfo"/> ID.</param>
        public static BookingPriceInfo GetBookingPriceInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="BookingPriceInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingPriceInfo"/> to be set.</param>
        public static void SetBookingPriceInfo(BookingPriceInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="BookingPriceInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingPriceInfo"/> to be deleted.</param>
        public static void DeleteBookingPriceInfo(BookingPriceInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="BookingPriceInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingPriceInfo"/> ID.</param>
        public static void DeleteBookingPriceInfo(int id)
        {
            BookingPriceInfo infoObj = GetBookingPriceInfo(id);
            DeleteBookingPriceInfo(infoObj);
        }
    }
}