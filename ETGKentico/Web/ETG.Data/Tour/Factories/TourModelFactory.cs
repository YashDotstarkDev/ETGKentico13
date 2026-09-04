using System;
using Castle.Core.Internal;
using ETG.Data.Destination.Services;
using ETG.Data.Tour.Models;
using System.Collections.Generic;
using System.Linq;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.Helpers;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Cache;
using ETG.Data.Models.Common;
using ETG.Data.Repositories.Image;
using ETG.Data.Experience.Services;
using ETG.Data.Tour.Repositories;
using Devotion.Cache;
using ETG.Booking.Pricing.Classes.Info;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace ETG.Data.Tour.Factories
{
    public class TourModelFactory : ITourModelFactory
    {
        private readonly ICruiseTypeRepository _cruiseTypeRepository;
        private readonly IDestinationService _destinationService;
        private readonly IExperienceService _experienceService;
        private readonly IImageRepository _imageRepository;
        private readonly ICacheService _cacheService;
        public TourModelFactory(ICruiseTypeRepository cruiseTypeRepository, IDestinationService destinationService, IExperienceService experienceService,
            IImageRepository imageRepository, ICacheService cacheService)
        {
            _cruiseTypeRepository = cruiseTypeRepository;
            _destinationService = destinationService;
            _experienceService = experienceService;
            _imageRepository = imageRepository;
            _cacheService = cacheService;
        }

        protected List<ImageModel> GetGalleryImagesPlusHeroInternal(string path, ImageModel hero)
        {
            var images = _imageRepository.GetImages(path);

            if (hero != null)
            {
                images.Insert(0, hero);
            }

            return images;
        }

        protected List<ImageModel> GetGalleryImagesPlusHero(string path, ImageModel hero)
        {
            return _cacheService.GetDocumentDependentOnChildrenPath(
                () => GetGalleryImagesPlusHeroInternal($"{path}/Hero-Images", hero), "GetGalleryImages", path);
        }
        public TourSummaryInfoModel CreateSummaryInfoModel(Core.PageTypes.Tour tour)
        {
            if (tour == null)
            {
                return null;
            }

            var primaryCountry = _destinationService.GetDestination(tour.TourPrimaryCountry);
            var primaryExperience = _experienceService.GetExperienceSummary(tour.TourPrimaryExperience);
            var cruiseType = _cruiseTypeRepository.GetCruiseType(tour.TourCruiseType);

            var destinationName = string.Empty;
            var path = string.Empty;
            if (primaryCountry != null)
            {
                destinationName = primaryCountry.Name.Trim();
                path = $"/{destinationName.Replace(" ", "-")}/{tour.NodeAlias}";
            }
            else if (primaryExperience != null)
            {
                destinationName = tour.TourPrimaryCountryOverride.Trim();
                path = $"/{primaryExperience.Path.Split('/')[2]}/{tour.NodeAlias}";
            }
            else if (tour.TourIsCruise && tour.TourCruiseType > 0)
            {
                destinationName = tour.TourPrimaryCountryOverride.Trim();
                path = $"/{cruiseType.Key.Replace(" ", "-")}/{tour.NodeAlias}";
            }

            IEnumerable<string> subCountryNames = null;
            if (!string.IsNullOrWhiteSpace(tour.TourSubCountries))
            {
                var subCountryGuids = tour.TourSubCountries
                    .Split(';')
                    .Select(x => ValidationHelper.GetGuid(x, Guid.Empty))
                    .ToList();

                subCountryNames = _destinationService.GetDestinations(subCountryGuids).Select(x => x.Name);
            }
            
            var mainHotel = DocumentLinkProvider
                .GetDocumentLinks()
                .OnCurrentSite()
                .TopN(1)
                .Path(tour.NodeAliasPath, PathTypeEnum.Section)
                .WhereTrue(nameof(DocumentLink.LinkDocumentIsMain))
                .OrderBy(nameof(DocumentLink.NodeOrder))
                .FirstOrDefault();

            var highlightedInclusions = _cacheService.GetDocumentDependentOnAll(
               () => GetHighlightedInclusions(tour.NodeAliasPath),
               $"GetHighlightedInclusions{tour.NodeAliasPath}",
               Core.PageTypes.TourInclusion.CLASS_NAME);


            var heroImage = new ImageModel
            {
                ImagePath = tour.TourHeroImage,
                ImageAltText = tour.TourHeroImageAltTag,
                ImageCaption = tour.TourHeroImageCaption
            };

            var images = GetGalleryImagesPlusHero(tour.NodeAliasPath, heroImage);

            
            return new TourSummaryInfoModel
            {
                NodeGuid = tour.NodeGUID,
                Name = tour.TourName,
                TourCode = tour.TourCode,
                Summary = tour.TourSummary,
                Images = images,
                DepartureCity = tour.TourDepartureCity,
                DestinationCity = tour.TourDestinationCity,
                NoOfNights = tour.TourNights,
                DiscountGuid = tour.TourDiscountAndOffer,
                PrimaryCountryGuid = tour.TourPrimaryCountry,
                PrimaryCountryName = destinationName,
                SubCountriesGuids = tour.TourSubCountries,
                SubCountryNames = subCountryNames,
                Path = path,
                NodeAliasPath = tour.NodeAliasPath,
                ExperienceGuids = tour.TourExperiences,
                PrimaryExperienceGuid = tour.TourPrimaryExperience,
                TypeCodes = tour.TourTypes,
                IsCruise = tour.TourIsCruise,
                HasPeaceOfMindGuarantee = tour.TourHasPeaceOfMindGuarantee,
                TourHasFreedomOfChoice = tour.TourHasFreedomOfChoice,
                TourIsExclusive = tour.TourIsExclusive,
                TourHasSafeTravel = tour.TourHasSafeTravel,
                //HasEntireFlex = tour.TourHasEntireFlex,
                CruiseType = tour.TourIsCruise ? tour.TourCruiseType : 0,
                PriceTypeLabel = tour.TourPriceTypeLabel,
                PriceInclusions = tour.TourPriceInclusions,
                DateModifed = tour.DocumentModifiedWhen,
                RelatedTourCodes = tour.TourRelatedTourCodes,
                CustomHeading = tour.TouUpgradeLabel,
                DocumentSearchExcluded = tour.DocumentSearchExcluded,
                MainHotelName = mainHotel?.DocumentName,
                HighlightedInclusions = highlightedInclusions?.Select(x => x.TourInclusionLabel),
                TourDepartsFromInAustralia = tour.TourDepartsFromInAustralia,
                TourTravelEnds = tour.TourTravelEnds
            };
        }

        public TourModel CreateTourModel(Core.PageTypes.Tour tour)
        {
            if (tour == null)
            {
                return null;
            }
            
            return new TourModel
            {
                DocumentID = tour.DocumentID,
                TourSummaryInfo = CreateSummaryInfoModel(tour),
                Exclusions = tour.TourExclusions,
                AdditionalInclusions = tour.TourAdditionalInclusions,
                DepartFromInAustralia = tour.TourDepartsFromInAustralia,
                TravelEndsLocation = tour.TourTravelEnds,
                Details = tour.TourDetails,
                MapImage = tour.TourMapImage,
                TravelDates = tour.TourTravelDates,
                OptionalExtras = tour.TourOptionalExtras,
                ImportantNote = tour.TourImportantNote,
                Smallprints = tour.TourSmallprint,
                HotelGuids = tour.TourHotels,
                PageTitle = tour.DocumentPageTitle.IsNullOrEmpty() ? tour.TourName : tour.DocumentPageTitle,
                PageDescription = tour.DocumentPageDescription.IsNullOrEmpty() ? tour.TourSummary : tour.DocumentPageDescription,
                PageAliasPath = tour.NodeAliasPath,
                PageAlias = tour.NodeAlias,
                PageKeywords = tour.DocumentPageKeyWords,
                ShareTitle = tour.TourName,
                ShareDescription = tour.TourSummary,
                ShareImage = tour.TourHeroImage,
                ExcludedFromSearch = tour.DocumentSearchExcluded,
                IsPublished = tour.IsPublished,
                InPartnershipPath = tour.TourInPartnershipPath,
                HasPeaceOfMindGuarantee = tour.TourHasPeaceOfMindGuarantee,
                HasSafeTravel = tour.TourHasSafeTravel,
                HasFreedomOfChoice = tour.TourHasFreedomOfChoice,
                BookNowEnabled = tour.TourBookNowEnabled,
                BookingDepartureDatesOptions = tour.TourDepartureDatesOption,
                //TourFOCEntireFlex = tour.TourHasEntireFlex && tour.TourFOCEntireFlex,
                BookingEarliestDepartureDate = tour.TourEarliestDepartureDate,
                BookingLatestDepartureDate = tour.TourLatestDepartureDate,
                PaymentTerms = tour.TourPaymentTerms,
                FullPaymentDaysFromDepartureDate = tour.TourFullPaymentDays,
                DepositValue = tour.TourDepositRequiredFixedValue,
                DepositPercentage = tour.TourDepositRequiredPercentage,
                SkuId = tour.NodeSKUID,
                DepartureDaysOfWeek = GetDaysOfWeekList(tour.TourDepartureDaysOfWeek),
                AlternativeDatesPath = tour.TourAlternativeDatesPath,
                ChangeOfMindThresholdDays = tour.ChangeOfMindThresholdDays,
                SaleComissionPercent = tour.TourSaleComissionPercent,
                FaqsFolderPath = tour.TourFAQFolder
            };
        }

        private List<int> GetDaysOfWeekList(string tourDepartureDaysOfWeek)
        {
            if (tourDepartureDaysOfWeek.IsNullOrEmpty())
            {
                return null;
            }

            return tourDepartureDaysOfWeek.Split('|').Select(a => a.ToInteger(0)).ToList();
        }

        private List<TourInclusion> GetHighlightedInclusions(string tourNodePath)
        {
            return TourInclusionProvider
                .GetTourInclusions()
                .OnCurrentSite()
                .Path(tourNodePath, PathTypeEnum.Section)
                .WhereTrue(nameof(TourInclusion.TourInclusionHighlight))
                .OrderBy(nameof(TourInclusion.NodeOrder))
                .ToList();
        }
    }
}
