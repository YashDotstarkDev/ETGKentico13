using CMS.DocumentEngine;
using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.DestinationExpertTeam.Models;
using System.Collections.Generic;
using System.Linq;
using ETG.Data.Factories;

namespace ETG.Data.DestinationExpertTeam.Repositories
{
    public class DestinationExpertTeamRepository : IDestinationExpertTeamRepository
    {
        private readonly ISiteContext _siteContext;

        public DestinationExpertTeamRepository(ISiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        public DestinationExpertTeamModel GetExpertTeam(string path)
        {
            return DestinationExpertTeamProvider.GetDestinationExpertTeam(path, _siteContext.CurrentCultureCode, _siteContext.SiteName)
                .OnCurrentSite()
                .Select(a => new DestinationExpertTeamModel
                {
                    SummaryInfo = ModelFactory.CreateDestinationExpertTeamSummaryModel(a),
                    ObjectiveStatement = a.DestinationExpertTeamObjectiveStatement,
                    Profile = a.DestinationExpertTeamProfile,
                    DocumentID = a.DocumentID,
                    GoalsInText = a.DestinationExpertTeamGoals,
                    FeatureTourCodes = a.DestinationExpertTeamFeatureTours,
                    FeatureCruiseCodes = a.DestinationExpertTeamFeatureCruises,
                    PageTitle = a.DocumentPageTitle,
                    PageDescription = a.DocumentPageDescription,
                    PageAliasPath = a.NodeAliasPath,
                    PageAlias = a.NodeAlias,
                    PageKeywords = a.DocumentPageKeyWords,
                    ShareTitle = $"Team {a.DestinationExpertTeamName}",
                    ShareDescription = a.DestinationExpertTeamSummary,
                    ShareImage = a.DestinationExpertTeamHeroImage,
                    ExcludedFromSearch = a.DocumentSearchExcluded
                }).FirstOrDefault();
        }

        public List<DestinationExpertTeamSummaryModel> GetExpertTeams(string path)
        {
            return DestinationExpertTeamProvider.GetDestinationExpertTeams().Path(path, PathTypeEnum.Children).OnCurrentSite()
                 .OnCurrentSite()
                 .Select(ModelFactory.CreateDestinationExpertTeamSummaryModel).ToList();
        }
    }
}

