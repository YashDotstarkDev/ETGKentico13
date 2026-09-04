using CMS.Search;

namespace ETG.Algolia
{
    public class AlgoliaIndexStatistics : IIndexStatistics
    {
        public long DocumentCount { get; set; }
        public long Size { get; set; }
    }
}