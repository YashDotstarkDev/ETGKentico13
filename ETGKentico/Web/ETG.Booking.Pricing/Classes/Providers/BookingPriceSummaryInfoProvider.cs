using CMS.DataEngine;
using ETG.Booking.Pricing.Classes.Info;

namespace ETG.Booking.Pricing.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="BookingPriceSummaryInfo"/> management.
    /// </summary>
    public partial class BookingPriceSummaryInfoProvider : AbstractInfoProvider<BookingPriceSummaryInfo, BookingPriceSummaryInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="BookingPriceSummaryInfoProvider"/>.
        /// </summary>
        public BookingPriceSummaryInfoProvider()
            : base(BookingPriceSummaryInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="BookingPriceSummaryInfo"/> objects.
        /// </summary>
        public static ObjectQuery<BookingPriceSummaryInfo> GetBookingPriceSummaries()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="BookingPriceSummaryInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingPriceSummaryInfo"/> ID.</param>
        public static BookingPriceSummaryInfo GetBookingPriceSummaryInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="BookingPriceSummaryInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingPriceSummaryInfo"/> to be set.</param>
        public static void SetBookingPriceSummaryInfo(BookingPriceSummaryInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="BookingPriceSummaryInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingPriceSummaryInfo"/> to be deleted.</param>
        public static void DeleteBookingPriceSummaryInfo(BookingPriceSummaryInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="BookingPriceSummaryInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingPriceSummaryInfo"/> ID.</param>
        public static void DeleteBookingPriceSummaryInfo(int id)
        {
            BookingPriceSummaryInfo infoObj = GetBookingPriceSummaryInfo(id);
            DeleteBookingPriceSummaryInfo(infoObj);
        }
    }
}