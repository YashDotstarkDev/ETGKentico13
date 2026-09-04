using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using ETG.Data.Cache;
using ETG.Data.DestinationExpertTeam.Services;
using ETG.Data.Repositories;
using ETG.Data.Tour.Models;
using ETG.Core.Constants;
using ETG.Core.PageTypes;
using ETG.Data.Brochure.Services;
using ETG.Data.Destination.Services;
using ETG.Data.Extensions;
using ETG.Data.Helpers;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Common;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;
using ETG.Data.Tour.Services;
using Devotion.Web.Base.Extensions;
using ETG.Data.Models.Common;
using ETG.Data.Experience.Services;
using ETG.Core.PageTypes.Providers;
using CMS.DocumentEngine;

namespace ETG.Data.Tour.Repositories
{
    public class TourPageRepository : BasePageRepository, ITourPageRepository
    {
        private readonly ITourExtraDetailService _tourExtraDetailService;
        private readonly ICruiseTypeRepository _cruiseTypeRepository;
        private readonly IDestinationExpertTeamService _destinationExpertTeamService;
        private readonly IDestinationService _destinationService;
        private readonly IExperienceService _experienceService;
        private readonly ITourService _tourService;
        private readonly IDiscountService _discountService;
        private readonly IBrochureService _brochureService;
        private readonly ICacheDependencyFactory _cacheDependencyFactory;
        private readonly ICTAImageRepository _ctaImageRepository;
        private readonly IContactService _contactService;
        private readonly IFAQRepository _faqRepository;
        private readonly ICacheService _cacheService;
        private readonly ICTAIconRepository _ctaIconRepository;
        private readonly IHomepageRepository _homepageRepository;

        public TourPageRepository(ICacheService cacheService, ITourExtraDetailService tourExtraDetailService,
            ICruiseTypeRepository cruiseTypeRepository, IDestinationExpertTeamService destinationExpertTeamService,
            IExperienceService experienceService, IImageRepository imageRepository, ICTAImageRepository ctaImageRepository,
            ITourService tourService, IDiscountService discountService, 
            IDestinationService destinationService, IBrochureService brochureService,
            IShareLinksService shareLinksService, ICacheDependencyFactory cacheDependencyFactory,
            IContactService contactService, IFAQRepository faqRepository, ICTAIconRepository ctaIconRepository, 
            IHomepageRepository homepageRepository) : base(imageRepository, cacheService, shareLinksService)
        {
            _cacheService = cacheService;
            _tourExtraDetailService = tourExtraDetailService;
            _cruiseTypeRepository = cruiseTypeRepository;
            _destinationExpertTeamService = destinationExpertTeamService;
            _tourService = tourService;
            _discountService = discountService;
            _destinationService = destinationService;
            _experienceService = experienceService;
            _brochureService = brochureService;
            _cacheDependencyFactory = cacheDependencyFactory;
            _ctaImageRepository = ctaImageRepository;
            _contactService = contactService;
            _faqRepository = faqRepository;
            _ctaIconRepository = ctaIconRepository;
            _homepageRepository = homepageRepository;
        }

