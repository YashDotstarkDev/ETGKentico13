using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="BookingQuoteInfo"/> management.
    /// </summary>
    public partial class BookingQuoteInfoProvider : AbstractInfoProvider<BookingQuoteInfo, BookingQuoteInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="BookingQuoteInfoProvider"/>.
        /// </summary>
        public BookingQuoteInfoProvider()
            : base(BookingQuoteInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="BookingQuoteInfo"/> objects.
        /// </summary>
        public static ObjectQuery<BookingQuoteInfo> GetBookingQuotes()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="BookingQuoteInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingQuoteInfo"/> ID.</param>
        public static BookingQuoteInfo GetBookingQuoteInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="BookingQuoteInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingQuoteInfo"/> to be set.</param>
        public static void SetBookingQuoteInfo(BookingQuoteInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="BookingQuoteInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BookingQuoteInfo"/> to be deleted.</param>
        public static void DeleteBookingQuoteInfo(BookingQuoteInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="BookingQuoteInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BookingQuoteInfo"/> ID.</param>
        public static void DeleteBookingQuoteInfo(int id)
        {
            BookingQuoteInfo infoObj = GetBookingQuoteInfo(id);
            DeleteBookingQuoteInfo(infoObj);
        }
    }
}