using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CommonServiceLocator;
using Devotion.Web.Base.Extensions;
using ETG.Algolia.SearchDocumentCreators.SearchDocumentModels;
using ETG.Core.Extensions;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Destination.Services;
using ETG.Data.Experience.Services;
using ETG.Data.Promotion;
using ETG.Data.Repositories.Image;
using ETG.Data.Repositories.Modules;
using ETG.Data.Tour.Data;
using ETG.Data.Tour.Repositories;
using ETG.Data.Tour.Services;
using Newtonsoft.Json.Linq;

namespace ETG.Algolia.SearchDocumentCreators
{
    public class TourSearchDocumentCreator : BaseSearchDocumentCreator
    {
        public TourSearchDocumentCreator(SearchDocument searchDocument, ISearchFields searchFields) : base(searchDocument, searchFields)
        {
            //FieldProcessors.Add(new Tuple<string, Func<object, object>>("ArticleCategories", ArticleCategoryProcessor));
            //FieldProcessors.Add(new Tuple<string, Func<object, object>>("TourHeroImage", ImgixifyProcessor));
            CamelCase = true;
            FieldExcludeList.AddRange(
                new []
                {
                    "TourHasFreedomOfChoice",
                    "TourDepartureFromPrice",
                    "TourHasPeaceOfMindGuarantee",
                    "TourHasSafeTravel",
                    "TourNights",
                    "TourSubCountries",
                    "DocumentPageDescription",
                    "DocumentPublishTo",
                    "DocumentPublishFrom",
                    "DocumentIsArchived",
                    "DocumentName",
                    "DocumentPageKeyWords",
                    "TourPriceInclusions",
                    "TourTypes",
                    "TourIsUpgrade",
                    "TourIsExclusive",
                    "TourDiscountAndOffer",
                    "TourHeroImage",
                    "TourPriceCurrency",
                    "TourExperiences",
                    "TourPrimaryCountry",
                    "TourBookNowEnabled",
                    "TourCruiseType",
                });
        }

        
        
        protected override void UrlProcessor(JObject doc)
        {
            var nodeAlias = ValidationHelper.GetString(searchDocument.GetValue("NodeAlias"), string.Empty);
            if (string.IsNullOrWhiteSpace(nodeAlias))
            {
                return;
            }

            var primaryCountryGuid = ValidationHelper.GetGuid(searchDocument.GetValue("TourPrimaryCountry"), Guid.Empty);
            var primaryCountryOverride = ValidationHelper.GetString(searchDocument.GetValue("TourPrimaryCountryOverride"), string.Empty);
            var primaryExperienceGuid = ValidationHelper.GetGuid(searchDocument.GetValue("TourPrimaryExperience"), Guid.Empty);
            var isCruise = ValidationHelper.GetBoolean(searchDocument.GetValue("TourIsCruise"), false);
            var cruiseTypeId = ValidationHelper.GetInteger(searchDocument.GetValue("TourCruiseType"), 0);

            if (primaryCountryGuid == Guid.Empty && primaryExperienceGuid == Guid.Empty && !isCruise)
            {
                return;
            }

            var destinationService = ServiceLocator.Current.GetInstance<IDestinationService>();
            var experienceService = ServiceLocator.Current.GetInstance<IExperienceService>();
            var cruiseTypeRepository = ServiceLocator.Current.GetInstance<ICruiseTypeRepository>();

            var primaryCountry = destinationService.GetDestination(primaryCountryGuid);
            var primaryExperience = experienceService.GetExperienceSummary(primaryExperienceGuid);
            var cruiseType = cruiseTypeRepository.GetCruiseType(cruiseTypeId);

            var destinationName = string.Empty;
            var path = string.Empty;
            if (primaryCountry != null)
            {
                destinationName = primaryCountry.Name;
                path = $"/{destinationName.Replace(" ", "-")}/{nodeAlias.ToLower()}";
            }
            else if (primaryExperience != null)
            {
                destinationName = primaryCountryOverride;
                path = $"/{primaryExperience.Path.Split('/')[2]}/{nodeAlias.ToLower()}";
            }
            else if (isCruise && cruiseTypeId > 0)
            {
                destinationName = primaryCountryOverride;
                path = $"/{cruiseType.Key.Replace(" ", "-")}/{nodeAlias.ToLower()}";
            }

            doc.Add("url", JToken.FromObject(path));
        }

