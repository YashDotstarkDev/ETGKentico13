using System.Text;

namespace Devotion.Cache
{
    public class MultiDependencyBuilder
    {
        private readonly StringBuilder _sb;

        public MultiDependencyBuilder()
        {
            _sb = new StringBuilder();
        }
        
        public MultiDependencyBuilder Add(ICacheBuilder cacheBuilder)
        {
            _sb.Append(cacheBuilder.ToString());
            _sb.AppendLine();
            return this;
        }

        public override string ToString()
        {
            return _sb.ToString().ToLowerInvariant();
        }

        public static implicit operator string(MultiDependencyBuilder multiDependencyBuilder)
        {
            return multiDependencyBuilder.ToString();
        }
    }
}