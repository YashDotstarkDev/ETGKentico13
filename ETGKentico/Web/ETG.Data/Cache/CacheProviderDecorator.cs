using System;
using System.Data;
using System.Text;
using Castle.Core.Internal;
using CMS.Helpers;
using Devotion.Cache;
using ETG.Data.Models.Base;

namespace ETG.Data.Cache
{
    public class CacheProviderDecorator : ICacheProviderDecorator
    {
        public readonly ICacheProvider _cacheProvider;
        public readonly ICacheDependencyFactory _cacheDependencyFactory;
        public CacheProviderDecorator(ICacheProvider cacheProvider, ICacheDependencyFactory cacheDependencyFactory)
        {
            _cacheProvider = cacheProvider;
            _cacheDependencyFactory = cacheDependencyFactory;
        }

        public int CacheMinutes => throw new NotImplementedException();

        public DataSet GetCached(Func<DataSet> func, string key)
        {
            return _cacheProvider.GetCached(func, key);
        }

        public T GetCached<T>(Func<T> funcToCache, string key, string dependencyValue, int? minutes = null)
        {
            return _cacheProvider.GetCached<T>(funcToCache, key, dependencyValue, minutes);

        }

        public T GetCachedCustomTableData<T>(Func<T> func, string tableName, int? minutes = null)
        {
            return _cacheProvider.GetCachedCustomTableData(func, tableName, minutes);
        }

        public T GetCachedObjectData<T>(Func<T> func, string objectType, int? minutes = null)
        {
            return _cacheProvider.GetCachedObjectData(func, objectType, minutes);
        }

        public T GetCachedPathDependency<T>(Func<T> funcToCache, string key, int? minutes = null, string[] additionalCacheDependency = null)
        {
            var cacheMin = minutes ?? _cacheProvider.CacheMinutes;

            var returnValue = default(T);

            using (var cs = new CachedSection<T>(ref returnValue, cacheMin, true, null, key))
            {
                if (cs.LoadData || cs.Data == null)
                {
                    var value = funcToCache.Invoke();
                    var dependencyValue = "/";
                    if (typeof(T).IsSubclassOf(typeof(PageNodeModel)))
                    {
                        var obj = (PageNodeModel)(object)value;// Convert.ChangeType(value, typeof(PageNodeModel));

                        if (obj != null)
                        {
                            if (obj.PageAliasPath.IsNullOrEmpty())
                            {
                                throw new Exception("PageAliasPath cannot be null.");
                            }

                            var stringBuilder = new StringBuilder();
                            stringBuilder.AppendLine(
                                _cacheDependencyFactory.GetSingleNodeDependency((obj).PageAliasPath));
                            stringBuilder.AppendLine(_cacheDependencyFactory.GetChildrenNodePathDependency((obj).PageAliasPath));

                            if (!additionalCacheDependency.IsNullOrEmpty())
                            {
                                foreach (var dependecy in additionalCacheDependency)
                                {
                                    stringBuilder.AppendLine(dependecy);
                                } 
                            }

                            dependencyValue = stringBuilder.ToString();

                        }
                    }
                    
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
    }
}
