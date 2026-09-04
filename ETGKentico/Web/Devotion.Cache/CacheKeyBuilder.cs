using System.Collections.Generic;

namespace Devotion.Cache
{
    public class CacheKeyBuilder : ICacheBuilder
    {
        private readonly string _siteName;
        private readonly List<object> _keys;

        public CacheKeyBuilder(string siteName)
        {
            _siteName = siteName;
            _keys = new List<object>();
        }

        public CacheKeyBuilder Append(object key)
        {
            if (_keys.Contains(key))
            {
                return this;
            }

            _keys.Add(key);
            return this;
        }

        public override string ToString()
        {
            return $"{_siteName}|{string.Join("|", _keys)}";
        }

        public static implicit operator string(CacheKeyBuilder cacheKeyBuilder)
        {
            return cacheKeyBuilder.ToString();
        }
    }
}