using Devotion.Data;
using ETG.Data.TravelType.Models;

namespace ETG.Data.TravelType.Repositories
{
    public interface ITravelTypeDetailPageRepository : IRepository<TravelTypeDetailPageModel>
    {
        TravelTypeLandingModel GetLandingPage(string url, string path = "");
    }
}