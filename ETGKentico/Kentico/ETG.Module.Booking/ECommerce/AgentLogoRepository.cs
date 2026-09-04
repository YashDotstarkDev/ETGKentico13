using System.Linq;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

namespace ETG.Module.Booking.ECommerce
{
    public class AgentLogoRepository : IAgentLogoRepository
    {
        public AgentLogoInfo GetAgentLogo(string domain, string agentEmail)
        {
            var agentLogo = AgentLogoInfoProvider.GetAgentLogoes()
                .Where(
                    $"CHAR(13)+CHAR(10) + {nameof(AgentLogoInfo.AgentLogoEmails)} + CHAR(13)+CHAR(10) LIKE '%' + CHAR(13)+CHAR(10) + '{agentEmail}' + CHAR(13)+CHAR(10) + '%'")
                .FirstOrDefault();

            if (agentLogo != null)
            {
                return agentLogo;
            }
            return AgentLogoInfoProvider.GetAgentLogoes().WhereLike(nameof(AgentLogoInfo.AgentLogoDomain), domain)
                .Or().WhereLike(nameof(AgentLogoInfo.AgentLogoDomain), $@"{domain}")
                .FirstOrDefault();
        }

        public AgentEmailLogoInfo GetAgentEmailLogo(string email)
        {
            return AgentEmailLogoInfoProvider.GetAgentEmailLogoes().WhereLike(nameof(AgentEmailLogoInfo.AgentEmailEmailAddress), email)
                .FirstOrDefault();
        }
    }
}