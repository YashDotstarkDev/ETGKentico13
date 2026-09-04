using System;
using System.Data;
using System.Threading.Tasks;
using System.Web;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace Devotion.Cache.MVC
{
    public class PreviewableCacheProvider : IPreviewableCacheProvider
    {
        private readonly ICacheProvider _cacheProvider;
        public bool PreviewEnabled => HttpContext.Current.Kentico().Preview().Enabled;

        public PreviewableCacheProvider(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public int CacheMinutes => _cacheProvider.CacheMinutes;

        public DataSet GetCached(Func<bool, DataSet> funcToCache, string key)
        {
            return PreviewEnabled ? _cacheProvider.GetCached(() => funcToCache(true), key) : funcToCache(false);
        }

        public T GetCached<T>(Func<bool, T> funcToCache, string key, string dependencyValue, int? minutes = null)
        {
            return PreviewEnabled ? _cacheProvider.GetCached(() => funcToCache(true), key, dependencyValue, minutes) : funcToCache(false);
        }

        public Task<T> GetCachedAsync<T>(Func<bool, Task<T>> funcToCache, string key, string dependencyValue, int? minutes = null)
        {
            return PreviewEnabled ? _cacheProvider.GetCached(() => funcToCache(true), key, dependencyValue, minutes) : funcToCache(false);
        }

        public T GetCachedCustomTableData<T>(Func<bool, T> func, string tableName, int? minutes = null)
        {
            return PreviewEnabled ? _cacheProvider.GetCachedCustomTableData(() => func(true), tableName, minutes) : func(false);
        }

        public T GetCachedObjectData<T>(Func<bool, T> func, string objectType, int? minutes = null)
        {
            return PreviewEnabled ? _cacheProvider.GetCachedObjectData(() => func(true), objectType, minutes) : func(false);
        }
    }
}