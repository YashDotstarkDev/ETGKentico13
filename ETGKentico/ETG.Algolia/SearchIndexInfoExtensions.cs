using System;
using CMS.Search;

namespace ETG.Algolia
{
    internal static class SearchIndexInfoExtensions
    {
        public static bool IsAlgoliaIndex(this SearchIndexInfo infoObj)
        {
            return infoObj.IndexProvider.Equals("Algolia", StringComparison.OrdinalIgnoreCase);
        }
    }
}