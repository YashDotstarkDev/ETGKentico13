using System;
using System.Collections.Generic;
using ETG.Data.DestinationExpertTeam.Models;

namespace ETG.Data.DestinationExpertTeam.Services
{
    public interface IDestinationExpertTeamService
    {
        Guid GetRandomDestinationExpertGuid();
        List<DestinationExpertTeamSummaryModel> GetAllDestinationExperts();
        DestinationExpertTeamSummaryModel GetDestinationExpert(Guid guid);
        DestinationExpertTeamSummaryModel GetDestinationExpertByDestinationGuid(Guid guid);
    }
}
