using AutoMapper;
using CMS.EventLog;
using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Destination.Services;
using ETG.Data.Experience.Services;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Search;
using ETG.Data.Services;
using ETG.Data.Tour.Repositories;
using ETG.Data.TravelDeals.Models;
using ETG.Data.TravelDeals.Services;

namespace ETG.Data.TravelDeals.Repositories
{
    public class TravelDealsPageRepository : BasePageRepository, ITravelDealsPageRepository
    {
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IPageItemRepository _pageRepository;
        private readonly ITravelDealsService _travelDealsService;
        private readonly IExperienceService _experienceService;
        private readonly IDestinationService _destinationService;
        private readonly ITourPricingRepository _tourPricingRepository;
        private readonly ISearchConfiguration _searchConfiguration;
        public TravelDealsPageRepository(ITravelDealsService travelDealsService, IExperienceService experienceService,
            IDestinationService destinationService, ITourPricingRepository tourPricingRepository,
            IPageItemRepository pageRepository, IShareLinksService shareLinksService, ISearchConfiguration searchConfiguration,
            ICacheService cacheService, IMapper mapper, IImageRepository imageRepository) : base(imageRepository, cacheService, shareLinksService)
        {
            _mapper = mapper;
            _cacheService = cacheService;
            _travelDealsService = travelDealsService;
            _experienceService = experienceService;
            _destinationService = destinationService;
            _tourPricingRepository = tourPricingRepository;
            _pageRepository = pageRepository;
            _searchConfiguration = searchConfiguration;
        }


        public TravelDealsPageModel Get(string url, string path)
        {
            var model = new TravelDealsPageModel();

            model.Page = _cacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(PathConstants.PATH_TRAVEL_DEALS), "TravelDealsPage", PathConstants.PATH_TRAVEL_DEALS);
            model.TourIndexName = _searchConfiguration.IndexTour;

            model.BreadCrumbs = GetBreadCrumbs("Travel deals");
            
            return model;
        }

    }
}
