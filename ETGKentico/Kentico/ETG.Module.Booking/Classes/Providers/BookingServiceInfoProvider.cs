using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="BookingServiceInfo"/> management.
    /// </summary>
    public partial class BookingServiceInfoProvider : AbstractInfoProvider<BookingServiceInfo, BookingServiceInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="BookingServiceInfoProvider"/>.
        /// </summary>
        public BookingServiceInfoProvider()
            : base(BookingServiceInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="BookingServiceInfo"/> objects.
        /// </summary>
        public static ObjectQuery<BookingServiceInfo> GetBookingServices()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="BookingServiceInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingServiceInfo"/> ID.</param>
        public static BookingServiceInfo GetBookingServiceInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="BookingServiceInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingServiceInfo"/> to be set.</param>
        public static void SetBookingServiceInfo(BookingServiceInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="BookingServiceInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingServiceInfo"/> to be deleted.</param>
        public static void DeleteBookingServiceInfo(BookingServiceInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="BookingServiceInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingServiceInfo"/> ID.</param>
        public static void DeleteBookingServiceInfo(int id)
        {
            BookingServiceInfo infoObj = GetBookingServiceInfo(id);
            DeleteBookingServiceInfo(infoObj);
        }
    }
}