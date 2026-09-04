using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Classes.Info;

namespace ETG.Module.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="TourTypeInfo"/> management.
    /// </summary>
    public partial class TourTypeInfoProvider : AbstractInfoProvider<TourTypeInfo, TourTypeInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="TourTypeInfoProvider"/>.
        /// </summary>
        public TourTypeInfoProvider()
            : base(TourTypeInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="TourTypeInfo"/> objects.
        /// </summary>
        public static ObjectQuery<TourTypeInfo> GetTourTypes()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="TourTypeInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="TourTypeInfo"/> ID.</param>
        public static TourTypeInfo GetTourTypeInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="TourTypeInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="TourTypeInfo"/> name.</param>
        public static TourTypeInfo GetTourTypeInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="TourTypeInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="TourTypeInfo"/> to be set.</param>
        public static void SetTourTypeInfo(TourTypeInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="TourTypeInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="TourTypeInfo"/> to be deleted.</param>
        public static void DeleteTourTypeInfo(TourTypeInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="TourTypeInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="TourTypeInfo"/> ID.</param>
        public static void DeleteTourTypeInfo(int id)
        {
            TourTypeInfo infoObj = GetTourTypeInfo(id);
            DeleteTourTypeInfo(infoObj);
        }
    }
}