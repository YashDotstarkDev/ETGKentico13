using System;
using System.Collections.Generic;
using CMS.SiteProvider;
using Devotion.Cache;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Repositories.Cached
{
    public class CachedTourPricingRepository : ICachedTourPricingRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ITourPricingRepository _tourPricingRepository;

        public CachedTourPricingRepository(ICacheProvider cacheProvider, ITourPricingRepository tourPricingRepository)
        {
            _cacheProvider = cacheProvider;
            _tourPricingRepository = tourPricingRepository;
        }

        public List<PricingModel> GetTourPricings(string aliasPath)
        {
            return _cacheProvider.GetCached(() => _tourPricingRepository.GetTourPricings(aliasPath),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettourpricings")
                    .Append("byaliaspath")
                    .Append(aliasPath),
                new GenericDependencyBuilder<Core.PageTypes.TourPricing>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(aliasPath));
        }

        public List<PricingModel> GetTourPricings(List<string> aliasPaths)
        {
            return _cacheProvider.GetCached(() => _tourPricingRepository.GetTourPricings(aliasPaths),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettourpricings")
                    .Append("byaliaspaths")
                    .Append(string.Join("-", aliasPaths)),
                new GenericDependencyBuilder<Core.PageTypes.TourPricing>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }

        public List<PricingModel> GetTourPricings(List<Guid> tourGuids)
        {
            return _cacheProvider.GetCached(() => _tourPricingRepository.GetTourPricings(tourGuids),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettourpricings")
                    .Append("bytourguids")
                    .Append(string.Join("-", tourGuids)),
                new GenericDependencyBuilder<Core.PageTypes.TourPricing>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }
    }
}