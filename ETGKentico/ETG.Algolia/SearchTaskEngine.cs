using Algolia.Search.Clients;
using CMS.DataEngine;
using CMS.Search;
using CMS.Search.Internal;
using ETG.Algolia.Classes;
using ETG.Algolia.Factories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.Core.Internal;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.Search.Azure;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using Newtonsoft.Json;

namespace ETG.Algolia
{
    public class SearchTaskEngine : ISearchTaskEngine
    {


        public void ProcessAlgoliaSearchTask(SearchTaskAlgoliaInfo task)
        {
            switch (task.SearchTaskAlgoliaType)
            {
                case SearchTaskTypeEnum.Update:
                    ExecuteUpdateTask(task);
                    break;
                case SearchTaskTypeEnum.Delete:
                    ExecuteDeleteTask(task);
                    break;
                case SearchTaskTypeEnum.Rebuild:
                    ExecuteRebuildTask(task);
                    break;
                case SearchTaskTypeEnum.Process:
                    //this.ExecuteProcessTask(task);
                    break;
            }
        }

        private void ExecuteDeleteTask(SearchTaskAlgoliaInfo task)
        {
            foreach (DataRow row in (InternalDataCollectionBase)SearchablesRetrievers.Get(task.SearchTaskAlgoliaObjectType).GetRelevantIndexes(task.SearchTaskAlgoliaObjectType, "Algolia").Tables[0].Rows)
            {
                SearchIndexInfo searchIndexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(ValidationHelper.GetInteger(row["IndexID"], 0, null));
                if (searchIndexInfo != null)
                { 
                    ExecuteDeleteTaskForIndex(task, searchIndexInfo);
                }
            }

        }

        private void ExecuteUpdateTask(SearchTaskAlgoliaInfo task)
        {
            foreach (KeyValuePair<SearchIndexInfo, IEnumerable<ISearchable>> searchableObject in GetIndexesWithSearchableObjects(task))
            {
                ExecuteUpdateOnIndex(searchableObject.Key, searchableObject.Value, task.SearchTaskAlgoliaAdditionalData);
            }
        }

        private void ExecuteUpdateOnIndex(SearchIndexInfo indexInfo, IEnumerable<ISearchable> searchables, string objectId)
        {
            var client = new SearchClient(indexInfo.IndexSearchServiceName, indexInfo.IndexAdminKey);
            var algoliaIndex = client.InitIndex(indexInfo.IndexName);

            UpdateIndex(indexInfo, searchables, algoliaIndex, objectId);
        }

        private void ExecuteDeleteTaskForIndex(SearchTaskAlgoliaInfo task, SearchIndexInfo indexInfo)
        {
            var client = new SearchClient(indexInfo.IndexSearchServiceName, indexInfo.IndexAdminKey);
            var algoliaIndex = client.InitIndex(indexInfo.IndexName);

            algoliaIndex.DeleteObject(task.SearchTaskAlgoliaAdditionalData);
        }


        private static Dictionary<SearchIndexInfo, IEnumerable<ISearchable>> GetIndexesWithSearchableObjects(
            SearchTaskAlgoliaInfo task)
        { 
            return GetIndexesWithSearchableObjectsForDocumentUpdate(task.SearchTaskAlgoliaObjectType, 
                ValidationHelper.GetInteger((object)task.SearchTaskAlgoliaInitiatorObjectID, 0, null));
            
        }

        /// <summary>
        /// Returns collection of Azure indexes with all the related <see cref="T:CMS.DataEngine.ISearchable" /> objects for given <paramref name="objectType" /> and <paramref name="documentId" />
        /// that are necessary for document update.
        /// </summary>
        private static Dictionary<SearchIndexInfo, IEnumerable<ISearchable>> GetIndexesWithSearchableObjectsForDocumentUpdate(
            string objectType,
            int documentId)
        {
            List<KeyValuePair<SearchIndexInfo, ISearchable>> source = new List<KeyValuePair<SearchIndexInfo, ISearchable>>();
            foreach (ISearchable searchable1 in SearchIndexers.GetIndexer(objectType).SelectSearchDocument(documentId))
            {
                ISearchable searchable = searchable1;
                source.AddRange(SearchIndexInfoProvider.GetRelevantIndexes(searchable, "Algolia").Select<SearchIndexInfo, KeyValuePair<SearchIndexInfo, ISearchable>>((Func<SearchIndexInfo, KeyValuePair<SearchIndexInfo, ISearchable>>)(index => new KeyValuePair<SearchIndexInfo, ISearchable>(index, searchable))));
            }
            return source.GroupBy(kvp => kvp.Key, kvp => kvp.Value).ToDictionary<IGrouping<SearchIndexInfo, ISearchable>, SearchIndexInfo, IEnumerable<ISearchable>>((Func<IGrouping<SearchIndexInfo, ISearchable>, SearchIndexInfo>)(group => group.Key), (Func<IGrouping<SearchIndexInfo, ISearchable>, IEnumerable<ISearchable>>)(group => group.AsEnumerable<ISearchable>()));
        }