        protected override void AddAdditionalFields(JObject doc)
        {
            ProcessorHelper.ExperienceProcessor(searchDocument, "TourExperiences", "experiences", doc);
            TourDestinationsProcessor(doc);
            PriceProcessor(doc);
            TourDayProcessor(doc);
            TravelStylesProcessor(doc);
            //OptionsProcessor(doc);
            InclusionsProcessor(doc);
            ImagesProcessor(doc);
            EndCityProcessor(doc);
            AdditionalContentProcessor(doc);
        }
        
        private void EndCityProcessor(JObject doc)
        {

            var endCity = ValidationHelper.GetString(searchDocument.GetValue("EndCity"), string.Empty);

            if (endCity.IsNullOrEmpty())
            {
                doc["endCity"] = ValidationHelper.GetString(searchDocument.GetValue("StartCity"), string.Empty);
            }
        }
        
        private void InclusionsProcessor(JObject doc)
        {
            var priceInclusions = new TourInclusions();

            var inclusions = ValidationHelper.GetString(searchDocument.GetValue("TourPriceInclusions"), string.Empty);

            priceInclusions.Flights = inclusions.Contains("Flights");
            priceInclusions.Accommodation = inclusions.Contains("Accommodation");
            priceInclusions.Meals = inclusions.Contains("Meals");
            priceInclusions.Transfer = inclusions.Contains("Transfer");

            doc.Add("inclusions", JToken.FromObject(priceInclusions));
        }
        
        private void ImagesProcessor(JObject doc)
        {
            var imageRepository = ServiceLocator.Current.GetInstance<IImageRepository>();
            var nodeAliasPath = ValidationHelper.GetString(searchDocument.GetValue("NodeAliasPath"), string.Empty);

            var images = imageRepository.GetImages($"{nodeAliasPath}/Hero-Images").Select(a=>a.ImagePath).ToList();

            var heroImage = ValidationHelper.GetString(searchDocument.GetValue("TourHeroImage"), string.Empty);

            if (!heroImage.IsNullOrEmpty())
            {
                images.Insert(0, heroImage);
            }

            for (var i = 0; i < images.Count; i++)
            {
                images[i] = $"{images[i].Replace("~", string.Empty).Imgixify()}";
            }
            doc.Add("thumbnails", JToken.FromObject(images));
        }
        
        /*private void OptionsProcessor(JObject doc)
        {
            var options = new TourOptions();
            
            options.BookNow = ValidationHelper.GetBoolean(searchDocument.GetValue("TourBookNowEnabled"), false);
            options.FreedomOfChoice = ValidationHelper.GetBoolean(searchDocument.GetValue("TourHasFreedomOfChoice"), false);
            options.ExclusivePackages = ValidationHelper.GetBoolean(searchDocument.GetValue("TourIsExclusive"), false);

            var promotionTitle = string.Empty;
            var isOnSale = IsOnSale(ValidationHelper.GetString(searchDocument.GetValue("code"), string.Empty),
                out promotionTitle);
            
            options.OnSale = isOnSale;
            options.PeaceOfMind = ValidationHelper.GetBoolean(searchDocument.GetValue("TourHasPeaceOfMindGuarantee"), false);
            options.SafeTravels = ValidationHelper.GetBoolean(searchDocument.GetValue("TourHasSafeTravel"), false);
            
            doc.Add("options", JToken.FromObject(options));
            doc.Add("promotionTitle", JToken.FromObject(""));
        }*/

        private void TourDayProcessor(JObject doc)
        {
            var nights =
                ValidationHelper.GetInteger(searchDocument.GetValue("TourNights"),0);

            if (nights > 0)
            {
                nights++;
            }
            
            doc.Add("duration", JToken.FromObject(nights));
        }


