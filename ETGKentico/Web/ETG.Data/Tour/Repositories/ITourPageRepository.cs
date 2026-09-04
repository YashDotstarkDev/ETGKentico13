using Devotion.Data;
using ETG.Data.Models.Pages;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Repositories
{
    public interface ITourPageRepository : IRepository<TourPageModel>
    {
        TourEnquirePageModel GetEnquiryPage(string tourcode);
    }
}
