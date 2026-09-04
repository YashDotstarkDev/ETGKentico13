using System.Collections.Generic;

namespace ETG.Data.Models.Base
{
    public class SearchResults<T>
    {
        public IEnumerable<T> Results;
        public long TotalCount { get; set; }
    }
}
