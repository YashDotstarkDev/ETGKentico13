using System.Collections.Generic;
using ETG.Data.AgentPortal.Models;

namespace ETG.Data.AgentPortal.Repositories
{
    public interface IAgentIncentiveRepository
    {
        List<AgentIncentiveModel> GetIncentives(string path);
        AgentIncentiveModel GetIncentive(string alias);
    }
}
