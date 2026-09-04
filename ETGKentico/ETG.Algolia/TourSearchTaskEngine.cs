using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using Castle.Core.Internal;
using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Search;
using CMS.Search.Internal;
using CommonServiceLocator;
using Devotion.Cache;
using ETG.Algolia.Classes;
using ETG.Algolia.SearchDocumentCreators;
using ETG.Algolia.SearchDocumentCreators.SearchDocumentModels;
using ETG.Booking.Pricing.Repositories;
using ETG.Core.Promotion;
using ETG.Data.Models.Booking;
using ETG.Data.Promotion;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ETG.Algolia
{
    public class TourSearchTaskEngine : ISearchTaskEngine
    {
        public bool RebuildMode = false;

        public TourSearchTaskEngine()
        {
            
        }

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
                RebuildMode = true;
                var searchables = SearchablesRetrievers.Get(indexInfo.IndexType).GetSearchableObjects(indexInfo);
                UpdateIndex(indexInfo, searchables,  algoliaIndex);

                SearchHelper.FinishRebuild(indexInfo);
            }
            catch (Exception ex)
            {
                SearchIndexInfoProvider.SetIndexStatus(indexInfo, IndexStatusEnum.ERROR);
                throw;
            }
        }


        private void UpdateIndex(SearchIndexInfo indexInfo,
            IEnumerable<ISearchable> searchables,  SearchIndex algoliaIndex, string objectId = "")
        {
            if (searchables == null)
            {
                return;
            }
            
            ISearchFields searchFields = null;
            using (IEnumerator<ISearchable> enumerator = searchables.GetEnumerator())
            {
                bool flag2 = enumerator.MoveNext();
                while (flag2)
                {
                    ISearchable current = enumerator.Current;
                    var searchDocument = current.GetSearchDocument(indexInfo);

                    if (!RebuildMode)
                    {
                        //search for all records for specific tour and delete in algolia

                        var ids = searchDocument.GetValue("_id").ToString();

                        var code = ids.Split(';')[1];
                        var objectIds = GetToursInAlgoliaForDeletion(code, algoliaIndex);

                        foreach (var id in objectIds)
                        {
                            algoliaIndex.DeleteObject(id.ToString());
                        }
                    }

                    if (ShouldDelete(searchDocument))
                    {    
                        flag2 = enumerator.MoveNext();
                        continue;
                    }
                   
                    searchFields = current.GetSearchFields(indexInfo);
                    var searchDocumentCreator = new TourSearchDocumentCreator( searchDocument, searchFields);
                    
                    JObject mainDocument = searchDocumentCreator.CreateDocument();
                    
                    var tourSearchObjects = GetAllTourRecords(searchDocument, mainDocument);    
                    
                    try{
                    
                        algoliaIndex.SaveObjects(tourSearchObjects);
                    }catch(Exception ex){
                        EventLogProvider.LogException("Algolia", "Save", ex );
                    }
                    flag2 = enumerator.MoveNext();
                }
            }
            
        }

        private bool IsOnSale(SearchDocument searchDocument, TourModel tour, DateTime dt, out string discountText, out PromotionInfo promotion)
        {
            discountText = string.Empty;
            var promotionRepository = ServiceLocator.Current.GetInstance<IPromotionRepository>();
            promotion = promotionRepository.GetPromotionInfoForNonAgent(tour, dt);

            if (promotion != null)
            {
                discountText = promotion.PromotionName;
                return true;
            }

            var discountGuid =
                ValidationHelper.GetGuid(searchDocument.GetValue("TourDiscountAndOffer"), Guid.Empty);

            if (discountGuid != Guid.Empty)
            {

                var discountService =
                    ServiceLocator.Current
                        .GetInstance<IDiscountService>(); //ServiceFactory.GetDiscountServiceObject();
                var discount = discountService.GetDiscount(discountGuid);
                if (discount != null)
                {
                    discountText = discount.DisplayAsLabel ? discount.DisplayLabel : "";
                    return discount.IsOnSaleNow;
                }
            }

            return false;
        }
        
        private TourOptions GetTourOptions(SearchDocument searchDocument, TourModel tour, DateTime dt, out string discountText, out PromotionInfo promotion)
        {
            var options = new TourOptions();
            
            options.BookNow = ValidationHelper.GetBoolean(searchDocument.GetValue("TourBookNowEnabled"), false);
            options.FreedomOfChoice = ValidationHelper.GetBoolean(searchDocument.GetValue("TourHasFreedomOfChoice"), false);
            options.ExclusivePackages = ValidationHelper.GetBoolean(searchDocument.GetValue("TourIsExclusive"), false);

            var isOnSale = IsOnSale(searchDocument, tour, dt, out discountText, out promotion);
            
            options.OnSale = isOnSale;
            options.PeaceOfMind = ValidationHelper.GetBoolean(searchDocument.GetValue("TourHasPeaceOfMindGuarantee"), false);
            options.SafeTravels = ValidationHelper.GetBoolean(searchDocument.GetValue("TourHasSafeTravel"), false);

            return options;
        }
        private List<JObject> GetAllTourRecords(SearchDocument searchDocument, JObject mainDocument)
        {
            var bookingPriceRepository = new BookingPriceRepository(new KenticoCacheProvider());

            var tourCode = mainDocument["code"].ToString();
            var tourService = ServiceLocator.Current.GetInstance<ITourService>();
            var tour = tourService.GetTourByTourCode(tourCode);

            if (tour == null)
            {
                return new List<JObject>();
            }
            var prices = bookingPriceRepository.GetDepartureBookingPrices(tourCode,DateTime.Today, null);

            var allPricesRecords = new List<JObject>();

            if (prices.IsNullOrEmpty())
            {
                // mainDocument["fromPrice"] = mainDocument["price"];
                mainDocument["objectID"] = Guid.NewGuid();
                var options = GetTourOptions(searchDocument, tour, DateTime.Today, out var discountText, out var promotion);
                mainDocument["promotionTitle"] = JToken.FromObject(discountText);
                mainDocument["options"] = JToken.FromObject(options);

                if (tour.BookingEarliestDepartureDate == DateTime.MinValue ||
                    tour.BookingLatestDepartureDate == DateTime.MinValue)
                {
                    allPricesRecords.Add(mainDocument);
                }
                else
                {
                    var dateStamps = new List<long>();
                    for (var dt = tour.BookingEarliestDepartureDate; dt <= tour.BookingLatestDepartureDate; dt = dt.AddDays(1))
                    {
                        
                        if (!tour.DepartureDaysOfWeek.IsNullOrEmpty())
                        {
                            var intDayOfWeek = (int)dt.DayOfWeek;

                            if (!tour.DepartureDaysOfWeek.Contains(intDayOfWeek))
                            {
                                continue;
                            }
                        }

                        dateStamps.Add(((DateTimeOffset)dt).ToUnixTimeSeconds());
                        mainDocument["departureDate"] = JToken.FromObject(dateStamps);
                        allPricesRecords.Add(mainDocument);
                    }
                }
                
            }
            else
            {
                DateTime? from = null, to = null;

                if (tour.BookingEarliestDepartureDate != DateTime.MinValue)
                {
                    from = tour.BookingEarliestDepartureDate;
                }

                if (tour.BookingLatestDepartureDate != DateTime.MinValue)
                {
                    to = tour.BookingLatestDepartureDate;
                }

                var tourDictionary = new Dictionary<string, JObject>();

                foreach (var price in prices)
                {

                    for (var dt = price.StartDate; dt <= price.EndDate; dt = dt.AddDays(1))
                    {
                        if (dt <= DateTime.Now || (from != null && dt < from) || (to != null && dt > to))
                        {
                            continue;
                        }

                        if (!tour.DepartureDaysOfWeek.IsNullOrEmpty())
                        {
                            var intDayOfWeek = (int)dt.DayOfWeek;

                            if (!tour.DepartureDaysOfWeek.Contains(intDayOfWeek))
                            {
                                continue;
                            }
                        }

                        var options = GetTourOptions(searchDocument, tour, DateTime.Today, out var discountText, out var promotion);
                        
                        
                        var record = Clone(mainDocument);

                        //record["departureDate"] = JToken.FromObject(((DateTimeOffset)dt).ToUnixTimeSeconds());
                        //record["departureDateIndex"] = JToken.FromObject(((DateTimeOffset)dt).ToUnixTimeSeconds());
                        if (promotion != null)
                        {
                            var promotionItem = promotion.MapToPromotionItem();


                            if (price.TwinSharePrice > 0)
                            {
                                record["price"] = JToken.FromObject((int)promotionItem.GetDiscountedPrice(price.TwinSharePrice));
                                record["fromPrice"] = JToken.FromObject(price.TwinSharePrice); 
                                record["discount"] =
                                    JToken.FromObject((int)promotionItem.GetDiscount(price.TwinSharePrice, 1));

                            }
                            else
                            {
                                var originalPrice = ValidationHelper.GetDouble(record["price"], 0);

                                if (originalPrice > 0)
                                {
                                    record["fromPrice"] = record["price"];
                                    record["price"] = JToken.FromObject((int)promotionItem.GetDiscountedPrice(originalPrice));
                                    record["discount"] = (int)promotionItem.GetDiscount(originalPrice, 1);
                                }
                                else
                                {
                                    record["fromPrice"] = record["price"];
                                }

                            }

                        }
                        else
                        {
                            if (price.TwinSharePrice > 0)
                            {
                                record["price"] = JToken.FromObject(price.TwinSharePrice);
                                record["fromPrice"] = JToken.FromObject(price.TwinSharePrice);
                                record["discount"] = JToken.FromObject(0);
                            }
                            else
                            {
                                record["fromPrice"] = record["price"];
                                record["discount"] = JToken.FromObject(0);
                            }
                        }

                        
                        record["promotionTitle"] = JToken.FromObject(discountText);
                        record["options"] = JToken.FromObject(options);
                        var key = $"{record["fromPrice"].Value<string>()}{options.OnSale}";

                        if (!tourDictionary.ContainsKey(key))
                        {
                            record["objectID"] = Guid.NewGuid();
                            record["departureDate"] = JArray.FromObject(new List<long>
                            {
                                ((DateTimeOffset)dt).ToUnixTimeSeconds()
                            }); 
                            tourDictionary.Add(key, record);
                        }
                        else
                        {
                            var rec = tourDictionary[key];
                            var newDateList = rec["departureDate"].ToArray().ToList();
                            newDateList.Add(((DateTimeOffset)dt).ToUnixTimeSeconds());
                            rec["departureDate"] = JArray.FromObject(newDateList);
                            tourDictionary[key] = rec;
                        }
                        
                    }
                }


                allPricesRecords.AddRange(tourDictionary.Select(kvp => kvp.Value));
            }

            return allPricesRecords;
        }

        private JObject Clone(JObject indexAction)
        {
            var str = JsonConvert.SerializeObject(indexAction);

            return JsonConvert.DeserializeObject<JObject>(str);
        }

        private IEnumerable<string> GetToursInAlgoliaForDeletion(string code, SearchIndex algoliaIndex)
        {
            SearchResponse<ObjectIDResult> result;
            List<string> deleteIds = new List<string>();
                
            var page = 0;
            do
            {
               
                result = algoliaIndex.Search<ObjectIDResult>(new Query("")
                {
                    Filters = $"code:\"{code}\"",
                    AttributesToRetrieve = new[] { "objectID" },
                    HitsPerPage = 100,
                    Page = page,
                    Distinct = 0
                    
                });

                page++;
                if (!result.Hits.IsNullOrEmpty())
                {
                    deleteIds.AddRange(result.Hits.Select(a => a.objectID));
                }
            } while (!result.Hits.IsNullOrEmpty());

            return deleteIds;
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

    }
}