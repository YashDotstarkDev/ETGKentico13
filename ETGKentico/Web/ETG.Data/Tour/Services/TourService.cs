using Castle.Core.Internal;
using CMS.Base;
using Devotion.Web.Base.Extensions;
using ETG.Core.Http;
using ETG.Data.Cache;
using ETG.Data.Destination.Services;
using ETG.Data.Experience.Services;
using ETG.Data.Repositories.Tour;
using ETG.Data.Tour.Factories;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using ETG.Core.Constants;
using ETG.Core.PageTypes;
using ETG.Data.Repositories.Modules;
using ETG.Booking.Pricing.Repositories;
using ETG.Core.Promotion;
using ETG.Data.Models.Booking;
using ETG.Data.Promotion;

namespace ETG.Data.Tour.Services
{
    public class TourService : ITourService
    {
        private readonly ICacheService _cacheService;
        private ITourProductRepository _tourRepository;
        private readonly ITourListingService _tourListingService;
        private readonly IExperienceService _experienceService;
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IDestinationService _destinationService;
        private readonly IDiscountService _discountService;
        private readonly IPriceInclusionRepository _priceInclusionRepository;
        private readonly ITourPricingRepository _tourPricingRepository;
        private readonly ICruiseTypeRepository _cruiseTypeRepository;
        private readonly ICacheDependencyFactory _cacheDependencyFactory;
        private readonly IBookingPriceRepository _bookingPriceRepository;
        private readonly IPromotionRepository _promotionRepository;
        public TourService(ICacheService cacheService,
            IDestinationService destinationService, IExperienceService experienceService, ITourTypeRepository tourTypeRepository,
            IDiscountService discountService, IPriceInclusionRepository priceInclusionRepository, ICruiseTypeRepository cruiseTypeRepository,
            ITourPricingRepository tourPricingRepository, ITourProductRepository tourRepository, ITourListingService tourListingService, 
            ICacheDependencyFactory cacheDependencyFactory, IBookingPriceRepository bookingPriceRepository, IPromotionRepository promotionRepository)
        {
            _tourRepository = tourRepository;
            _cacheService = cacheService;
            _experienceService = experienceService;
            _tourTypeRepository = tourTypeRepository;
            _discountService = discountService;
            _priceInclusionRepository = priceInclusionRepository;
            _destinationService = destinationService;
            _tourPricingRepository = tourPricingRepository;
            _tourListingService = tourListingService;
            _cruiseTypeRepository = cruiseTypeRepository;
            _cacheDependencyFactory = cacheDependencyFactory;
            _bookingPriceRepository = bookingPriceRepository;
            _promotionRepository = promotionRepository;
        }

        public void SetTourRepository(ITourProductRepository repository)
        {
            _tourRepository = repository;
        }
        private TourModel GetTourByAliasInternal(string alias)
        {
            var tour = _tourRepository.GetTourByPageAlias(alias);

            if (tour?.TourSummaryInfo == null)
            {
                return null;
            }

            tour.TourSummaryInfo = AssignOtherDetailsForTiledTour(tour.TourSummaryInfo, true);
            return tour;
        }