        /// <summary>
        /// Returns collection of Azure indexes with all the related <see cref="T:CMS.DataEngine.ISearchable" /> objects for given <paramref name="objectType" /> and <paramref name="documentId" />
        /// that are necessary for document update.
        /// </summary>
        private static Dictionary<SearchIndexInfo, IEnumerable<ISearchable>> GetIndexesWithSearchableObjectsForDocumentDelete(
            string objectType,
            int documentId)
        {
            List<KeyValuePair<SearchIndexInfo, ISearchable>> source = new List<KeyValuePair<SearchIndexInfo, ISearchable>>();
            foreach (ISearchable searchable1 in SearchIndexers.GetIndexer(objectType).SelectSearchDocument(documentId))
            {
                ISearchable searchable = searchable1;
                source.AddRange(SearchIndexInfoProvider.GetRelevantIndexes(searchable, "Algolia").Select<SearchIndexInfo, KeyValuePair<SearchIndexInfo, ISearchable>>((Func<SearchIndexInfo, KeyValuePair<SearchIndexInfo, ISearchable>>)(index => new KeyValuePair<SearchIndexInfo, ISearchable>(index, searchable))));
            }
            return source.GroupBy(kvp => kvp.Key, kvp => kvp.Value).ToDictionary<IGrouping<SearchIndexInfo, ISearchable>, SearchIndexInfo, IEnumerable<ISearchable>>((Func<IGrouping<SearchIndexInfo, ISearchable>, SearchIndexInfo>)(group => group.Key), (Func<IGrouping<SearchIndexInfo, ISearchable>, IEnumerable<ISearchable>>)(group => group.AsEnumerable<ISearchable>()));
        }
        private void ExecuteRebuildTask(SearchTaskAlgoliaInfo task)
        {
            SearchIndexInfo searchIndexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(task.SearchTaskAlgoliaInitiatorObjectID);
            if (searchIndexInfo == null || !(searchIndexInfo.ActualRebuildTime <= task.SearchTaskAlgoliaCreated))
            {
                return;
            }

            SearchIndexInfoProvider.SetIndexStatus(searchIndexInfo, IndexStatusEnum.REBUILDING);
            Rebuild(searchIndexInfo);
            if (SearchIndexInfoProvider.GetIndexStatus(searchIndexInfo) == IndexStatusEnum.ERROR)
            {
                return;
            }

            SearchIndexInfoProvider.SetIndexStatus(searchIndexInfo, IndexStatusEnum.READY);
            SearchIndexInfoProvider.SetIndexFilesLastUpdateTime(searchIndexInfo, DateTime.Now);
        }

        private void Rebuild(SearchIndexInfo indexInfo)
        {
            var client = new SearchClient(indexInfo.IndexSearchServiceName, indexInfo.IndexAdminKey);
            var algoliaIndex = client.InitIndex(indexInfo.IndexName);

            try
            {
                algoliaIndex.ClearObjects();

                var searchables = SearchablesRetrievers.Get(indexInfo.IndexType).GetSearchableObjects(indexInfo);
                UpdateIndex(indexInfo, searchables, algoliaIndex);

                SearchHelper.FinishRebuild(indexInfo);
            }
            catch (Exception ex)
            {
                SearchIndexInfoProvider.SetIndexStatus(indexInfo, IndexStatusEnum.ERROR);
                throw;
            }
        }


