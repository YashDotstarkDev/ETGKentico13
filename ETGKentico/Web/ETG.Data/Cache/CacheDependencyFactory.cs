using ETG.Core.Kentico;
using System;

namespace ETG.Data.Cache
{
    public class CacheDependencyFactory : ICacheDependencyFactory
    {
        private const string _pageTypeDependency = "nodes|{0}|{1}|all";
        private const string _singleNodePathDependency = "node|{0}|{1}";
        private const string _singleNodeGuidDependency = "nodeguid|{0}|{1}";
        private const string _customTableItemsDependency = "customtableitem.{0}|all";
        private const string _childrenNodePathDependency = "node|{0}|{1}|childnodes";
        private const string _singleNodeIdDependency = "nodeid|{0}";
        private const string _classnameDependency = "{0}|all";
        private const string _mediafileDependency = "mediafile|{0}";
        private readonly ISiteContext _siteContext;
        public CacheDependencyFactory(ISiteContext siteContext)
        {
            _siteContext = siteContext;
        }

        public string GetSingleNodeDependency(string aliasPath)
        {
            return string.Format(_singleNodePathDependency, _siteContext.SiteName, aliasPath).ToLower();
        }

        public string GetPageTypeDependency(string className)
        {
            return string.Format(_pageTypeDependency, _siteContext.SiteName, className).ToLower();
        }

        public string GetCustomTableItemsDependency(string className)
        {
            return string.Format(_customTableItemsDependency, className).ToLower();
        }

        public string GetChildrenNodePathDependency(string aliasPath)
        {
            return string.Format(_childrenNodePathDependency, _siteContext.SiteName, aliasPath).ToLower();
        }

        public string GetSingleNodeIdDependency(int id)
        {
            return string.Format(_singleNodeIdDependency, id).ToLower();
        }

        public string GetDependencyByClassName(string className)
        {
            return string.Format(_classnameDependency, className).ToLower();
        }

        public string GetMediaFileDependency(Guid guid)
        {
            return string.Format(_mediafileDependency, guid).ToLower();
        }

        public string GetSingleNodeDependency(Guid guid)
        {
            return string.Format(_singleNodeGuidDependency, _siteContext.SiteName, guid).ToLower();
        }

    }
}
