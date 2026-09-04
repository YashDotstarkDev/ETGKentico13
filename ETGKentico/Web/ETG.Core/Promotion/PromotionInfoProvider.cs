using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace ETG.Core.Promotion
{
    /// <summary>
    /// Class providing <see cref="PromotionInfo"/> management.
    /// </summary>
    public partial class PromotionInfoProvider : AbstractInfoProvider<PromotionInfo, PromotionInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="PromotionInfoProvider"/>.
        /// </summary>
        public PromotionInfoProvider()
            : base(PromotionInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="PromotionInfo"/> objects.
        /// </summary>
        public static ObjectQuery<PromotionInfo> GetPromotions()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="PromotionInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="PromotionInfo"/> ID.</param>
        public static PromotionInfo GetPromotionInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="PromotionInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="PromotionInfo"/> name.</param>
        public static PromotionInfo GetPromotionInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="PromotionInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="PromotionInfo"/> to be set.</param>
        public static void SetPromotionInfo(PromotionInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="PromotionInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="PromotionInfo"/> to be deleted.</param>
        public static void DeletePromotionInfo(PromotionInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="PromotionInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="PromotionInfo"/> ID.</param>
        public static void DeletePromotionInfo(int id)
        {
            PromotionInfo infoObj = GetPromotionInfo(id);
            DeletePromotionInfo(infoObj);
        }
    }
}