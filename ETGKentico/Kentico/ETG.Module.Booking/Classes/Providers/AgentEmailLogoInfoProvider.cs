using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="AgentEmailLogoInfo"/> management.
    /// </summary>
    public partial class AgentEmailLogoInfoProvider : AbstractInfoProvider<AgentEmailLogoInfo, AgentEmailLogoInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="AgentEmailLogoInfoProvider"/>.
        /// </summary>
        public AgentEmailLogoInfoProvider()
            : base(AgentEmailLogoInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="AgentEmailLogoInfo"/> objects.
        /// </summary>
        public static ObjectQuery<AgentEmailLogoInfo> GetAgentEmailLogoes()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="AgentEmailLogoInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="AgentEmailLogoInfo"/> ID.</param>
        public static AgentEmailLogoInfo GetAgentEmailLogoInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="AgentEmailLogoInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="AgentEmailLogoInfo"/> to be set.</param>
        public static void SetAgentEmailLogoInfo(AgentEmailLogoInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="AgentEmailLogoInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="AgentEmailLogoInfo"/> to be deleted.</param>
        public static void DeleteAgentEmailLogoInfo(AgentEmailLogoInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="AgentEmailLogoInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="AgentEmailLogoInfo"/> ID.</param>
        public static void DeleteAgentEmailLogoInfo(int id)
        {
            AgentEmailLogoInfo infoObj = GetAgentEmailLogoInfo(id);
            DeleteAgentEmailLogoInfo(infoObj);
        }
    }
}