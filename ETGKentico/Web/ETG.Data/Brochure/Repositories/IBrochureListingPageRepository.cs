using Devotion.Data;
using ETG.Data.Brochure.Models;

namespace ETG.Data.Brochure.Repositories
{
    public interface IBrochureListingPageRepository : IRepository<BrochureListingPageModel>
    {
        BrochureOrderPageModel GetBrochureOrderPage(string brochureGuids);
        BrochurePageModel GetBrochurePage(string url, string alias = "");
    }
}
