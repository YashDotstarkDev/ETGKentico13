using ETG.Data.Models.PageTypes;

namespace ETG.Data.Repositories
{
    public interface IHomepageRepository
    {
        HomepageModel GetHomepage(string path);
    }
}
