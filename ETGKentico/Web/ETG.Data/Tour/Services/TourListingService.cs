using System;
using System.Collections.Generic;
using ETG.Data.Cache;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Repositories;

namespace ETG.Data.Tour.Services
{
    public class TourListingService : ITourListingService
    {
        private readonly ITourProductRepository _tourRepository;
        private readonly ICacheService _cacheService;

        public TourListingService(ITourProductRepository tourRepository, ICacheService cacheService)
        {
            _tourRepository = tourRepository;
            _cacheService = cacheService;
        }

        public List<TourSummaryInfoModel> GetCombinedTours(List<string> tourCodes,
            bool includeHiddenUpgradeTours = false)
        {
            return _cacheService.GetDocumentDependentOnAll(
                () => _tourRepository.GetToursByTourCodes(tourCodes, includeHiddenUpgradeTours),
                $"GetCombinedTours{string.Join("-", tourCodes)}{includeHiddenUpgradeTours}",
                Core.PageTypes.Tour.CLASS_NAME,
                Core.PageTypes.DocumentLink.CLASS_NAME,
                Core.PageTypes.TourInclusion.CLASS_NAME);
        }

        public List<TourSummaryInfoModel> GetCombinedToursByDestination(Guid destinationGuid, int topN)
        {
            return _cacheService.GetDocumentDependentOnAll(
                () => _tourRepository.GetToursByDestination(destinationGuid, topN),
                $"GetCombinedToursByDestination{destinationGuid}{topN}",
                Core.PageTypes.Tour.CLASS_NAME,
                Core.PageTypes.DocumentLink.CLASS_NAME,
                Core.PageTypes.TourInclusion.CLASS_NAME);
        }

        public List<TourSummaryInfoModel> GetCombinedToursByExperience(Guid experienceGuid, int topN)
        {
            return _cacheService.GetDocumentDependentOnAll(
                () => _tourRepository.GetToursByExperience(experienceGuid, topN),
                $"GetCombinedToursByExperience{experienceGuid}{topN}",
                Core.PageTypes.Tour.CLASS_NAME,
                Core.PageTypes.DocumentLink.CLASS_NAME,
                Core.PageTypes.TourInclusion.CLASS_NAME);
        }

        public List<TourSummaryInfoModel> GetCombinedTours(int topN)
        {
            return _cacheService.GetDocumentDependentOnAll(() => _tourRepository.GetTours(topN),
                $"GetCombinedTours{topN}",
                Core.PageTypes.Tour.CLASS_NAME,
                Core.PageTypes.DocumentLink.CLASS_NAME,
                Core.PageTypes.TourInclusion.CLASS_NAME);
        }

        public List<TourSummaryInfoModel> GetCombinedToursByDestinations(List<Guid> destinationGuids, int topN)
        {
            return _cacheService.GetDocumentDependentOnAll(
                () => _tourRepository.GetToursByDestinations(destinationGuids, topN),
                $"GetCombinedTours{string.Join("-", destinationGuids)}{topN}",
                Core.PageTypes.Tour.CLASS_NAME,
                Core.PageTypes.DocumentLink.CLASS_NAME,
                Core.PageTypes.TourInclusion.CLASS_NAME);
        }
    }
}