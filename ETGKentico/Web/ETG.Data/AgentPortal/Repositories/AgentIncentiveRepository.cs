using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using ETG.Core.Constants;
using ETG.Core.PageTypes.Providers;
using ETG.Data.AgentPortal.Models;

namespace ETG.Data.AgentPortal.Repositories
{
    public class AgentIncentiveRepository : IAgentIncentiveRepository
    {
   

        public List<AgentIncentiveModel> GetIncentives(string path)
        {
            return IncentiveProvider.GetIncentives().Path(path, PathTypeEnum.Section).OnCurrentSite().Select(a =>
                new AgentIncentiveModel
                {
                    Name = a.IncentiveName,
                    Image = a.IncentiveImage,
                    Description = a.IncentiveDescription,
                    Destination = a.IncentiveDestination,
                    Url = $"{PathConstants.AGENT_INCENTIVES}/{a.NodeAlias}"
                }).ToList();
        }

        public AgentIncentiveModel GetIncentive(string alias)
        {
            return IncentiveProvider.GetIncentives().WhereLike("NodeAlias", alias).OnCurrentSite().Select(a =>
                new AgentIncentiveModel
                {
                    DocumentID = a.DocumentID,
                    PageTitle = a.DocumentPageTitle,
                    PageDescription = a.DocumentPageDescription,
                    PageAliasPath = a.NodeAliasPath,
                    PageAlias = a.NodeAlias,
                    PageKeywords = a.DocumentPageKeyWords,
                    Name = a.IncentiveName,
                    Image = a.IncentiveImage,
                    Description = a.IncentiveDescription,
                    Destination = a.IncentiveDestination,
                    HeroImage = a.IncentiveHeroImage,
                    HeroImageCaption = a.IncentiveHeroImageAccreditation
                }).FirstOrDefault();
        }
    }
}
