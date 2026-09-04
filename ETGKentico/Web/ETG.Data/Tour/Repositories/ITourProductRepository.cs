using ETG.Data.Tour.Models;
using System;
using System.Collections.Generic;

namespace ETG.Data.Tour.Repositories
{
    public interface ITourProductRepository
    {
        TourModel GetTourByPageAlias(string alias);
        List<TourSummaryInfoModel> GetToursByTourCodes(List<string> tourCodes, bool includeHiddenUpgradeTours = false);
        List<TourSummaryInfoModel> GetToursByTourType(string tourType, int topN);
        List<TourSummaryInfoModel> GetToursByCruiseType(int cruiseType, int topN);
        List<TourSummaryInfoModel> GetToursByExperience(Guid experienceGuid, int topN);

        List<TourSummaryInfoModel> GetToursByDestination(Guid destinationGuid, int topN);
        List<TourSummaryInfoModel> GetToursByDestinations(List<Guid> destinationGuids, int topN);
        List<TourSummaryInfoModel> GetTours(int topN);
        TourModel GetTourByTourCode(string tourCode);
        TourModel GetPreviewableTourByTourCode(string tourCode);
        List<TourBookingAdditions> GetTourBookingAdditions(List<string> tourCodes);
        List<Core.PageTypes.Tour> GetToursWithDiscounts(List<Guid> discountGuids);
        List<string> GetInvalidTourCodes(List<string> allTourCodes);
    }
}
