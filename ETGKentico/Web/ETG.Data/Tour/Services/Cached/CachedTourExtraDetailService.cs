using System.Collections.Generic;
using CMS.SiteProvider;
using Devotion.Cache;
using ETG.Core.PageTypes;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Services.Cached
{
    public class CachedTourExtraDetailService : ICachedTourExtraDetailService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ITourExtraDetailService _tourExtraDetailService;

        public CachedTourExtraDetailService(ICacheProvider cacheProvider, ITourExtraDetailService tourExtraDetailService)
        {
            _cacheProvider = cacheProvider;
            _tourExtraDetailService = tourExtraDetailService;
        }

        public List<TourItineraryModel> GetTourItinerary(string path)
        {
            return _cacheProvider.GetCached(() => _tourExtraDetailService.GetTourItinerary(path),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettouritinerary")
                    .Append("byaliaspath")
                    .Append(path),
                new GenericDependencyBuilder<TourItinerary>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(path));
        }

        public List<NameValuePathModel> GetTourHighlights(string path)
        {
            return _cacheProvider.GetCached(() => _tourExtraDetailService.GetTourHighlights(path),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettourhighlights")
                    .Append("byaliaspath")
                    .Append(path),
                new GenericDependencyBuilder<TourHighlilght>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(path));
        }

        public List<KeyValuePair<string, string>> GetTourInclusions(string path)
        {
            return _cacheProvider.GetCached(() => _tourExtraDetailService.GetTourInclusions(path),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gettourinclusions")
                    .Append("byaliaspath")
                    .Append(path),
                new GenericDependencyBuilder<TourInclusion>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(path));
        }

        public List<HotelModel> GetHotels(string guids)
        {
            return _cacheProvider.GetCached(() => _tourExtraDetailService.GetHotels(guids),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("gethotels")
                    .Append("byguids")
                    .Append(guids),
                new GenericDependencyBuilder<Hotel>(SiteContext.CurrentSiteName)
                    .DependsOnAllNodesOfPageType());
        }
    }
}