        private TourModel GetTourByUrlInternal(string url)
        {
            if (url.IsNullOrEmpty())
            {
                return null;
            }

            var arr = url.Split('/');

            if (arr.Length != 3)
            {
                return null;
            }

            var categoryName = arr[1];

            if (!_destinationService.IsMainCountry(categoryName.Replace("-", " ")) &&
                !_experienceService.IsMainExperience(categoryName) &&
                _cruiseTypeRepository.GetCruiseType(categoryName).Key <= 0)
            {
                return null;
            }

            var alias = arr[2];

            var tour = _tourRepository.GetTourByPageAlias(alias);

            if (tour?.TourSummaryInfo == null)
            {
                return null;
            }

            var primaryCountry = _destinationService.GetDestination(tour.TourSummaryInfo.PrimaryCountryGuid);
            var primaryExperience = _experienceService.GetExperienceSummary(tour.TourSummaryInfo.PrimaryExperienceGuid);
            var cruiseType = _cruiseTypeRepository.GetCruiseType(tour.TourSummaryInfo.CruiseType);
            if (primaryCountry == null && primaryExperience == null && string.IsNullOrEmpty(cruiseType.Key))
            {
                return null;
            }
            if (primaryCountry != null && !primaryCountry.Name.Trim().Equals(categoryName.Replace("-", " "), StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            else if (primaryExperience != null && !primaryExperience.Path.Split('/')[2].Equals(categoryName, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            else if (!string.IsNullOrEmpty(cruiseType.Key) && !tour.TourSummaryInfo.IsCruise && !cruiseType.Key.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            tour.TourSummaryInfo = AssignOtherDetailsForTiledTour(tour.TourSummaryInfo, true);

            return tour;
        }

        public TourModel GetTourByAlias(string alias)
        {
            var tour = _cacheService.GetDocumentDependentOnPath
                (() => GetTourByAliasInternal(alias), $"tour{alias}", new string[] {$"{_cacheDependencyFactory.GetPageTypeDependency(DiscountAndOffer.CLASS_NAME)}"});
            
            return tour;
        }

        private TourModel GetTourByTourCodeInternal(string tourcode)
        {
            var tour = _tourRepository.GetTourByTourCode(tourcode);

            if (tour?.TourSummaryInfo == null)
            {
                return null;
            }

            tour.TourSummaryInfo = AssignOtherDetailsForTiledTour(tour.TourSummaryInfo, true);

            return tour;
        }

        private TourModel GetPreviewableTourByTourCodeInternal(string tourcode)
        {
            var tour = _tourRepository.GetPreviewableTourByTourCode(tourcode);

            if (tour?.TourSummaryInfo == null)
            {
                return null;
            }

            tour.TourSummaryInfo = AssignOtherDetailsForTiledTour(tour.TourSummaryInfo, true);

            return tour;
        }
        public TourModel GetPreviewableTourByTourCode(string tourcode)
        {
            var tour = _cacheService.GetDocumentDependentOnPath
                (() => GetPreviewableTourByTourCodeInternal(tourcode), $"tourpreviewable{tourcode}", new string[] { $"{_cacheDependencyFactory.GetPageTypeDependency(DiscountAndOffer.CLASS_NAME)}" });

            return tour;
        }
        public TourModel GetTourByTourCode(string tourcode)
        {
            var tour =  _cacheService.GetDocumentDependentOnPath
                (() => GetTourByTourCodeInternal(tourcode), $"tour{tourcode}", new string[] { $"{_cacheDependencyFactory.GetPageTypeDependency(DiscountAndOffer.CLASS_NAME)}" });

            return tour;
        }
        public TourModel GetTourByUrl(string url)
        {
            return _cacheService.GetDocumentDependentOnPath
                (() => GetTourByUrlInternal(url), $"tour{url}", new string[] { $"{_cacheDependencyFactory.GetPageTypeDependency(DiscountAndOffer.CLASS_NAME)}" });
        }

        public TourSummaryInfoModel AssignOtherDetailsForTiledTour(TourSummaryInfoModel tour, bool getPrice = false)
        {
            if (tour == null)
            {
                return null;
            }

            if (!tour.ExperienceGuids.IsNullOrEmpty())
            {
                tour.Experiences = _experienceService.GetExperienceSummaries(tour.ExperienceGuids.Split(';').Select(a => a.ToGuid()).ToList()).ToList();
                tour.ExperienceIcons = _experienceService.GetExperienceSummaries(tour.ExperienceGuids.Split(';').Select(a => a.ToGuid()).ToList()).Select(a => new KeyValuePair<string, string>(a.Name, a.IconClass)).ToList();
                
            }

            if (tour.PrimaryExperienceGuid != Guid.Empty)
            {
                tour.PrimaryExperience = _experienceService.GetExperienceSummary(tour.PrimaryExperienceGuid);
            }

            if (!tour.TypeCodes.IsNullOrEmpty())
            {
                tour.TourTypes = _tourTypeRepository
                    .GetTourTypes(tour.TypeCodes.Split(';').ToList()).ToList();
                tour.TourTypeIcons = _tourTypeRepository
                    .GetTourTypes(tour.TypeCodes.Split(';').ToList())
                    .Select(a => new KeyValuePair<string, string>(a.Name, a.IconClass)).ToList();
            }

            if (!tour.PriceInclusions.IsNullOrEmpty())
            {
                tour.PriceInclusionIcons = _priceInclusionRepository.GetAllPriceInclusionsIconSVG()
                    .Where(a => tour.PriceInclusions.Split('|').Contains(a.Name)).ToList();
            }

            if (tour.IsCruise && tour.CruiseType > 0)
            {
                var cruiseIcon = _cruiseTypeRepository.GetCruiseType(tour.CruiseType);

                if (!cruiseIcon.Equals(default(KeyValuePair<string, string>)))
                {
                    tour.CruiseTypeIcons = new List<KeyValuePair<string, string>>{cruiseIcon};
                }
            }

            if (getPrice)
            {
                tour.FromPrice = GetLowestPrice(tour);
                
            }

            return tour;

        }

        private void AssignOtherDetailsForTiledTours(ref List<TourSummaryInfoModel> tours)
        {
            if (!tours.IsNullOrEmpty())
            {
                var tourAliasPaths = tours.Select(a => a.NodeAliasPath).ToList();
                var tourCodes = tours.Select(a => a.TourCode).ToList();
                var allBookingLowestPrices = _bookingPriceRepository.GetLowestPricesFromBookNowPricing(tourCodes);
                var allPricing = _tourPricingRepository.GetTourPricings(tourAliasPaths);
                var allPromotions = _promotionRepository.GetAllPromotions(DateTime.Today);

                for (var i = 0; i < tours.Count; i++)
                {
                    tours[i] = AssignOtherDetailsForTiledTour(tours[i]);
                    tours[i].Promotion = GetTourPromotion(tours[i], allPromotions);
                    var tourCode = tours[i].TourCode;
                    var lowestPrice = allBookingLowestPrices.Where(a => a.Key == tourCode).Select(a=>a.Value).FirstOrDefault();

                    if (lowestPrice > 0)
                    {
                        /*if (tours[i].Promotion != null)
                        {
                            lowestPrice = tours[i].Promotion.GetDiscountedPrice(lowestPrice);
                        }*/
                        tours[i].FromPrice = new PricingModel
                        {
                            OverridePrice = lowestPrice.ToInteger(0)
                        };
                    }else
                    {
                        tours[i].FromPrice = GetLowestPrice(allPricing, tours[i].NodeAliasPath);
                    }

                    tours[i].Discount = _discountService.GetDiscount(tours[i].DiscountGuid);
                }


            }
        }

        private PromotionItem GetTourPromotion(TourSummaryInfoModel tour, List<PromotionInfo> allPromotions)
        {
            if (allPromotions.IsNullOrEmpty() || tour == null)
            {
                return null;
            }

            var promotion = allPromotions.FirstOrDefault(a => CollectionExtensions.IsNullOrEmpty(a.PromotionEmailDomains) && ((a.PromotionPackageCodes != null &&
                                                                  ("\r\n" + a.PromotionPackageCodes + "\r\n").Contains(
                                                                      $"\r\n{tour.TourCode}\r\n")))
                                                              || (a.PromotionDestinations != null &&
                                                                  a.PromotionDestinations.Contains(tour.PrimaryCountryGuid.ToString())));

            if (promotion == null)
            {
                return null;
            }

            return promotion.MapToPromotionItem();
        }

        public List<TourSummaryInfoModel> GetTiledTours(List<string> tourCodes, bool includeHiddenUpgradeTours=false)
        {
            var tours = _tourListingService.GetCombinedTours(tourCodes, includeHiddenUpgradeTours);

            AssignOtherDetailsForTiledTours(ref tours);

            return tours;
        }

        private PricingModel GetLowestPrice(TourSummaryInfoModel tour)
        {
            var bookNowPrice = _bookingPriceRepository.GetLowestPriceFromBookNowPricing(tour.TourCode);

            if (bookNowPrice > 0)
            {
                return new PricingModel
                {
                    OverridePrice = bookNowPrice
                };
            }
            var tourPricings = GetTourPricings(tour.NodeAliasPath);

            if (!tourPricings.IsNullOrEmpty())
            {
                return tourPricings.Where(a => a.LowestPrice > 0).OrderBy(a => a.LowestPrice).FirstOrDefault();
            }

            return null;
        }
        private PricingModel GetLowestPrice(List<PricingModel> allPricing, string tourNodeAliasPath)
        {
            if (allPricing.IsNullOrEmpty() || tourNodeAliasPath.IsNullOrEmpty())
            {
                return null;
            }
            return allPricing.Where(a => a.NodeAliasPath.ToLower().StartsWith(tourNodeAliasPath.ToLower().EndWithSlash())).Where(a => a.LowestPrice > 0).OrderBy(a => a.LowestPrice).FirstOrDefault();
        }

        public List<PricingModel> GetTourPricings(string aliasPath)
        {
            return _cacheService.GetDocumentDependentOnChildrenPath(() => _tourPricingRepository.GetTourPricings(aliasPath), $"tourpricing", aliasPath);

        }

        public List<TourSummaryInfoModel> GetTiledToursByDestination(Guid destinationGuid, int topN)
        {
            var tours = _tourListingService.GetCombinedToursByDestination(destinationGuid, topN);

            AssignOtherDetailsForTiledTours(ref tours);

            return tours;
        }

        public List<TourSummaryInfoModel> GetTiledToursByExperience(Guid experienceGuid, int topN)
        {
            var tours = _tourListingService.GetCombinedToursByExperience(experienceGuid, topN);

            AssignOtherDetailsForTiledTours(ref tours);

            return tours;
        }

        public List<TourSummaryInfoModel> GetTiledTours(int topN)
        {
            var tours = _tourListingService.GetCombinedTours(topN);

            AssignOtherDetailsForTiledTours(ref tours);

            return tours;
        }

        public List<TourSummaryInfoModel> GetTiledToursByDestinations(List<Guid> destinationGuids, int topN)
        {
            var tours = _tourListingService.GetCombinedToursByDestinations(destinationGuids, topN);

            AssignOtherDetailsForTiledTours(ref tours);

            return tours;
        }

        public List<TourSummaryInfoModel> GetRelatedTiledTours(int topN, TourSummaryInfoModel tour)
        {
            if (tour == null)
            {
                return null;
            }

            var allTours = new List<TourSummaryInfoModel>();

            if (!tour.RelatedTourCodes.IsNullOrEmpty())
            {
                var tourCodes = tour.RelatedTourCodes.ToStringList(';');
                allTours = GetTiledTours(tourCodes);
            }

            if (allTours.IsNullOrEmpty())
            {
                allTours = _tourRepository.GetToursByDestinations(tour.AllDestinationGuids, 500);

            }
            if (allTours == null)
            {
                return null;
            }

            allTours = allTours.Where(a => a.TourCode != tour.TourCode).ToList();
            var relatedTours = allTours.Where(a => a.ExperienceGuids.InGuidList(';', tour.ExperienceGuids.ToGuidList(';'))
            && a.TypeCodes.InStringList(';', tour.TypeCodes.ToStringList(';'))).Take(topN).ToList();

            if (relatedTours.Count < topN)
            {
                var relatedTourCodes = relatedTours.Select(a => a.TourCode).ToList();
                var take = topN - relatedTours.Count;
                var addedTours = allTours
                    .Where(a => a.ExperienceGuids.InGuidList(';', tour.ExperienceGuids.ToGuidList(';'))
                    && !relatedTourCodes.Contains(a.TourCode)).Take(take).ToList();

                relatedTours.AddRange(addedTours);
            }

            if (relatedTours.Count < topN)
            {
                var relatedTourCodes = relatedTours.Select(a => a.TourCode).ToList();
                var take = topN - relatedTours.Count;
                var addedTours = allTours.Where(a => !relatedTourCodes.Contains(a.TourCode)).Take(take).ToList();
                relatedTours.AddRange(addedTours);
            }


            AssignOtherDetailsForTiledTours(ref relatedTours);

            return relatedTours;
        }

        public List<TourSummaryInfoModel> GetTiledToursByTourType(string tourType, int topN)
        {
            var tours = _tourRepository.GetToursByTourType(tourType, topN);
            AssignOtherDetailsForTiledTours(ref tours);

            return tours;
        }

        public List<TourSummaryInfoModel> GetTiledToursByCruiseType(int cruiseType, int topN)
        {
            var tours = _tourRepository.GetToursByCruiseType(cruiseType, topN);
            AssignOtherDetailsForTiledTours(ref tours);

            return tours;
        }
    }
}
