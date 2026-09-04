using System;
using ETG.Data.Tour.Models;
using System.Collections.Generic;
using ETG.Data.Tour.Repositories;

namespace ETG.Data.Tour.Services
{
    public interface ITourService
    {
        void SetTourRepository(ITourProductRepository repository);
        TourModel GetTourByTourCode(string tourcode);
        TourModel GetPreviewableTourByTourCode(string tourcode);
        TourModel GetTourByUrl(string url);
        TourModel GetTourByAlias(string alias);
        List<PricingModel> GetTourPricings(string aliasPath);
        TourSummaryInfoModel AssignOtherDetailsForTiledTour(TourSummaryInfoModel tour, bool getPrice = false);

        List<TourSummaryInfoModel> GetTiledTours(List<string> tourCodes, bool includeHiddenUpgradeTours = false);
        List<TourSummaryInfoModel> GetTiledToursByTourType(string tourType, int topN);

        List<TourSummaryInfoModel> GetTiledToursByCruiseType(int cruiseType, int topN);
        List<TourSummaryInfoModel> GetTiledToursByDestination(Guid destinationGuid, int topN);

        List<TourSummaryInfoModel> GetTiledToursByExperience(Guid experienceGuid, int topN);

        List<TourSummaryInfoModel> GetTiledTours(int topN);
        List<TourSummaryInfoModel> GetTiledToursByDestinations(List<Guid> destinationGuids, int topN);
        List<TourSummaryInfoModel> GetRelatedTiledTours(int topN, TourSummaryInfoModel tourSummaryInfo);
    }
}