        /*private bool IsOnSale(string tourCode, out string discountText)
        {
            discountText = string.Empty;
            var promotionRepository = ServiceLocator.Current.GetInstance<IPromotionRepository>();
            var tourService = ServiceLocator.Current.GetInstance<ITourService>();//ServiceFactory.GetTourServiceObject();
            var promotion = promotionRepository.GetPromotionInfoForNonAgent(tourService.GetTourByTourCode(tourCode), DateTime.Today);

            if (promotion != null)
            {
                discountText = promotion.PromotionName;
                return true;
            }
            else
            {
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
                        if (discount.IsOnSaleNow)
                        {
                            discountText = discount.DisplayAsLabel ? discount.DisplayLabel : "";
                            return true;
                        }
                        
                        return false;
                        
                    }
                }

                return false;
            }
        }*/

        private void PriceProcessor(JObject doc)
        {
            var tourCode = ValidationHelper.GetString(searchDocument.GetValue("code"), string.Empty);
            var tourService = ServiceLocator.Current.GetInstance<ITourService>();//ServiceFactory.GetTourServiceObject();
            var tour = tourService.GetTourByTourCode(tourCode);
            
            if (tour?.TourSummaryInfo?.FromPrice != null)
            {
                var promotionRepository = ServiceLocator.Current.GetInstance<IPromotionRepository>();
                
                var promotion = promotionRepository.GetPromotionInfoForNonAgent(tour, DateTime.Today);
                if (promotion != null)
                {
                    var promotionItem = promotion.MapToPromotionItem();
                    var grossPrice = tour.TourSummaryInfo.FromPrice.LowestPrice;
                    doc.Add("price", JToken.FromObject((int)promotionItem.GetDiscountedPrice((double)tour.TourSummaryInfo.FromPrice.LowestPrice)));
                    doc.Add("fromPrice", JToken.FromObject(grossPrice));
                    doc.Add("discount", JToken.FromObject((int)promotionItem.GetDiscount(grossPrice, 1)));
                }
                else
                {
                    doc.Add("price", JToken.FromObject(tour.TourSummaryInfo.FromPrice.LowestPrice));
                    doc.Add("fromPrice", JToken.FromObject(tour.TourSummaryInfo.FromPrice.LowestPrice));
                    
                }
            }
            else
            {
                doc.Add("price", JToken.FromObject(0));
                doc.Add("fromPrice", JToken.FromObject(0));
            }
        }

        private void TourDestinationsProcessor(JObject doc)
        {
            var nodeAlias = ValidationHelper.GetString(searchDocument.GetValue("NodeAlias"), string.Empty);
            if (string.IsNullOrWhiteSpace(nodeAlias))
            {
                return;
            }

            var primaryCountryGuid = ValidationHelper.GetGuid(searchDocument.GetValue("TourPrimaryCountry"), Guid.Empty);
            var primaryCountryOverride = ValidationHelper.GetString(searchDocument.GetValue("TourPrimaryCountryOverride"), string.Empty);
            var primaryExperienceGuid = ValidationHelper.GetGuid(searchDocument.GetValue("TourPrimaryExperience"), Guid.Empty);
            var isCruise = ValidationHelper.GetBoolean(searchDocument.GetValue("TourIsCruise"), false);
            var cruiseTypeId = ValidationHelper.GetInteger(searchDocument.GetValue("TourCruiseType"), 0);

            if (primaryCountryGuid == Guid.Empty && primaryExperienceGuid == Guid.Empty && !isCruise)
            {
                return;
            }

            var destinationService = ServiceLocator.Current.GetInstance<IDestinationService>();
            var experienceService = ServiceLocator.Current.GetInstance<IExperienceService>();
            var cruiseTypeRepository = ServiceLocator.Current.GetInstance<ICruiseTypeRepository>();

            var primaryCountry = destinationService.GetDestination(primaryCountryGuid);
            var primaryExperience = experienceService.GetExperienceSummary(primaryExperienceGuid);
            var cruiseType = cruiseTypeRepository.GetCruiseType(cruiseTypeId);

            var destinationName = string.Empty;
            var path = string.Empty;
            if (primaryCountry != null)
            {
                destinationName = primaryCountry.Name;
                path = $"/{destinationName.Replace(" ", "-")}/{nodeAlias.ToLower()}";
            }
            else if (primaryExperience != null)
            {
                destinationName = primaryCountryOverride;
                path = $"/{primaryExperience.Path.Split('/')[2]}/{nodeAlias.ToLower()}";
            }
            else if (isCruise && cruiseTypeId > 0)
            {
                destinationName = primaryCountryOverride;
                path = $"/{cruiseType.Key.Replace(" ", "-")}/{nodeAlias.ToLower()}";
            }

            doc.Add("destination", JToken.FromObject(destinationName));
        }


