using System.Linq;
using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.AgentPortal.Models;

namespace ETG.Data.AgentPortal.Repositories
{
    public class AgentsPortalMainRepository : IAgentsPortalMainRepository
    {
        private readonly ISiteContext _siteContext;
        public AgentsPortalMainRepository(ISiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        public AgentPortalMainModel GetAgentPortalPage(string path)
        {
            return AgentPortalMainPageProvider
                .GetAgentPortalMainPage(path, _siteContext.CurrentCultureCode, _siteContext.SiteName).Select(a =>
                    new AgentPortalMainModel
                    {
                        MainHeading = a.AgentPortalHeading,
                        MainDescription = a.AgentPortalMainDescription,

                        IncentiveHeading = a.AgentPortalIncentiveHeading,
                        IncentiveDescription = a.AgentPortalIncentiveDescription,

                        ToolkitHeading = a.AgentPortalTookitHeading,
                        ToolkitDescription = a.AgentPortalToolkitDescription,

                        WebinarHeading = a.AgentPortalWebinarHeading,
                        WebinarDescription = a.AgentPortalWebinarDescription,

                        BookingHeading = a.AgentPortalBookingHeading,
                        BookingDescription = a.AgentPortalBookingDescription,

                        BookingOptionsHeading = a.AgentPortalBookingOptionsHeading,
                        BookingOptionsDescription = a.AgentPortalBookingOptionsDescription,

                        AgentInstructionHeading = a.AgentPortalInstructionHeading,
                        AgentInstructionDescription = a.AgentPortalInstructionDescription,

                        PaymentsHeading = a.AgentPortalPaymentsHeading,
                        PaymentsDescription = a.AgentPortalPaymentsDescription
                    })
                .FirstOrDefault();
        }
    }
}
