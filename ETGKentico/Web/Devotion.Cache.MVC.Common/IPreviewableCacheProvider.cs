using System;
using System.Data;
using System.Threading.Tasks;

namespace Devotion.Cache.MVC
{
    public interface IPreviewableCacheProvider
    {
        bool PreviewEnabled { get; }

        int CacheMinutes { get; }

        DataSet GetCached(Func<bool, DataSet> funcToCache, string key);

        T GetCached<T>(Func<bool, T> funcToCache, string key, string dependencyValue, int? minutes = null);

        Task<T> GetCachedAsync<T>(Func<bool, Task<T>> funcToCache, string key, string dependencyValue, int? minutes = null);

        T GetCachedCustomTableData<T>(Func<bool, T> func, string tableName, int? minutes = null);

        T GetCachedObjectData<T>(Func<bool, T> func, string objectType, int? minutes = null);
    }
}