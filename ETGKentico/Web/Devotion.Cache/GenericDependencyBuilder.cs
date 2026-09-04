using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Devotion.Cache.Common;

namespace Devotion.Cache
{
    public class GenericDependencyBuilder<T> where T : class, new()
    {
        private readonly string _siteName;
        private readonly StringBuilder _builder;

        public GenericDependencyBuilder(string siteName)
        {
            _siteName = siteName;
            _builder = new StringBuilder();
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnAllNodesOfPageType()
        {
            var classNames = typeof(T).GetField("CLASS_NAME").GetValue(null);
            if (classNames == null)
            {
                throw new InvalidOperationException("Class name cannot be null");
            }

            classNames.ToString().Split(',').ToList().ForEach(delegate (string className)
            {
                _builder.AppendFormat("nodes|{0}|{1}|all", _siteName, className.Trim());
                //builder.AppendLine();
            });

            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnAllNodesOfPageTypeAndOrder()
        {
            var classNames = typeof(T).GetField("CLASS_NAME").GetValue(null);
            if (classNames == null)
            {
                throw new InvalidOperationException("Class name cannot be null");
            }

            classNames.ToString().Split(',').ToList().ForEach(delegate (string className)
            {
                _builder.AppendFormat("nodes|{0}|{1}|all", _siteName, className.Trim());
                _builder.AppendLine();
                _builder.AppendFormat("nodeorder");
            });

            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnChildNodesOfAliasPath(string aliasPath, string culture = "")
        {
            if (string.IsNullOrEmpty(culture))
            {
                _builder.AppendFormat("node|{0}|{1}|childnodes", _siteName, PathUtil.TrimAliasPath(aliasPath));
            }
            else
            {
                _builder.AppendFormat("node|{0}|{1}|childnodes|{2}", _siteName, PathUtil.TrimAliasPath(aliasPath), culture);
            }
            //builder.AppendLine();

            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnNodeAliasPath(string aliasPath)
        {
            _builder.Append($"node|{_siteName}|{PathUtil.TrimAliasPath(aliasPath)}");
            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnNodeId(int nodeId)
        {
            _builder.AppendFormat("nodeid|{0}", nodeId);
            //builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnNodeGuid(Guid nodeGuid)
        {
            _builder.AppendFormat("nodeguid|{0}", nodeGuid);
            //builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnNodeGuids(List<Guid> nodeGuids)
        {
            foreach (var nodeGuid in nodeGuids)
            {
                DependsOnNodeGuid(nodeGuid);
            }

            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnRootChildNodes()
        {
            _builder.AppendFormat("nodes|{0}|/|childnodes", _siteName);
            //builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnDocumentId(int documentId)
        {
            _builder.AppendFormat("documentid|{0}", documentId);
            //builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnCustomTable(string className)
        {
            _builder.AppendFormat("customtableitem.{0}|all", className);
            //builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public GenericDependencyBuilder<T> DependsOnAllObjects(string className)
        {
            _builder.AppendFormat("{0}|all", className);
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString().ToLowerInvariant();
        }

        public static implicit operator string(GenericDependencyBuilder<T> dependencyBuilder)
        {
            return dependencyBuilder.ToString();
        }
    }
}
