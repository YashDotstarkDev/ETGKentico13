using Devotion.Data;
using ETG.Data.Experience.Models;
using ETG.Data.Models.Pages;

namespace ETG.Data.Experience.Repositories { 
    public interface IExperiencePageRepository : IRepository<ExperiencePageModel>
    {
        PrimaryLandingPageModel GetLandingPage(string url, string path = "");
    }
}
