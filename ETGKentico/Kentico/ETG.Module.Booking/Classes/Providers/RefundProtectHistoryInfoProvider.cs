using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="RefundProtectHistoryInfo"/> management.
    /// </summary>
    public partial class RefundProtectHistoryInfoProvider : AbstractInfoProvider<RefundProtectHistoryInfo, RefundProtectHistoryInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="RefundProtectHistoryInfoProvider"/>.
        /// </summary>
        public RefundProtectHistoryInfoProvider()
            : base(RefundProtectHistoryInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="RefundProtectHistoryInfo"/> objects.
        /// </summary>
        public static ObjectQuery<RefundProtectHistoryInfo> GetRefundProtectHistory()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="RefundProtectHistoryInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="RefundProtectHistoryInfo"/> ID.</param>
        public static RefundProtectHistoryInfo GetRefundProtectHistoryInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="RefundProtectHistoryInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="RefundProtectHistoryInfo"/> to be set.</param>
        public static void SetRefundProtectHistoryInfo(RefundProtectHistoryInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="RefundProtectHistoryInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="RefundProtectHistoryInfo"/> to be deleted.</param>
        public static void DeleteRefundProtectHistoryInfo(RefundProtectHistoryInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="RefundProtectHistoryInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="RefundProtectHistoryInfo"/> ID.</param>
        public static void DeleteRefundProtectHistoryInfo(int id)
        {
            RefundProtectHistoryInfo infoObj = GetRefundProtectHistoryInfo(id);
            DeleteRefundProtectHistoryInfo(infoObj);
        }
    }
}