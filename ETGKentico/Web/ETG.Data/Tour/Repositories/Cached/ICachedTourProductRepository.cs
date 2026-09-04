using System;
using System.Collections.Generic;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Repositories.Cached
{
    public interface ICachedTourProductRepository
    {
        TourModel GetTour(string path);
        
        TourModel GetTourByPageAlias(string alias);
        
        List<TourSummaryInfoModel> GetToursByTourCodes(List<string> tourCodes);

        List<TourSummaryInfoModel> GetToursByExperience(Guid experienceGuid, int topN);

        List<TourSummaryInfoModel> GetToursByDestination(Guid destinationGuid, int topN);
        
        List<TourSummaryInfoModel> GetToursByDestinations(List<Guid> destinationGuids, int topN);

        List<TourSummaryInfoModel> GetTours(int topN);

        TourModel GetTourByTourCode(string tourCode);
    }
}