        private void UpdateIndex(SearchIndexInfo indexInfo,
            IEnumerable<ISearchable> searchables, SearchIndex algoliaIndex, string objectId = "")
        {
            if (searchables == null)
            {
                return;
            }
            List<JObject> indexActionList = new List<JObject>();

            int num = 0;

            ISearchFields searchFields = null;
            using (IEnumerator<ISearchable> enumerator = searchables.GetEnumerator())
            {
                bool flag2 = enumerator.MoveNext();
                while (flag2)
                {
                    ISearchable current = enumerator.Current;
                    var searchDocument = current.GetSearchDocument(indexInfo);
                    
                    if (ShouldDelete(searchDocument))
                    {
                        if (!objectId.IsNullOrEmpty())
                        {
                            algoliaIndex.DeleteObject(objectId);
                            return;
                        }
                        
                        flag2 = enumerator.MoveNext();

                        if (!flag2)
                        {
                            algoliaIndex.SaveObjects(indexActionList);
                        }
                        continue;
                        
                    }
                   
                    searchFields = current.GetSearchFields(indexInfo);
                    var searchDocumentCreator = SearchDocumentCreatorFactory.GetDocumentCreator(indexInfo.IndexCodeName, searchDocument, searchFields);
                    
                    JObject indexAction = searchDocumentCreator.CreateDocument();

                    // Special fields for tour objects
                    if (indexAction["classname"]?.ToString()?.Equals("etg.tour") ?? false)
                    {
                        var primaryCountryGuid = indexAction["TourPrimaryCountry"]?.ToString();
                        var subCountryGuids = indexAction["TourSubCountries"]?.ToString();
                        var nodeId = indexAction["NodeID"]?.ToInteger(0);

                        // Primary country names
                        if (!string.IsNullOrWhiteSpace(primaryCountryGuid))
                        {
                            var primaryCountryDestination = DestinationProvider
                                .GetDestinations()
                                .TopN(1)
                                .OnCurrentSite()
                                .WhereEquals(nameof(Destination.NodeGUID), primaryCountryGuid)
                                .FirstOrDefault();

                            if (primaryCountryDestination != null)
                            {
                                indexAction["TourPrimaryCountryName"] = JToken.FromObject(primaryCountryDestination.DestinationName);
                            }
                            else
                            {
                                indexAction["TourPrimaryCountryName"] = JToken.FromObject("");
                            }
                        }
                        else
                        {
                            indexAction["TourPrimaryCountryName"] = JToken.FromObject("");
                        }

                        // Sub country names
                        if (!string.IsNullOrWhiteSpace(subCountryGuids))
                        {
                            var subCountryDestination = DestinationProvider
                                .GetDestinations()
                                .OnCurrentSite()
                                .WhereIn(nameof(Destination.NodeGUID), subCountryGuids.Split(';'))
                                .ToList();

                            if (subCountryDestination?.Any()?? false)
                            {
                                indexAction["TourSubCountryNames"] = JToken.FromObject(subCountryDestination.Select(x => x.DestinationName));
                            }
                            else
                            {
                                indexAction["TourSubCountryNames"] = JToken.FromObject(Enumerable.Empty<string>());
                            }
                        }
                        else
                        {
                            indexAction["TourSubCountryNames"] = JToken.FromObject(Enumerable.Empty<string>());
                        }
                        
                        
                        if (nodeId > 0)
                        {
                            var nodePath = DocumentHelper
                                .GetDocuments<TreeNode>()
                                .WhereEquals(nameof(TreeNode.NodeID), nodeId)
                                .FirstOrDefault()
                                ?.NodeAliasPath;

                            if (!string.IsNullOrWhiteSpace(nodePath))
                            {
                                // Main hotel name
                                var mainLink = DocumentLinkProvider
                                    .GetDocumentLinks()
                                    .TopN(1)
                                    .Path(nodePath, PathTypeEnum.Section)
                                    .WhereTrue(nameof(DocumentLink.LinkDocumentIsMain))
                                    .FirstOrDefault();

                                if (mainLink != null)
                                {
                                    indexAction["MainHotelName"] = JToken.FromObject(mainLink.DocumentName);
                                }
                                else
                                {
                                    indexAction["MainHotelName"] = JToken.FromObject("");
                                }
                                
                                // Highlighted tour inclusions
                                var highlightedInclusions = TourInclusionProvider
                                    .GetTourInclusions()
                                    .Path(nodePath, PathTypeEnum.Section)
                                    .WhereTrue(nameof(TourInclusion.TourInclusionHighlight))
                                    .ToList();

                                if (highlightedInclusions?.Any()?? false)
                                {
                                    indexAction["TourHighlights"] = JToken.FromObject(highlightedInclusions.Select(x => x.TourInclusionLabel));
                                }
                                else
                                {
                                    indexAction["TourHighlights"] = JToken.FromObject(Enumerable.Empty<string>());
                                }
                            }
                            else
                            {
                                indexAction["MainHotelName"] = JToken.FromObject("");
                                indexAction["TourHighlights"] = JToken.FromObject(Enumerable.Empty<string>());
                            }
                        }
                        else
                        {
                            indexAction["MainHotelName"] = JToken.FromObject("");
                            indexAction["TourHighlights"] = JToken.FromObject(Enumerable.Empty<string>());
                        }

                        
                        
                        indexAction.Remove("objectID");
                    }

                    ++num;
                    indexActionList.Add(indexAction);
                    //if (indexAction.ActionType != IndexActionType.Delete)
                    //{

                    // this.MergeFields(
                    //            DocumentFieldCreator.Instance.CreateFields(current, (ISearchIndexInfo) indexInfo).Where<Field>((Func<Field, bool>) (f =>
                    //                indexAction.Document.Keys.Contains<string>(f.Name, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase))), azureIndexFields) | flag1;
                    //}

                    if (!(flag2 = enumerator.MoveNext()) || num == Math.Min(indexInfo.IndexBatchSize, 500)) // Batch size of 500
                    {

                        algoliaIndex.SaveObjects(indexActionList);
                        num = 0;
                        indexActionList = new List<JObject>();
                    }
                }
            }
            
        }

