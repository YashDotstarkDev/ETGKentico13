using System.Collections.Generic;
using ETG.Data.AgentPortal.Models;

namespace ETG.Data.AgentPortal.Repositories
{
    public interface IAgentWebinarRepository
    {
        List<AgentWebinarModel> GetWebinars(string path);
    }
}