        private void TravelStylesProcessor(JObject doc)
        {
            var tourTypeRepository = ServiceLocator.Current.GetInstance<ITourTypeRepository>(); //ServiceFactory.GetTourTypeRepositoryObject();
            var tourTypeCodes = ValidationHelper.GetString(searchDocument.GetValue("TourTypes"), string.Empty);
            var codeList = tourTypeCodes.Split(';').ToList();
            var tourTypes = tourTypeRepository.GetTourTypes(codeList).ToList();

            var styles = new List<string>();
            if (!tourTypes.IsNullOrEmpty())
            {
                styles = tourTypes.Select(a => a.Name).ToList();
            }
            
            var tourTypeCode = ValidationHelper.GetInteger(searchDocument.GetValue("TourCruiseType"), 0);
          
            var typeName = TourData.CruiseTypes.Where(a => a.Item1 == tourTypeCode).Select(a=>a.Item2).FirstOrDefault();

            if (!typeName.IsNullOrEmpty())
            {
                styles.Add(typeName);
            }

            doc.Add("travelStyles", JToken.FromObject(styles));
        }
        
        private void AdditionalContentProcessor(JObject doc)
        {
            var nodeAliasPath = ValidationHelper.GetString(searchDocument.GetValue("NodeAliasPath"), null);
            var builder = new StringBuilder();

            var tourItineraries = TourItineraryProvider.GetTourItineraries().Path(nodeAliasPath, PathTypeEnum.Children);
            var optionalExtras = OptionalExtrasProvider.GetOptionalExtras().Path(nodeAliasPath, PathTypeEnum.Children);
            var inclusions = TourInclusionProvider.GetTourInclusions().Path(nodeAliasPath, PathTypeEnum.Children);

            foreach (var itinerary in tourItineraries)
            {
                builder.AppendLine(itinerary.TourItineraryTitle);
            }
            foreach (var extra in optionalExtras)
            {
                builder.AppendLine(extra.OptionalExtrasTitle);
            }
            foreach (var inc in inclusions)
            {
                builder.AppendLine(inc.TourInclusionLabel);
            }

            string additionalContent = HTMLHelper.StripTags(builder.ToString(), false);
            additionalContent = Regex.Replace(additionalContent, @"<[^>]+>|&nbsp;", " ").Trim();
            additionalContent = Regex.Replace(additionalContent, @"\s{2,}", " ");

            additionalContent = additionalContent.Length > 0
                ? additionalContent.Substring(0,
                    additionalContent.Length >= 2000 ? 2000 : additionalContent.Length - 1)
                : string.Empty;
            doc.Add("additionalContent", JToken.FromObject(WebUtility.HtmlDecode(additionalContent)));
            
            
            var highlights = TourHighlilghtProvider.GetTourHighlilghts().Path(nodeAliasPath, PathTypeEnum.Children);

            builder = new StringBuilder();
            foreach (var h in highlights)
            {
                builder.AppendLine(HTMLHelper.StripTags(h.TourHighlightLabel));
            }
            doc.Add("highlights", JToken.FromObject(builder.ToString()));
        }
    }
}