        private bool ShouldDelete(SearchDocument searchDocument)
        {
            if (searchDocument.Names.Contains("DocumentIsArchived") && ValidationHelper.GetBoolean(searchDocument.GetValue("DocumentIsArchived"), false))
            {
                return true;
            }

            var nodeAliasPath = "";//
            if (searchDocument.Names.Contains("NodeAliasPath") )
            {
                nodeAliasPath = ValidationHelper.GetString(searchDocument.GetValue("NodeAliasPath"), string.Empty);
            }     

            if (!nodeAliasPath.IsNullOrEmpty() && nodeAliasPath.ToLower().Contains("/tour-folder/"))
            {
                var upgradeType = ValidationHelper.GetInteger(searchDocument.GetValue("TourIsUpgrade"), 0);

                if (upgradeType == 2)
                {
                    return true;
                }
            }
            if (searchDocument.Names.Contains("DocumentPublishFrom") &&
                searchDocument.Names.Contains("DocumentPublishTo"))
            {
                var publishFrom = ValidationHelper.GetDateTime(searchDocument.GetValue("DocumentPublishFrom"),
                    DateTime.MinValue);
                var publishTo = ValidationHelper.GetDateTime(searchDocument.GetValue("DocumentPublishTo"),
                    DateTime.MinValue);

                if (publishFrom == DateTime.MinValue && publishTo == DateTime.MinValue)
                {
                    return false;
                }

                if (publishTo == DateTime.MinValue)
                {
                    if (DateTime.Now >= publishFrom )
                    {
                        return false;
                    }

                }
                else 
                {
                    if (DateTime.Now >= publishFrom && DateTime.Now <= publishTo)
                    {
                        return false;
                    }
                }

                return true;

            }

            if (searchDocument.Names.Contains("DocumentSearchExcluded") &&
                ValidationHelper.GetBoolean(searchDocument.GetValue("DocumentSearchExcluded"), false))
            {
                return true;
            }


            return false;
        }

        private void DeleteObjectFromIndex(SearchIndexInfo indexInfo,
            IEnumerable<ISearchable> searchables, SearchIndex algoliaIndex)
        {
            if (searchables == null)
            {
                return;
            }
            List<JObject> indexActionList = new List<JObject>();

            int num = 0;

            ISearchFields searchFields = null;
            using (IEnumerator<ISearchable> enumerator = searchables.GetEnumerator())
            {



                bool flag2 = enumerator.MoveNext();
                while (flag2)
                {

                    ++num;
                    ISearchable current = enumerator.Current;

                    var searchDocument = current.GetSearchDocument(indexInfo);
                    if (!searchDocument.Names.Contains("_id"))
                    {
                        continue;
                    }
                    var objectId = searchDocument.GetValue("_id");

                    if (!(flag2 = enumerator.MoveNext()) || num == Math.Min(indexInfo.IndexBatchSize, 500)) // Batch size of 500
                    {

                        algoliaIndex.DeleteObject(objectId.ToString());
                        num = 0;
                        indexActionList = new List<JObject>();
                    }
                }
            }

        }
    }
}