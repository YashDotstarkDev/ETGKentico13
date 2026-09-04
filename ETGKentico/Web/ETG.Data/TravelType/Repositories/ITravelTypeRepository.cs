using System.Collections.Generic;
using Devotion.Data;
using ETG.Data.TravelType.Models;

namespace ETG.Data.TravelType.Repositories
{
    public interface ITravelTypeRepository
    {
        TravelTypeLandingModel GetLanding(string path);
        TravelTypeDetailModel GetTravelTypeDetail(string path);

        IEnumerable<TravelTypeDetailSummaryModel> GetTravelTypeDetails(string path);
    }
}