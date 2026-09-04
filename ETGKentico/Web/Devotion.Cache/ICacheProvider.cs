using System;
using System.Data;

namespace Devotion.Cache
{
    public interface ICacheProvider
    {
        int CacheMinutes { get; }

        DataSet GetCached(Func<DataSet> func, string key);

        T GetCached<T>(Func<T> funcToCache, string key, string dependencyValue, int? minutes = null);

        T GetCachedCustomTableData<T>(Func<T> func, string tableName, int? minutes = null);

        T GetCachedObjectData<T>(Func<T> func, string objectType, int? minutes = null);
    }
}