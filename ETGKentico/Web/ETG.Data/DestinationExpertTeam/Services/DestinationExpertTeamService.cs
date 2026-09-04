using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Cache;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Factories;

namespace ETG.Data.DestinationExpertTeam.Services
{
    public class DestinationExpertTeamService: IDestinationExpertTeamService
    {
        private readonly ICacheService _cacheService;
        public DestinationExpertTeamService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }


        private List<DestinationExpertTeamSummaryModel> GetAllDestinationExpertsInternal()
        {
            return DestinationExpertTeamProvider.GetDestinationExpertTeams().OnCurrentSite().OrderBy("NodeOrder").Select(
                ModelFactory.CreateDestinationExpertTeamSummaryModel).ToList();
        }

        public List<DestinationExpertTeamSummaryModel> GetAllDestinationExperts()
        {
            return _cacheService.GetDocumentDependentOnAll(() => GetAllDestinationExpertsInternal(),
                "allDestinationExperts", Core.PageTypes.DestinationExpertTeam.CLASS_NAME);
        }


        public DestinationExpertTeamSummaryModel GetDestinationExpert(Guid guid)
        {
            return GetAllDestinationExperts().FirstOrDefault(a => a.DestinationExpertTeamGuid == guid);
        }

        public DestinationExpertTeamSummaryModel GetDestinationExpertByDestinationGuid(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                return null;
            }
            return GetAllDestinationExperts().FirstOrDefault(a => a.Destinations != null && a.Destinations.Contains(guid.ToString()) );
        }

        public Guid GetRandomDestinationExpertGuid()
        {
            var allExperts = GetAllDestinationExperts();

            if (allExperts.IsNullOrEmpty())
            {
                return Guid.Empty;
            }
            Random m = new Random();
            var index = m.Next(0, allExperts.Count - 1);

            return allExperts[index].DestinationExpertTeamGuid;
        }
    }
}
