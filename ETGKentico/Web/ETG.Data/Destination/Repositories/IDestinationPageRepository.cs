using Devotion.Data;
using ETG.Data.Destination.Models;
using ETG.Data.Models.Pages;

namespace ETG.Data.Destination.Repositories
{
    public interface IDestinationPageRepository : IRepository<DestinationPageModel>
    {
        PrimaryLandingPageModel GetLandingPage(string url, string path = "");

        bool IsDestinationUnpublished(string url);
    }
}
