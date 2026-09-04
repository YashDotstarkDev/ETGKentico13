using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Factories
{
    public interface ITourModelFactory
    {
        TourSummaryInfoModel CreateSummaryInfoModel(Core.PageTypes.Tour tour);
        TourModel CreateTourModel(Core.PageTypes.Tour tour);
    }
}
