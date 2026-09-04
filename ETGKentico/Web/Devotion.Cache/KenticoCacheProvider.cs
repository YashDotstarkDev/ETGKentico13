using System;
using System.Data;
using JetBrains.Annotations;
using CMS.Helpers;
using CMS.SiteProvider;

namespace Devotion.Cache
{
    [PublicAPI]
    public class KenticoCacheProvider : ICacheProvider
    {
        public int CacheMinutes => CacheHelper.CacheMinutes(SiteContext.CurrentSiteName);

        public DataSet GetCached(Func<DataSet> func, string key)
        {
            var value = default(DataSet);

            using (var cs = new CachedSection<DataSet>(ref value, CacheMinutes, true, key, null))
            {
                if (!cs.LoadData)
                {
                    return value;
                }

                value = func.Invoke();
                cs.Data = value;
            }

            return value;
        }

        public T GetCached<T>(Func<T> funcToCache, string key, string dependencyValue, int? minutes = null)
        {
            var cacheMin = minutes ?? CacheMinutes;

            var returnValue = default(T);

            using (var cs = new CachedSection<T>(ref returnValue, cacheMin, true, null, key))
            {
                if (cs.LoadData || cs.Data == null)
                {
                    var value = funcToCache.Invoke();

                    if (!string.IsNullOrEmpty(dependencyValue))
                    {
                        var builder = new CacheDependencyBuilder();
                        builder.Add(dependencyValue);
                        cs.CacheDependency = builder.Build();
                    }

                    cs.Data = value;
                    return value;
                }

                return returnValue;
            }
        }

        public T GetCachedCustomTableData<T>(Func<T> func, string tableName, int? minutes = null)
        {
            var cacheMin = minutes ?? CacheMinutes;

            var returnValue = default(T);

            using (var cs = new CachedSection<T>(ref returnValue, cacheMin, true, tableName, null))
            {
                if (!cs.LoadData && cs.Data != null)
                {
                    return returnValue;
                }

                var value = func.Invoke();
                var builder = new CacheDependencyBuilder();
                builder.Add($"customtableitem.{tableName}|all");
                cs.CacheDependency = builder.Build();
                cs.Data = value;
                return value;

            }
        }

        public T GetCachedObjectData<T>(Func<T> func, string objectType, int? minutes = null)
        {
            var cacheMin = minutes ?? CacheMinutes;

            var returnValue = default(T);

            using (var cs = new CachedSection<T>(ref returnValue, cacheMin, true, objectType, null))
            {
                if (!cs.LoadData && cs.Data != null)
                {
                    return returnValue;
                }

                var value = func.Invoke();
                var builder = new CacheDependencyBuilder();
                builder.Add($"{objectType}|all");
                cs.CacheDependency = builder.Build();
                cs.Data = value;
                return value;
            }
        }
    }
}