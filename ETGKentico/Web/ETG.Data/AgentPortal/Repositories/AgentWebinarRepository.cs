using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using ETG.Core.PageTypes.Providers;
using ETG.Data.AgentPortal.Models;

namespace ETG.Data.AgentPortal.Repositories
{
    public class AgentWebinarRepository : IAgentWebinarRepository
    {


        public List<AgentWebinarModel> GetWebinars(string path)
        {
            return WebinarProvider.GetWebinars().Path(path, PathTypeEnum.Section).OnCurrentSite().Select(a => new AgentWebinarModel
            {
                Name = a.WebinarName,
                Image = a.WebinarImage,
                ComingSoonDate = a.WebinarComingSoon,
                Description = a.WebinarDescription,
                RecordingUrl = a.WebinarRecordingUrl
            }).ToList();
        }
    }
}
