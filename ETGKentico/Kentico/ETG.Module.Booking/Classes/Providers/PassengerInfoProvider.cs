using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="PassengerInfo"/> management.
    /// </summary>
    public partial class PassengerInfoProvider : AbstractInfoProvider<PassengerInfo, PassengerInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="PassengerInfoProvider"/>.
        /// </summary>
        public PassengerInfoProvider()
            : base(PassengerInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="PassengerInfo"/> objects.
        /// </summary>
        public static ObjectQuery<PassengerInfo> GetPassengers()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="PassengerInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="PassengerInfo"/> ID.</param>
        public static PassengerInfo GetPassengerInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="PassengerInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="PassengerInfo"/> to be set.</param>
        public static void SetPassengerInfo(PassengerInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="PassengerInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="PassengerInfo"/> to be deleted.</param>
        public static void DeletePassengerInfo(PassengerInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="PassengerInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="PassengerInfo"/> ID.</param>
        public static void DeletePassengerInfo(int id)
        {
            PassengerInfo infoObj = GetPassengerInfo(id);
            DeletePassengerInfo(infoObj);
        }
    }
}