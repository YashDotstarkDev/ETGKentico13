using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Models.Pages;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Tour;
using ETG.Data.Services;
using System.Linq;
using ETG.Data.Experience.Services;
using ETG.Data.Repositories.Image;
using ETG.Data.Repositories.Modules;
using ETG.Data.Search;
using ETG.Data.Tour.Data;
using ETG.Data.Tour.Repositories;

namespace ETG.Data.Repositories.Pages
{
    public class SearchPageRepository : BasePageRepository, ISearchPageRepository
    {
        private readonly IExperienceService _experienceService;
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IPriceInclusionRepository _priceInclusionRepository;
        private readonly IPageItemRepository _pageRepository;
        private readonly ISearchConfiguration _searchConfiguration;
        public SearchPageRepository(IPageItemRepository pageRepository, ITourTypeRepository tourTypeRepository, IPriceInclusionRepository priceInclusionRepository,
            IExperienceService experienceService, ISearchConfiguration searchConfiguration, ICacheService cacheService,
            IImageRepository imageRepository, IShareLinksService shareLinksService)
            : base(imageRepository, cacheService, shareLinksService)
        {
            _pageRepository = pageRepository;
            _tourTypeRepository = tourTypeRepository;
            _priceInclusionRepository = priceInclusionRepository;
            _experienceService = experienceService;
            _searchConfiguration = searchConfiguration;
        }

        public SearchPageModel Get()
        {
            var page = CacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(PathConstants.PATH_SEARCH), "SearchPage", PathConstants.PATH_SEARCH);

            if (page == null)
            {
                return null;
            }
            var model = new SearchPageModel
            {
                Page = page,
                TourTypes = _tourTypeRepository.GetAllTourTypes().ToDictionary(a => a.Name, a => a.IconClass),
                PriceInclusions = _priceInclusionRepository.GetAllPriceInclusionsIconSVG(),
                Experiences = _experienceService.GetAllExperienceSummaries().ToDictionary(a=>a.Name, a=>a.IconClass),
                CruiseTypes = TourData.CruiseTypes.ToDictionary(a=>a.Item2, a=>a.Item3),
                TourIndexName = _searchConfiguration.IndexTour,
                ArticleIndexName = _searchConfiguration.IndexArticle,
                ContentsIndexName = _searchConfiguration.IndexContents

            };
            model.BreadCrumbs = GetBreadCrumbs("Search");

            return model;
        }

        public PackageSearchPageModel GetPackageSearchPage()
        {
            var page = CacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(PathConstants.PATH_SEARCH), "PackageSearchPage", PathConstants.PATH_SEARCH);

            if (page == null)
            {
                return null;
            }
            var model = new PackageSearchPageModel
            {
                Page = page,
                TourIndexName = _searchConfiguration.IndexTour,

            };
            model.BreadCrumbs = GetBreadCrumbs("Search");

            return model;
        }
    }
}
