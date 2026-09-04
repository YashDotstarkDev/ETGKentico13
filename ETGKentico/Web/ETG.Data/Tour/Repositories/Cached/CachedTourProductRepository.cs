using System;
using System.Collections.Generic;
using CMS.SiteProvider;
using Devotion.Cache;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Repositories.Cached
{
    public class CachedTourProductRepository : ICachedTourProductRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ITourProductRepository _tourProductRepository;

        public CachedTourProductRepository(ICacheProvider cacheProvider, ITourProductRepository tourProductRepository)
        {
            _cacheProvider = cacheProvider;
            _tourProductRepository = tourProductRepository;
        }

        public TourModel GetTour(string path)
        {
            return _cacheProvider.GetCached(() => _tourProductRepository.GetTour(path),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettour")
                    .Append("byaliaspath")
                    .Append(path),
                new GenericDependencyBuilder<Core.PageTypes.Tour>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(path));
        }

        public TourModel GetTourByPageAlias(string alias)
        {
            return _cacheProvider.GetCached(() => _tourProductRepository.GetTourByPageAlias(alias),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettourbypagealias")
                    .Append("like")
                    .Append(alias),
                new GenericDependencyBuilder<Core.PageTypes.Tour>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }

        public TourModel GetTourByTourCode(string tourCode)
        {
            return _cacheProvider.GetCached(() => _tourProductRepository.GetTourByTourCode(tourCode),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettourbytourcode")
                    .Append(tourCode),
                new GenericDependencyBuilder<Core.PageTypes.Tour>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }

        public List<TourSummaryInfoModel> GetToursByTourCodes(List<string> tourCodes)
        {
            return _cacheProvider.GetCached(() => _tourProductRepository.GetToursByTourCodes(tourCodes),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettoursbytourcodes")
                    .Append(string.Join("-", tourCodes)),
                new GenericDependencyBuilder<Core.PageTypes.Tour>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }

        public List<TourSummaryInfoModel> GetToursByExperience(Guid experienceGuid, int topN)
        {
            return _cacheProvider.GetCached(() => _tourProductRepository.GetToursByExperience(experienceGuid, topN),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettoursbyexperience")
                    .Append(experienceGuid)
                    .Append("topN")
                    .Append(topN),
                new GenericDependencyBuilder<Core.PageTypes.Tour>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }

        public List<TourSummaryInfoModel> GetToursByDestination(Guid destinationGuid, int topN)
        {
            return _cacheProvider.GetCached(() => _tourProductRepository.GetToursByDestination(destinationGuid, topN),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettoursbydestination")
                    .Append(destinationGuid)
                    .Append("topN")
                    .Append(topN),
                new GenericDependencyBuilder<Core.PageTypes.Tour>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }

        public List<TourSummaryInfoModel> GetTours(int topN)
        {
            return _cacheProvider.GetCached(() => _tourProductRepository.GetTours(topN),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettours")
                    .Append("topN")
                    .Append(topN),
                new GenericDependencyBuilder<Core.PageTypes.Tour>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }

        public List<TourSummaryInfoModel> GetToursByDestinations(List<Guid> destinationGuids, int topN)
        {
            return _cacheProvider.GetCached(() => _tourProductRepository.GetToursByDestinations(destinationGuids, topN),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettoursbydestinations")
                    .Append(string.Join("-", destinationGuids))
                    .Append("topN")
                    .Append(topN),
                new GenericDependencyBuilder<Core.PageTypes.Tour>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }
    }
}