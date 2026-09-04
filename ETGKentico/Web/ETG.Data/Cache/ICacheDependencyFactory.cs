using System;

namespace ETG.Data.Cache
{
    public interface ICacheDependencyFactory
    {
        string GetSingleNodeDependency(string aliasPath);
        string GetSingleNodeDependency(Guid guid);

        string GetPageTypeDependency(string className);

        string GetCustomTableItemsDependency(string className);

        string GetChildrenNodePathDependency(string aliasPath);

        string GetSingleNodeIdDependency(int id);
        string GetMediaFileDependency(Guid guid);
        string GetDependencyByClassName(string className);
    }
}
