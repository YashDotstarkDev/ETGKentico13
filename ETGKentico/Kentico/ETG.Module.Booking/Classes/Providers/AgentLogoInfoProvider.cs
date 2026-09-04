using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="AgentLogoInfo"/> management.
    /// </summary>
    public partial class AgentLogoInfoProvider : AbstractInfoProvider<AgentLogoInfo, AgentLogoInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="AgentLogoInfoProvider"/>.
        /// </summary>
        public AgentLogoInfoProvider()
            : base(AgentLogoInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="AgentLogoInfo"/> objects.
        /// </summary>
        public static ObjectQuery<AgentLogoInfo> GetAgentLogoes()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="AgentLogoInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="AgentLogoInfo"/> ID.</param>
        public static AgentLogoInfo GetAgentLogoInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="AgentLogoInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="AgentLogoInfo"/> name.</param>
        public static AgentLogoInfo GetAgentLogoInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="AgentLogoInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="AgentLogoInfo"/> to be set.</param>
        public static void SetAgentLogoInfo(AgentLogoInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="AgentLogoInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="AgentLogoInfo"/> to be deleted.</param>
        public static void DeleteAgentLogoInfo(AgentLogoInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="AgentLogoInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="AgentLogoInfo"/> ID.</param>
        public static void DeleteAgentLogoInfo(int id)
        {
            AgentLogoInfo infoObj = GetAgentLogoInfo(id);
            DeleteAgentLogoInfo(infoObj);
        }
    }
}