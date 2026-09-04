using System;
using System.Collections.Generic;
using System.Linq;
using Algolia.Search.Clients;
using ETG.Algolia;
using ETG.Algolia.Classes;
using ETG.Algolia.IndexStatistics;
using ETG.Algolia.Providers;
using ETG.Algolia.Tasks;
using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Search;

[assembly: RegisterModule(typeof(AlgoliaModule))]

namespace ETG.Algolia
{
    public class AlgoliaModule : CMS.DataEngine.Module
    {
        public AlgoliaModule() : base("ETG.Algolia")
        {
        }

        protected override void OnInit()
        {
            
            base.OnInit();
            SearchIndexInfo.TYPEINFO.Events.Delete.Before += DeleteAlgoliaIndex;
            SearchEvents.SearchTaskCreationHandler.Execute += LogAlgoliaSearchTasks;
            IndexStatisticsProviders.Instance.Register("Algolia", new AlgoliaIndexStatisticsProvider());
            SearchHelper.CreatingDefaultSearchSettings.Execute += SearchFieldsHandlers.CreateDefaultSearchSettings;
            SearchFieldFactory.Instance.Creating.Execute += SearchFieldsHandlers.SetSearchFieldFlags;
            SearchFieldFactory.Instance.CreatingFromSettings.Execute += SearchFieldsHandlers.MapSearchFieldFlags;
            SearchFieldsHelper.Instance.IncludeContentField.Execute += SearchFieldsHandlers.IsContentField;
            SearchFieldsHelper.Instance.IncludeIndexField.Execute += SearchFieldsHandlers.IsIndexField;
            
        }

        private void DeleteAlgoliaIndex(object sender, ObjectEventArgs e)
        {
            var indexInfo = e.Object as SearchIndexInfo;
            if (indexInfo == null || !indexInfo.IsAlgoliaIndex())
            {
                return;
            }

            try
            {
                var client = new SearchClient(indexInfo.IndexSearchServiceName, indexInfo.IndexAdminKey);
                var index = client.InitIndex(indexInfo.IndexName);

                index.Delete();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogEvent(new EventLogInfo
                {
                    EventType = "W",
                    Exception = ex,
                    Source = "Algolia Search provider",
                    EventCode = "DELETE INDEX",
                    EventDescription = string.Format("An error occurred when deleting index '{0}' on Algolia. If the index exists, it has to be deleted manually.", indexInfo.IndexName)
                });
            }
        }

        private void LogAlgoliaSearchTasks(object sender, SearchTaskCreationEventArgs e)
        {
            var isAlgoliaTask = false;
            foreach (var parameter in e.Parameters)
            {
                if (IsAlgoliaSearchTask(parameter))
                {
                    isAlgoliaTask = true;

                    SearchTaskAlgoliaInfoProvider.SetSearchTaskAlgoliaInfo(new SearchTaskAlgoliaInfo
                    {
                        SearchTaskAlgoliaAdditionalData = parameter.TaskValue,
                        SearchTaskAlgoliaInitiatorObjectID = parameter.RelatedObjectID,
                        SearchTaskAlgoliaMetadata = parameter.ObjectField,
                        SearchTaskAlgoliaObjectType = parameter.ObjectType,
                        SearchTaskAlgoliaType = parameter.TaskType
                    });
                }
            }

            if (!isAlgoliaTask || !SearchIndexInfoProvider.SearchEnabled || !CMSActionContext.CurrentEnableSmartSearchIndexer)
            {
                return;
            }
/*
            if (CMSTransactionScope.IsInTransaction)
            {
                ConnectionContext.CurrentConnectionScope.CallOnDispose(SearchTaskExecutorUtils.ProcessSearchTasks);
            }
            else
            {
                SearchTaskExecutorUtils.ProcessSearchTasks();
            }*/
        }

