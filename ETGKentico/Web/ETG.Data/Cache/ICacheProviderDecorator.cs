using System;
using Devotion.Cache;

namespace ETG.Data.Cache
{
    public interface ICacheProviderDecorator : ICacheProvider
    {
        T GetCachedPathDependency<T>(Func<T> funcToCache, string key, int? minutes = null, string[] additionalCacheDependency = null);
    }
}