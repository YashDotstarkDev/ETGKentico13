using System.Text;
using JetBrains.Annotations;
using CMS.Helpers;

namespace Devotion.Cache
{
    public class CacheDependencyBuilder
    {
        private readonly StringBuilder _builder;

        public CacheDependencyBuilder()
        {
            _builder = new StringBuilder();
        }

        [PublicAPI]
        public CacheDependencyBuilder AddNode(string nodeId)
        {
            if (!string.IsNullOrWhiteSpace(nodeId))
            {
                Add($"nodeid|{nodeId}");
            }

            return this;
        }

        [PublicAPI]
        public CacheDependencyBuilder AddNode(string nodeAliasPath, string siteName)
        {
            if (!string.IsNullOrWhiteSpace(nodeAliasPath))
            {
                Add($"node|{siteName.ToLower()}|{nodeAliasPath}|childnodes");
            }

            return this;
        }

        [PublicAPI]
        public CacheDependencyBuilder Add(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                _builder.AppendLine(key.ToLower());
            }

            return this;
        }

        [PublicAPI]
        public CMSCacheDependency Build()
        {
            return CacheHelper.GetCacheDependency(_builder.ToString());
        }
    }
}
