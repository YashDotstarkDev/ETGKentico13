using System;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Devotion.Cache.Common;

namespace Devotion.Cache
{
    public class DependencyBuilder
    {
        private readonly string _siteName;
        private readonly StringBuilder _builder;
        private readonly string[] _classNames;

        public DependencyBuilder(string siteName, string[] classNames)
        {
            _siteName = siteName;
            _classNames = classNames;
            _builder = new StringBuilder();
        }

        [PublicAPI]
        public DependencyBuilder DependsOnAllNodesOfPageType()
        {
            if (_classNames == null)
            {
                throw new InvalidOperationException("Class name cannot be null");
            }

            _classNames.ToList().ForEach(delegate (string className)
            {
                _builder.AppendFormat("nodes|{0}|{1}|all", _siteName, className.Trim());
                _builder.AppendLine();
            });

            return this;
        }

        [PublicAPI]
        public DependencyBuilder DependsOnChildNodesOfAliasPath(string aliasPath)
        {
            _builder.AppendFormat("node|{0}|{1}|childnodes", _siteName, PathUtil.TrimAliasPath(aliasPath));
            _builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public DependencyBuilder DependsOnNodeId(int nodeId)
        {
            _builder.AppendFormat("nodeid|{0}", nodeId);
            _builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public DependencyBuilder DependsOnNodeGuid(Guid nodeGuid)
        {
            _builder.AppendFormat("nodeguid|{0}", nodeGuid);
            _builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public DependencyBuilder DependsOnRootChildNoes()
        {
            _builder.AppendFormat("nodes|{0}|/|childnodes", _siteName);
            _builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public string Build()
        {
            return _builder.ToString().ToLowerInvariant();
        }
    }
}
