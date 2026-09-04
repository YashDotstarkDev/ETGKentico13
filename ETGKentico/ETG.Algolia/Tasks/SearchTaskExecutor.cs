using CMS.EventLog;
using CMS.Scheduler;
using CMS.Search;
using ETG.Algolia.Providers;
using System;
using System.Linq;
using CMS.DocumentEngine;
using CMS.Helpers;

namespace ETG.Algolia.Tasks
{
    public class SearchTaskExecutor : ITask
    {
        private ISearchTaskEngine _searchTaskEngine;

        private ISearchTaskEngine SearchTaskEngine
        {
            get => _searchTaskEngine ?? new SearchTaskEngine();
            set => _searchTaskEngine = value;
        }


        public string Execute(TaskInfo task)
        {
            var source = SearchTaskAlgoliaInfoProvider.GetSearchTaskAlgolias()
                .OrderByDescending("SearchTaskAlgoliaPriority")
                .OrderByAscending("SearchTaskAlgoliaID")
                .TopN(SearchManager.TaskProcessingBatchSize);


            while (source.Any())
            {
                foreach (var searchTaskAlgoliaInfo in source)
                {
                    try
                    {
                        if (searchTaskAlgoliaInfo.SearchTaskAlgoliaType == SearchTaskTypeEnum.Rebuild)
                        {
                            if (!searchTaskAlgoliaInfo.SearchTaskAlgoliaAdditionalData.ToLower().Contains("tourpricing"))
                            {
                                SearchTaskEngine.ProcessAlgoliaSearchTask(searchTaskAlgoliaInfo);
                            }
                            else
                            {
                                var tourSearchTaskEngine = new TourSearchTaskEngine();
                            
                                tourSearchTaskEngine.ProcessAlgoliaSearchTask(searchTaskAlgoliaInfo);
                            }    
                        }
                        else
                        {
                            var documentID =
                                ValidationHelper.GetLong(searchTaskAlgoliaInfo.SearchTaskAlgoliaInitiatorObjectID, 0);

                            var document = DocumentHelper.GetDocuments().OnCurrentSite()
                                .WhereEquals("DocumentID", documentID)
                                .WhereEquals("ClassName", "ETG.Tour").FirstOrDefault();

                            if (document != null)
                            {
                                var tourSearchTaskEngine = new TourSearchTaskEngine();
                            
                                tourSearchTaskEngine.ProcessAlgoliaSearchTask(searchTaskAlgoliaInfo);
                            }
                            else
                            {
                                SearchTaskEngine.ProcessAlgoliaSearchTask(searchTaskAlgoliaInfo);
                            }

                        }
                        
                    }
                    catch (Exception ex)
                    {
                        EventLogProvider.LogException("Algolia Search task processor", "PROCESS", ex);
                        searchTaskAlgoliaInfo.SearchTaskAlgoliaErrorMessage = ex.Message;
                        SearchTaskAlgoliaInfoProvider.SetSearchTaskAlgoliaInfo(searchTaskAlgoliaInfo);
                        return null;
                    }

                    SearchTaskAlgoliaInfoProvider.DeleteSearchTaskAlgoliaInfo(searchTaskAlgoliaInfo);
                }

                source.Reset();
            }

            return null;
        }
    }
}