        private bool IsAlgoliaSearchTask(SearchTaskCreationParameters searchTaskCreationParameters)
        {
            if (searchTaskCreationParameters.TaskType == SearchTaskTypeEnum.Rebuild)
            {
                var searchIndexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(searchTaskCreationParameters.RelatedObjectID);
                if (searchIndexInfo == null || !searchIndexInfo.IsAlgoliaIndex())
                {
                    return false;
                }
            }
            else if (searchTaskCreationParameters.TaskType == SearchTaskTypeEnum.Process &&
                     string.Equals(searchTaskCreationParameters.ObjectType, "cms.customtable", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }

    public class SearchFieldsHandlers
    {
        private static readonly string[] MappedSearchFieldFlags = {
            AlgoliaSearchFieldFlags.SEARCHABLE,
            AlgoliaSearchFieldFlags.RETRIEVABLE
        };

        public static void CreateDefaultSearchSettings(
            object sender,
            CreateDefaultSearchSettingsEventArgs eventArgs)
        {/*
            SearchSettingsInfo searchSettings = eventArgs.SearchSettings;
            searchSettings.SetFlag(AlgoliaSearchFieldFlags.CONTENT, eventArgs.DataType == typeof(string));
            searchSettings.SetFlag(AlgoliaSearchFieldFlags.SEARCHABLE,  false);
            searchSettings.SetFlag(AlgoliaSearchFieldFlags.RETRIEVABLE, false);*/
        }

        public static void SetSearchFieldFlags(object sender, CreateSearchFieldEventArgs eventArgs)
        {
            /*
            switch (eventArgs.CreateOption)
            {
                case CreateSearchFieldOption.SearchableAndRetrievable:
                    break;
                case CreateSearchFieldOption.SearchableWithTokenizer:
                    eventArgs.SearchField.SetFlag(AlgoliaSearchFieldFlags.SEARCHABLE, eventArgs.SearchField.GetFlag(AlgoliaSearchFieldFlags.SEARCHABLE));
                    eventArgs.SearchField.SetFlag(AlgoliaSearchFieldFlags.RETRIEVABLE, eventArgs.SearchField.GetFlag(AlgoliaSearchFieldFlags.RETRIEVABLE));
                    break;
                case CreateSearchFieldOption.SearchableAndRetrievableWithTokenizer:
                    eventArgs.SearchField.SetFlag(AlgoliaSearchFieldFlags.SEARCHABLE, IsAlgoliaSearchableType(eventArgs.SearchField.DataType));
                    break;
            }*/
        }

        public static bool IsAlgoliaSearchableType(Type type)
        {
            if (!(type == typeof(string)))
                return typeof(IEnumerable<string>).IsAssignableFrom(type);
            return true;
        }

        public static void MapSearchFieldFlags(
            object sender,
            CreateSearchFieldFromSettingsEventArgs eventArgs)
        {
            ISearchField searchField = eventArgs.SearchField;
            SearchSettingsInfo searchSettings = eventArgs.SearchSettings;
            foreach (string mappedSearchFieldFlag in SearchFieldsHandlers.MappedSearchFieldFlags)
                searchField.SetFlag(mappedSearchFieldFlag, searchSettings.GetFlag(mappedSearchFieldFlag));
        }

        public static void IsContentField(object sender, IsContentFieldEventArgs eventArgs)
        {
            bool flag = eventArgs.SearchSettings.GetFlag(AlgoliaSearchFieldFlags.CONTENT);
            if (eventArgs.Index == null)
            {
                eventArgs.Result |= flag;
            }
            else
            {
                if (!eventArgs.Index.IndexProvider.Equals("Algolia", StringComparison.OrdinalIgnoreCase))
                    return;
                eventArgs.Result = flag;
            }
        }

        public static void IsIndexField(object sender, IsIndexFieldEventArgs eventArgs)
        {
            bool flag1 = ((IEnumerable<string>) MappedSearchFieldFlags).Any(flag => eventArgs.SearchSettings.GetFlag(flag));
            if (eventArgs.Index == null)
            {
                eventArgs.Result |= flag1;
            }
            else
            {
                if (!eventArgs.Index.IndexProvider.Equals("Algolia", StringComparison.OrdinalIgnoreCase))
                    return;
                eventArgs.Result = flag1;
            }
        }
    }
}