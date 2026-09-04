using System;
using System.Text;
using JetBrains.Annotations;

namespace Devotion.Cache
{
    public class ObjectDependencyBuilder<T> : ICacheBuilder where T : class, new()
    {
        private readonly StringBuilder _builder;
        private readonly string _className;

        [PublicAPI]
        public ObjectDependencyBuilder()
        {
            _builder = new StringBuilder();
            var classNameObj = typeof(T).GetField("OBJECT_TYPE").GetValue(null);
            if (classNameObj != null)
            {
                _className = classNameObj.ToString();
            }
        }

        [PublicAPI]
        public ObjectDependencyBuilder(string className)
        {
            _builder = new StringBuilder();
            _className = className;
        }

        [PublicAPI]
        public ObjectDependencyBuilder<T> DependsOnAll()
        {
            if (_className == null)
            {
                throw new InvalidOperationException("Class name can not be null");
            }

            _builder.AppendFormat("{0}|all", _className.Trim());
            _builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public ObjectDependencyBuilder<T> DependsOnId(int id)
        {
            if (_className == null)
            {
                throw new InvalidOperationException("OBJECT_TYPE can not be null");
            }

            _builder.AppendFormat("{0}|byid|{1}", _className, id);
            _builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public ObjectDependencyBuilder<T> DependsOnValue(string value)
        {
            if (_className == null)
            {
                throw new InvalidOperationException("Class name can not be null");
            }

            _builder.AppendFormat("{0}|{1}", _className, value);
            _builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public ObjectDependencyBuilder<T> DependsOnGuid(Guid guid)
        {
            if (_className == null)
            {
                throw new InvalidOperationException("OBJECT_TYPE can not be null");
            }

            _builder.AppendFormat("{0}|byguid|{1}", _className, guid);
            _builder.AppendLine();
            return this;
        }

        [PublicAPI]
        public ObjectDependencyBuilder<T> DependsOnName(string value)
        {
            if (_className == null)
            {
                throw new InvalidOperationException("OBJECT_TYPE can not be null");
            }

            _builder.AppendFormat("{0}|byname|{1}", _className, value);
            _builder.AppendLine();
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString().ToLowerInvariant();
        }

        public static implicit operator string(ObjectDependencyBuilder<T> dependencyBuilder)
        {
            return dependencyBuilder.ToString();
        }
    }
}
