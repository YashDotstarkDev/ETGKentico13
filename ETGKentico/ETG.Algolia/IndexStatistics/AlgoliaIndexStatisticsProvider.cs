using System;
using System.Linq;
using Algolia.Search.Clients;
using Algolia.Search.Models.Common;
using CMS.EventLog;
using CMS.Search;

namespace ETG.Algolia.IndexStatistics
{
    public class AlgoliaIndexStatisticsProvider : IIndexStatisticsProvider
    {
        public IIndexStatistics GetStatistics(SearchIndexInfo indexInfo)
        {
            var client = new SearchClient(indexInfo.IndexSearchServiceName, indexInfo.IndexAdminKey);

            ListIndicesResponse indices;
            
            try
            {
                indices = client.ListIndices();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("AlgoliaIndexStatisticsProvider", "GetStatistics", ex);
                return null;
            }
            
            var index = indices.Items.FirstOrDefault(x => x.Name.Equals(indexInfo.IndexName, StringComparison.OrdinalIgnoreCase));
            if (index == null)
            {
                return null;
            }

            return new AlgoliaIndexStatistics
            {
                DocumentCount = index.Entries,
                Size = index.DataSize
            };
        }
    }
}