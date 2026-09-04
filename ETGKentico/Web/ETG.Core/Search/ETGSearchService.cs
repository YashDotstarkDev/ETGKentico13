using System.Linq;
using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Search;

namespace ETG.Core.Search
{
    public class ETGSearchService : IETGSearchService
    {
        public string RebuildTourSearchIndex()
        {
            var tourIndexName = SettingsKeyInfoProvider.GetValue("SearchIndexTour");

            if (tourIndexName.IsNullOrEmpty())
            {
                return "Search Tour Index Settings not found";
            }
            
            var tourIndex = SearchIndexInfoProvider.GetSearchIndexes().WhereEquals(nameof(SearchIndexInfo.IndexName), tourIndexName ).FirstOrDefault();
                    
            if (tourIndex != null)
            {
                SearchTaskInfoProvider.CreateTask(SearchTaskTypeEnum.Rebuild, null, null, tourIndex.IndexName, tourIndex.IndexID);
               
                return string.Empty;
            }

            return "Search Tour Index not found";
        }
    }
}