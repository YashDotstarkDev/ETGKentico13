using System;

namespace ETG.Data.Cache
{
    public interface ICacheService
    {
        T GetMedia<T>(Func<T> function, string cachedItemName, Guid guid);
        T GetDocumentDependentOnAll<T>(Func<T> function, string cachedItemName, string className);
        T GetDocumentDependentOnChildrenPath<T>(Func<T> function, string cachedItemName, string path);
        T GetDocumentDependentOnGuid<T>(Func<T> function, string cachedItemName, Guid guid);
        T GetDocumentDependentOnPath<T>(Func<T> function, string cachedItemName, string path);
        T GetAllObjectDependency<T>(Func<T> function, string cachedItemName, string className);
        T GetDocumentDependentOnPath<T>(Func<T> function, string cachedItemName, string[] additionalCacheDependency = null);
        T GetDocumentDependentOnAll<T>(Func<T> function, string cachedItemName, params string[] classNames);
        T GetDocumentMultipleDependency<T>(Func<T> function, string cachedItemName, params string[] dependencies);
    }
}