        public TourPageModel Get(string url, string path = "")
        {
            var home = _cacheService.GetDocumentDependentOnPath(() => _homepageRepository.GetHomepage(PathConstants.PATH_HOME), "home",
                PathConstants.PATH_HOME);
            var tour = path.IsNullOrEmpty() ? _tourService.GetTourByUrl(url) : _tourService.GetTourByAlias(path);
            if (tour?.TourSummaryInfo == null)
            {
                return null;
            }


            var tourDestinations = new List<string>
            {
                tour.TourSummaryInfo.PrimaryCountryName
            };
            var subDestinations = _destinationService.GetDestinations(tour.TourSummaryInfo.SubCountriesGuids.ToGuidList(';'));

            if (!subDestinations.IsNullOrEmpty())
            {
                tourDestinations.AddRange(subDestinations.Select(a=>a.Name));
            }

            tour.DestinationsText = string.Join(", ", tourDestinations);
            var destination = _destinationService.GetDestination(tour.TourSummaryInfo.PrimaryCountryGuid);

            var highlights =  _tourExtraDetailService.GetTourHighlights(tour.PageAliasPath);
            var bonuses = _tourExtraDetailService.GetTourBonuses(tour.PageAliasPath);

            var roomUpgrades = CacheService.GetDocumentDependentOnChildrenPath
            (() => _tourExtraDetailService.GetTourRoomUpgrades(tour.PageAliasPath), "roomUpgrades",
                tour.PageAliasPath);
            var hotels =
                _tourExtraDetailService.GetHotelsByParentAliasPath(tour.PageAliasPath + "/Hotels");
            hotels.ForEach(a=>a.RoomUpgrades = roomUpgrades.Where(u => u.HotelGuid == a.NodeGuid).ToList());
            
            var count = 6;
            var cmsProofPoints =
                _cacheService
                 .GetDocumentDependentOnChildrenPath(
                     () => 
                _ctaIconRepository.Get("", home.ProofPointsFolder).Take(count)
                     ,
                     $"proofpoints{count}",
                     home.ProofPointsFolder
                     );

            var proofPoints = new List<CTAIconModel>();
            foreach (var item in cmsProofPoints)
            {
                if (proofPoints.Any(x => x.Label.Equals(item.Label)))
                {
                    continue;
                }
                    
                proofPoints.Add(item);
            }

            var faqFolderPath = !string.IsNullOrWhiteSpace(tour.FaqsFolderPath)
                ? tour.FaqsFolderPath
                : PathConstants.PATH_FAQs;
            var tourPage = new TourPageModel
            {
                TourInfo = tour,
                DestinationExpert =
                    _destinationExpertTeamService.GetDestinationExpertByDestinationGuid(tour.TourSummaryInfo
                        .PrimaryCountryGuid),
                Hotels =  hotels,
                Inclusions = CacheService.GetDocumentDependentOnChildrenPath
                (() => _tourExtraDetailService.GetTourInclusions(tour.PageAliasPath), "tourinclusionfalse",
                    tour.PageAliasPath),
                Highlights = highlights,
                Bonuses = bonuses,
                Itineraries = CacheService.GetDocumentDependentOnChildrenPath
                (() => _tourExtraDetailService.GetTourItinerary(tour.PageAliasPath), "touritinerary",
                    tour.PageAliasPath),
                Destination = destination,
                Brochure = _brochureService.GetBrochureByDestinationGuid(tour.TourSummaryInfo.PrimaryCountryGuid),
                FeatureTours = new TourListingModel
                {
                    Tours = _tourService.GetRelatedTiledTours(8, tour.TourSummaryInfo),
                    ViewAllUrl = 
                        destination == null
                            ? null 
                            : SearchUrlHelper.GetDestinationFilterUrl(destination.Name, "tours")
                },
                ShareLinks = GetShareLinks(tour.TourSummaryInfo.Path, tour.TourSummaryInfo?.Images.FirstOrDefault()?.ImagePath),
                InPartnershipImages = tour.InPartnershipPath.IsNullOrEmpty() ? null : CacheService.GetDocumentDependentOnChildrenPath
                (() => _ctaImageRepository.GetCTAImages(tour.InPartnershipPath), "inpartnership",
                    
                    tour.InPartnershipPath),
                FAQs = CacheService.GetDocumentDependentOnChildrenPath(() => _faqRepository.GetFAQs(faqFolderPath), $"faqs_{path}", path),
                OptionalExtras = CacheService.GetDocumentDependentOnChildrenPath
                (() => _tourExtraDetailService.GetTourOptionalExtras(tour.PageAliasPath), "optionalExtras",
                    tour.PageAliasPath),
                RoomUpgrades = roomUpgrades,
                TourUpgrades = _tourExtraDetailService.GetTourUpgrades(tour.PageAliasPath),
                TourAlternativeDates = _tourExtraDetailService.GetRelatedTourTiles(tour.AlternativeDatesPath),
                ProofPointsComponent = new ProofPointComponentModel()
                {
                    Title = home.ProofPointsTitle,
                    ProofPoints = proofPoints
                }

            };

            if (tour.TourSummaryInfo != null && tour.TourSummaryInfo.DiscountGuid != Guid.Empty)
            {
                tourPage.TourInfo.TourSummaryInfo.Discount = _discountService.GetDiscount(tour.TourSummaryInfo.DiscountGuid);
            }

            if (tour.TourSummaryInfo != null && tour.TourSummaryInfo.PrimaryCountryGuid != Guid.Empty)
            {
                var primaryCountry = _destinationService.GetDestination(tour.TourSummaryInfo.PrimaryCountryGuid);
                if (primaryCountry != null)
                {
                    tourPage.BreadCrumbs = GetBreadCrumbs(primaryCountry.Name.Trim(), $"{primaryCountry.Path}", tour.TourSummaryInfo.Name);
                }
            }
            else if (tour.TourSummaryInfo != null && tour.TourSummaryInfo.PrimaryExperience != null)
            {
                var primaryExperience = _experienceService.GetExperienceSummary(tour.TourSummaryInfo.PrimaryExperience.NodeGuid);
                if (primaryExperience != null)
                {
                    tourPage.BreadCrumbs = GetBreadCrumbs(primaryExperience.Name, $"{primaryExperience.Path}", tour.TourSummaryInfo.Name);
                }
            }
            else if (tour.TourSummaryInfo != null && tour.TourSummaryInfo.IsCruise && tour.TourSummaryInfo.CruiseType > 0)
            {
                var cruiseType = _cruiseTypeRepository.GetCruiseType(tour.TourSummaryInfo.CruiseType);
                if (!cruiseType.Key.IsNullOrEmpty())
                {
                    var cruisePage = ContainerProvider.GetContainers()
                        .WhereStartsWith(nameof(TreeNode.NodeAlias), cruiseType.Key.ToLower())
                        .FirstOrDefault();

                    tourPage.BreadCrumbs = GetBreadCrumbs(cruisePage.ContainerName, $"/cruises/{cruisePage.NodeAlias}", tour.TourSummaryInfo.Name);
                }
            }
            
            var contactInfo = _contactService.GetETGContactInfo();

            if (contactInfo != null)
            {
                tourPage.PeaceOfMindUrl = contactInfo.PeaceOfMindUrl;
                tourPage.SafeTravelUrl = contactInfo.SafeTravelUrl;
                tourPage.FreedomOfChoiceUrl = contactInfo.FreedomOfChoiceUrl;
            }


           
            if (tour.HasFreedomOfChoice)
            {
                var choices = _tourExtraDetailService.GetTourFreedomOfChoiceOptions(tour.TourSummaryInfo.NodeAliasPath);
            
                if (!choices.IsNullOrEmpty())
                {

                
                    var dayFreedomOfChoiceList = new List<string>();
                    var currentDay = string.Empty;
                    foreach (var choice in choices)
                    {
                        if (choice.DayCaption != currentDay)
                        {
                            currentDay = choice.DayCaption;
                            dayFreedomOfChoiceList.Add(choice.DayCaption);
                        }

                    }

                    tourPage.FreedomOfChoiceDays = dayFreedomOfChoiceList;

                    if (!tourPage.Itineraries.IsNullOrEmpty())
                    {

                        foreach (var itinerary in tourPage.Itineraries)
                        {
                            itinerary.FreedomOfChoices = choices.Where(c => c.DayCaption == itinerary.DayCaption)
                                .Select(a => new TourFreedomOfChoiceModel
                                {
                                    DayCaption = a.DayCaption,
                                    FreedomOfChoiceDescription = a.FreedomOfChoiceDescription,
                                    FreedomOfChoiceOptionName = a.FreedomOfChoiceOptionName
                                }).ToList();
                        }
                    }
                }
            }

            if (tour.BookNowEnabled)
            {
                tourPage.BookNowDisclaimer = (tourPage.TourInfo.TourSummaryInfo.Discount != null 
                                              && tourPage.TourInfo.TourSummaryInfo.Discount.IsOnSaleNow 
                                              && tourPage.TourInfo.TourSummaryInfo.Discount.FullPaymentRequired)
                    ? ResourceHelper.GetString("booknow.disclaimer.fullpayment")
                    : ResourceHelper.GetString("booknow.disclaimer.default");
            }
            return tourPage;
        }

        public TourEnquirePageModel GetEnquiryPage(string tourCode)
        {
            var tour = _tourService.GetTourByTourCode(tourCode);
            if (tour?.TourSummaryInfo == null)
            {
                return null;
            }

            tour.TourSummaryInfo.Images = GetGalleryImages(tour.PageAliasPath);
            
            var tourPage = new TourEnquirePageModel
            {
                TourInfo = tour
            };

            if (tour.TourSummaryInfo != null && tour.TourSummaryInfo.DiscountGuid != Guid.Empty)
            {
                tourPage.TourInfo.TourSummaryInfo.Discount = _discountService.GetDiscount(tour.TourSummaryInfo.DiscountGuid);
            }
            
            var primaryCountry = _destinationService.GetDestination(tour.TourSummaryInfo.PrimaryCountryGuid);
            if (primaryCountry != null)
            {
                tourPage.BreadCrumbs = GetBreadCrumbs(primaryCountry.Name, $"{primaryCountry.Path}", tour.TourSummaryInfo.Name);
            }

            return tourPage;
        }
    }
}