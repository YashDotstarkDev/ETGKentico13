using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Classes.Info;

namespace ETG.Module.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="RoleDestinationLinkInfo"/> management.
    /// </summary>
    public partial class RoleDestinationLinkInfoProvider : AbstractInfoProvider<RoleDestinationLinkInfo, RoleDestinationLinkInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="RoleDestinationLinkInfoProvider"/>.
        /// </summary>
        public RoleDestinationLinkInfoProvider()
            : base(RoleDestinationLinkInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="RoleDestinationLinkInfo"/> objects.
        /// </summary>
        public static ObjectQuery<RoleDestinationLinkInfo> GetRoleDestinationLinks()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="RoleDestinationLinkInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="RoleDestinationLinkInfo"/> ID.</param>
        public static RoleDestinationLinkInfo GetRoleDestinationLinkInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="RoleDestinationLinkInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="RoleDestinationLinkInfo"/> name.</param>
        public static RoleDestinationLinkInfo GetRoleDestinationLinkInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="RoleDestinationLinkInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="RoleDestinationLinkInfo"/> to be set.</param>
        public static void SetRoleDestinationLinkInfo(RoleDestinationLinkInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="RoleDestinationLinkInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="RoleDestinationLinkInfo"/> to be deleted.</param>
        public static void DeleteRoleDestinationLinkInfo(RoleDestinationLinkInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="RoleDestinationLinkInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="RoleDestinationLinkInfo"/> ID.</param>
        public static void DeleteRoleDestinationLinkInfo(int id)
        {
            RoleDestinationLinkInfo infoObj = GetRoleDestinationLinkInfo(id);
            DeleteRoleDestinationLinkInfo(infoObj);
        }
    }
}