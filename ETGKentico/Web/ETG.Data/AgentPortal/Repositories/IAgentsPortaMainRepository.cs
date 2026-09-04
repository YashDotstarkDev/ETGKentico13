using ETG.Data.AgentPortal.Models;

namespace ETG.Data.AgentPortal.Repositories
{
    public interface IAgentsPortalMainRepository
    {
        AgentPortalMainModel GetAgentPortalPage(string path);
    }
}
