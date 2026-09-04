using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.ECommerce
{
    public interface IAgentLogoRepository
    {
        AgentLogoInfo GetAgentLogo(string domain, string agentEmail);
        AgentEmailLogoInfo GetAgentEmailLogo(string email);
    }
}