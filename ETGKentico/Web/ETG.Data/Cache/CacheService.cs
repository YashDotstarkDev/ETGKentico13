using Devotion.Cache;
using ETG.Core.Kentico;
using System;
using System.Text;

namespace ETG.Data.Cache
{
    public class CacheService : ICacheService
    {
        private readonly ISiteContext _siteContext;
        private readonly ICacheProviderDecorator _cacheProvider;
        private readonly ICacheDependencyFactory _cacheDependencyFactory;
        public CacheService(ISiteContext siteContext, ICacheProviderDecorator cacheProvider, ICacheDependencyFactory cacheDependencyFactory)
        {
            _siteContext = siteContext;
            _cacheProvider = cacheProvider;
            _cacheDependencyFactory = cacheDependencyFactory;
        }

        public T GetDocumentDependentOnChildrenPath<T>(Func<T> function, string cachedItemName, string path)
        {
            return _cacheProvider.GetCached(function,
                      new CacheKeyBuilder(_siteContext.SiteName)
                          .Append($"{cachedItemName}{path}"), _cacheDependencyFactory.GetChildrenNodePathDependency(path));
        }

        public T GetDocumentDependentOnPath<T>(Func<T> function, string cachedItemName, string path)
        {
            return _cacheProvider.GetCached(function,
                          new CacheKeyBuilder(_siteContext.SiteName)
                              .Append($"{cachedItemName}{path}"), _cacheDependencyFactory.GetSingleNodeDependency(path));

        }

        public T GetDocumentDependentOnPath<T>(Func<T> function, string cachedItemName, string[] additionalCacheDependency = null)
        {
            return _cacheProvider.GetCachedPathDependency(function,
                new CacheKeyBuilder(_siteContext.SiteName)
                    .Append($"{cachedItemName}"),null, additionalCacheDependency);
        }

        public T GetDocumentDependentOnGuid<T>(Func<T> function, string cachedItemName, Guid guid)
        {
            return _cacheProvider.GetCached(function,
                          new CacheKeyBuilder(_siteContext.SiteName)
                              .Append($"{cachedItemName}{guid}"), _cacheDependencyFactory.GetSingleNodeDependency(guid));

        }

        public T GetMedia<T>(Func<T> function, string cachedItemName, Guid guid)
        {
            return _cacheProvider.GetCached(function,
                          new CacheKeyBuilder(_siteContext.SiteName)
                              .Append($"{cachedItemName}{guid}"), _cacheDependencyFactory.GetMediaFileDependency(guid));

        }

        public T GetAllObjectDependency<T>(Func<T> function, string cachedItemName, string className)
        {
            return _cacheProvider.GetCached(function,
                         new CacheKeyBuilder(_siteContext.SiteName)
                             .Append($"{cachedItemName}{className}"), _cacheDependencyFactory.GetDependencyByClassName(className));
        }

        public T GetDocumentDependentOnAll<T>(Func<T> function, string cachedItemName, string className)
        {
            return _cacheProvider.GetCached(function,
                         new CacheKeyBuilder(_siteContext.SiteName)
                             .Append($"{cachedItemName}{className}"), _cacheDependencyFactory.GetPageTypeDependency(className));
        }
        
        public T GetDocumentDependentOnAll<T>(Func<T> function, string cachedItemName, params string[] classNames)
        {
            var dependency = new StringBuilder();
            foreach (var c in classNames)
            {
                dependency.AppendLine(_cacheDependencyFactory.GetPageTypeDependency(c));
            }

            dependency.AppendLine();
            return _cacheProvider.GetCached(function,
                new CacheKeyBuilder(_siteContext.SiteName)
                    .Append($"{cachedItemName}"), dependency.ToString());
        }

        public T GetDocumentMultipleDependency<T>(Func<T> function, string cachedItemName, params string[] dependencies)
        {
            var dependency = new StringBuilder();
            foreach (var d in dependencies)
            {
                dependency.AppendLine(d);
            }

            dependency.AppendLine();
            return _cacheProvider.GetCached(function,
                new CacheKeyBuilder(_siteContext.SiteName)
                    .Append($"{cachedItemName}"), dependency.ToString());
        }
    }
}
