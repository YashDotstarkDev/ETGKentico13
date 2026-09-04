using ETG.Data.DestinationExpertTeam.Models;
using System.Collections.Generic;

namespace ETG.Data.DestinationExpertTeam.Repositories
{
    public interface IDestinationExpertTeamRepository
    {
        DestinationExpertTeamModel GetExpertTeam(string path);
        List<DestinationExpertTeamSummaryModel> GetExpertTeams(string path);
    }
}
