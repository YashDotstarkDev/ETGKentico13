using ETG.Core.PageTypes.Providers;
using ETG.Data.Brochure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Data.Brochure.Repositories
{
    public class BrochureRepository : IBrochureRepository
    {
        private BrochureModel CreateModel(Core.PageTypes.Brochure item)
        {
            return new BrochureModel
            {
                BrochureNodeGuid = item.NodeGUID,
                Name = item.BrochureName,
                CoverImage = item.BrochureCoverImage,
                IssuuLink = item.BrochureIssuuLink,
                FlipStackID = item.BrochureFlipStackID,
                FilePath = item.BrochureFile,
                DestinationGuids = item.Destinations,
                AgentPurchaseLink = item.BrochureAgentPurchaseLink,
                PageDescription = item.DocumentPageDescription,
                PageAliasPath = item.NodeAliasPath,
                PageTitle = item.DocumentPageTitle,
                ShareTitle = item.DocumentPageTitle,
                ShareDescription = item.DocumentPageDescription,
                ShareImage = item.BrochureCoverImage,
                PageAlias = item.NodeAlias,
                ExcludedFromSearch = item.DocumentSearchExcluded,
                IsComingSoon = item.IsComingSoon
            };
        }
        public List<BrochureModel> GetBrochures(string path)
        {
            return BrochureProvider.GetBrochures().Path(path, CMS.DocumentEngine.PathTypeEnum.Section).OnCurrentSite()
                .Select(CreateModel).ToList();
        }

        public List<BrochureModel> GetBrochures(List<Guid> brochureGuids)
        {
            return BrochureProvider.GetBrochures().WhereIn("NodeGuid", brochureGuids).OnCurrentSite()
                .Select(CreateModel).ToList();
        }
    }
}
