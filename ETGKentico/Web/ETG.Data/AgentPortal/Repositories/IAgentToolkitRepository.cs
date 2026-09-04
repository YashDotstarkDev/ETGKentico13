using System.Collections.Generic;
using ETG.Data.AgentPortal.Models;

namespace ETG.Data.AgentPortal.Repositories
{
    public interface IAgentToolkitRepository
    {
        List<AgentToolkitModel> GetToolkits(string path);
    }
}
