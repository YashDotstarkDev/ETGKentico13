using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Article.Services;
using ETG.Data.Cache;
using ETG.Data.Destination.Models;
using ETG.Data.Destination.Services;
using ETG.Data.DestinationExpertTeam.Services;
using ETG.Data.Models.Common;
using ETG.Data.Models.Pages;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Common;
using ETG.Data.Tour.Services;
using System.Collections.Generic;
using System.Linq;
using ETG.Data.Brochure.Services;
using ETG.Data.Helpers;
using ETG.Data.Repositories.Image;
using ETG.Data.Search;
using ETG.Data.Services;
using ETG.Data.Tour.Models;

namespace ETG.Data.Destination.Repositories
{
    public class DestinationPageRepository : BasePageRepository, IDestinationPageRepository
    {
        private readonly IMapper _mapper;
        private readonly IContainerRepository _containerRepository;
        private readonly ICacheService _cacheService;
        private readonly IDestinationService _destinationService;
        private readonly IArticleService _articleService;
        private readonly IDestinationExpertTeamService _destinationExpertTeamService;
        private readonly IAccordionItemRepository _accordionItemRepository;
        private readonly ITourService _tourService;
        private readonly IBrochureService _brochureService;
        private readonly IFeatureTileRepository _featureTileRepository;
        private readonly ISearchConfiguration _searchConfiguration;
        private readonly IQuickLinkItemRepository _quickLinkItemRepository;
        public DestinationPageRepository(ICacheService cacheService, IDestinationService destinationService,
            IContainerRepository containerRepository,
            IFeatureTileRepository featureTileRepository,
            IDestinationExpertTeamService destinationExpertTeamService, IImageRepository imageRepository,
            IArticleService articleService,
            IAccordionItemRepository accordionItemRepository, ITourService tourService,
            IBrochureService brochureService,
            IShareLinksService shareLinksService, IMapper mapper, ISearchConfiguration searchConfiguration,
            IQuickLinkItemRepository quickLinkItemRepository) : base(imageRepository, cacheService,
            shareLinksService)
        {
            _mapper = mapper;
            _searchConfiguration = searchConfiguration;
            _cacheService = cacheService;
            _destinationService = destinationService;
            _articleService = articleService;
            _containerRepository = containerRepository;
            _destinationExpertTeamService = destinationExpertTeamService;
            _accordionItemRepository = accordionItemRepository;
            _tourService = tourService;
            _brochureService = brochureService;
            _featureTileRepository = featureTileRepository;
            _quickLinkItemRepository = quickLinkItemRepository;
        }

        public bool IsDestinationUnpublished(string url)
        {
            return _destinationService.IsDestinationUnpublished(url);
        }

