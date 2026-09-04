using CMS.DataEngine;
using ETG.Booking.Pricing.Classes.Info;

namespace ETG.Booking.Pricing.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="BookingRoomOptionInfo"/> management.
    /// </summary>
    public partial class BookingRoomOptionInfoProvider : AbstractInfoProvider<BookingRoomOptionInfo, BookingRoomOptionInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="BookingRoomOptionInfoProvider"/>.
        /// </summary>
        public BookingRoomOptionInfoProvider()
            : base(BookingRoomOptionInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="BookingRoomOptionInfo"/> objects.
        /// </summary>
        public static ObjectQuery<BookingRoomOptionInfo> GetBookingRoomOptions()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="BookingRoomOptionInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingRoomOptionInfo"/> ID.</param>
        public static BookingRoomOptionInfo GetBookingRoomOptionInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="BookingRoomOptionInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingRoomOptionInfo"/> to be set.</param>
        public static void SetBookingRoomOptionInfo(BookingRoomOptionInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="BookingRoomOptionInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingRoomOptionInfo"/> to be deleted.</param>
        public static void DeleteBookingRoomOptionInfo(BookingRoomOptionInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="BookingRoomOptionInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingRoomOptionInfo"/> ID.</param>
        public static void DeleteBookingRoomOptionInfo(int id)
        {
            BookingRoomOptionInfo infoObj = GetBookingRoomOptionInfo(id);
            DeleteBookingRoomOptionInfo(infoObj);
        }
    }
}