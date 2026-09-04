using ETG.Core.PageTypes.Providers;
using ETG.Data.AgentPortal.Models;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;

namespace ETG.Data.AgentPortal.Repositories
{
    public class AgentToolkitRepository : IAgentToolkitRepository
    {


        public List<AgentToolkitModel> GetToolkits(string path)
        {
            return ToolkitProvider.GetToolkits().Path(path, PathTypeEnum.Section).OnCurrentSite().Select(a => new AgentToolkitModel
            {
                Name = a.ToolkitName,
                Image = a.ToolkitImage,
                Url = a.ToolkitUrl
            }).ToList();
        }
    }
}