        public DestinationPageModel Get(string url, string alias = "")
        {
            var destination = _destinationService.GetDestinationFullPage(url);
            if (destination?.BasicInfo == null)
            {
                return null;
            }

            List<MapItemModel> mapItems = null;
            List<RegionMapItemModel> mainDestinationMapItems = null;

            var mainDestinationGuid = destination.NodeGuid;
            DestinationSummaryModel mainRegion = null;
            if (destination.IsSubRegion)
            {
                var index = destination.PageAliasPath.LastIndexOf("/");

                if (index > -1)
                {

                    mainRegion = _destinationService.GetDestination(destination.PageAliasPath.Substring(0, index));

                    if (mainRegion != null)
                    {
                        mainDestinationGuid = mainRegion.DestinationGuid;
                    }


                }
            }

            if (destination.IsSubRegion)
            {
                if (destination.BasicInfo.Latitude != 0 && destination.BasicInfo.Longitude != 0)
                {
                    mapItems = new List<MapItemModel>
                    {
                        new MapItemModel
                        {
                            Name = destination.BasicInfo.Name,
                            Summary = destination.BasicInfo.Summary,
                            Longitude = destination.BasicInfo.Longitude,
                            Latitude = destination.BasicInfo.Latitude

                        }
                    };
                }
            }
            else
            {
                var subRegions = _destinationService.GetDestinationRegions(destination.BasicInfo.Path);
                if (subRegions != null)
                {
                    mainDestinationMapItems = subRegions.Select(a => new RegionMapItemModel
                    {
                        ID = a.IDInMap,
                        RegionName = a.Name,
                        RegionUrl = a.Path

                    }).ToList();
                }
            }

            var destinationPageModel = new DestinationPageModel
            {
                Destination = destination,
                Hero = new PageHeroModel
                {
                    Heading = destination.BasicInfo.Heading,
                    HeroImage = destination.BasicInfo.HeroImage,
                    HeroImageAltText = destination.BasicInfo.HeroAltText,
                    GalleryImages = GetGalleryImages(destination.PageAliasPath),
                    LargeHeading = true,
                    CampaignTitle = destination.BasicInfo.CampaignTitle,
                    HeroCaption = destination.BasicInfo.HeroAltText,
                    HeroForegroundImage = destination.BasicInfo.HeroForegroundImage, 
                    DisableOverlay = destination.BasicInfo.DisableOverlay,
                },
                RelatedArticles = _articleService.GetRelatedArticlesByDestination(mainDestinationGuid),
                DestinationExpert =
                    _destinationExpertTeamService.GetDestinationExpertByDestinationGuid(mainDestinationGuid),
                HelpfulInformation = _cacheService.GetDocumentDependentOnChildrenPath(
                    () => _accordionItemRepository.GetAccorionItems($"{destination.PageAliasPath}/Helpful-Information"),
                    "GetAccorionItems", destination.PageAliasPath),
                RegionMapItems = mapItems,
                MainDestinationMapItems = mainDestinationMapItems,
                Brochure = _brochureService.GetBrochureByDestinationGuid(mainDestinationGuid),
                MainDestination = mainRegion,
                QuickLinkItems = _quickLinkItemRepository.GetQuickLinkItems($"{destination.PageAliasPath}"),
                DestinationStickyModel = destination.DestinationStickyModel,
            };

            List<TourSummaryInfoModel> featureTours = null;
            if (!destination.FeatureTourCodes.IsNullOrEmpty() || !destination.FeatureCruiseCodes.IsNullOrEmpty())
            {
                var codes = ListUtility.GetCombinedList(';', destination.FeatureTourCodes,
                    destination.FeatureCruiseCodes);
                featureTours = _tourService.GetTiledTours(codes);
            }
            // else
            // {
            //     if (!destination.IsSubRegion)
            //     {
            //         featureTours = _tourService.GetTiledToursByDestination(destination.NodeGuid, 8);
            //
            //     }
            //     else if (mainRegion != null)
            //     {
            //         featureTours = _tourService.GetTiledToursByDestination(mainRegion.DestinationGuid, 8);
            //     }
            // }

            destinationPageModel.FeatureTours = new TourListingModel
            {
                Tours = featureTours,
                ViewAllUrl = (!destination.IsSubRegion)
                    ? SearchUrlHelper.GetDestinationFilterUrl(_searchConfiguration.IndexTour, destination.BasicInfo.Name)
                    : SearchUrlHelper.GetDestinationFilterUrl(_searchConfiguration.IndexTour,mainRegion?.Name)
            };

            if (destination.IsSubRegion)
            {
                destinationPageModel.BreadCrumbs = GetBreadCrumbs("Destinations", "/destinations", mainRegion.Name,
                    mainRegion.Path, destination.BasicInfo.Name);
            }
            else
            {
                destinationPageModel.BreadCrumbs =
                    GetBreadCrumbs("Destinations", "/destinations", destination.BasicInfo.Name);
            }

            destinationPageModel.Hero.ShareLinks =
                GetShareLinks(destination.BasicInfo.Path, destination.BasicInfo.HeroImage);

            var features = _cacheService.GetDocumentDependentOnChildrenPath(
                () => _featureTileRepository.Get(destination.BasicInfo.Path, destination.BasicInfo.Path),
                $"{destination.BasicInfo.Name}|featuretiles",
                destination.BasicInfo.Path);

            if (!features.IsNullOrEmpty())
            {
                destinationPageModel.FeatureTilesComponent = new FeatureTilesComponentModel
                {
                    Title = "Featured",
                    FeatureTiles = features
                };
            }
            
            
            return destinationPageModel;
        }

        public PrimaryLandingPageModel GetLandingPage(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _containerRepository.GetContainer(url),
                "destinationspage", url);
            
            if (!string.IsNullOrWhiteSpace(page.RedirectTo))
            {
                return new PrimaryLandingPageModel
                {
                    Page = page
                };
            }
            
            if (page?.Page == null)
            {
                return null;
            }

            if (page.PageHero == null)
            {
                page.PageHero = new PageHeroModel();
            }

            page.PageHero.GalleryImages = GetGalleryImages(page.Page.PageAliasPath);

            var viewModel = new PrimaryLandingPageModel
            {
                Page = page
            };

            var destinations = _destinationService.GetMainDestinations();
            if (destinations != null)
            {
                viewModel.Items = _mapper.Map<List<PrimaryLandingItemModel>>(destinations);
            }

            viewModel.BreadCrumbs = GetBreadCrumbs("Destinations");
            return viewModel;
        }
    }
}
