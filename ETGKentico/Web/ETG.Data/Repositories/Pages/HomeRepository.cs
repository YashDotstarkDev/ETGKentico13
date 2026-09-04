using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Data.Destination.Services;
using ETG.Data.DestinationExpertTeam.Services;
using ETG.Data.Models.Common;
using ETG.Data.Models.Pages;
using ETG.Data.Repositories.Common;
using ETG.Data.Repositories.Image;
using ETG.Data.Search;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Services;
using ETG.Data.Article.Models;
using ETG.Data.Article.Services;
using ETG.Data.Experience.Services;
using ETG.Data.Repositories.Base;

namespace ETG.Data.Repositories.Pages
{
    public class HomeRepository : IHomeRepository  
    {
        private readonly IHomepageRepository _homepageRepository;
        private readonly ICTAIconRepository _ctaIconRepository;
        private readonly IFeatureTileRepository _featureTileRepository;
        private readonly ICacheService _cacheService;
        private readonly IDestinationService _destinationService;
        private readonly IDestinationExpertTeamService _destinationExpertTeamService;
        private readonly IImageRepository _imageRepository;
        private readonly ITourService _tourService;
        private readonly IArticleService _articleService;
        private readonly IExperienceService _experienceService;
        private readonly ISearchConfiguration _searchConfiguration;
        private readonly IFAQRepository _faqRepository;

        public HomeRepository(IHomepageRepository homepageRepository, IDestinationService destinationService,
            ICTAIconRepository ctaIconRepository,
            IFeatureTileRepository featureTileRepository, IImageRepository imageRepository,
            IDestinationExpertTeamService destinationExpertTeamService,
            ITourService tourService,
            IArticleService articleService,
            ICacheService cacheService,
            ISearchConfiguration searchConfiguration, IExperienceService experienceService, IFAQRepository faqRepository)
        {
            _ctaIconRepository = ctaIconRepository;
            _featureTileRepository = featureTileRepository;
            _homepageRepository = homepageRepository;
            _cacheService = cacheService;
            _destinationService = destinationService;
            _imageRepository = imageRepository;
            _destinationExpertTeamService = destinationExpertTeamService;
            _tourService = tourService;
            _articleService = articleService;
            _searchConfiguration = searchConfiguration;
            _experienceService = experienceService;
            _faqRepository = faqRepository;
        }

        public HomeModel Get(string url, string path = "")
        {
            var home = _cacheService.GetDocumentDependentOnPath(() => _homepageRepository.GetHomepage("/home"), "home",
                "/home");
            if (home == null)
            {
                return null;
            }

            var homeModel = new HomeModel
            {
                Page = home
            };

            /*if (!home.FeaturedTilesFolder.IsNullOrEmpty())
            {
                homeModel.FeatureTilesComponent = new FeatureTilesComponentModel
                {
                    Title = home.FeaturedTilesTitle,
                    FeatureTiles = _cacheService.GetDocumentDependentOnChildrenPath(
                        () => _featureTileRepository.Get(home.FeaturedTilesFolder, home.FeaturedTilesFolder), "featuretiles",
                        home.FeaturedTilesFolder)
                };
            }*/

            if (!home.ProofPointsFolder.IsNullOrEmpty())
            {
                var count = 6;
                var cmsProofPoints = _cacheService.GetDocumentDependentOnChildrenPath(
                    () => _ctaIconRepository.Get("",home.ProofPointsFolder).Take(count),
                    $"proofpoints{count}", home.ProofPointsFolder);
                
                var proofPoints = new List<CTAIconModel>();
                foreach (var item in cmsProofPoints)
                {
                    if (proofPoints.Any(x => x.Label.Equals(item.Label)))
                    {
                        continue;
                    }
                    
                    proofPoints.Add(item);
                }              
                
                homeModel.ProofPointsComponent = new ProofPointComponentModel
                {
                    Title = home.ProofPointsTitle,
                    ProofPoints = proofPoints
                };
            }

            var destinations = _destinationService.GetMainDestinations();
            if (!destinations.IsNullOrEmpty())
            {
                homeModel.DestinationComponent = new DestinationComponentModel
                {
                    Title = home.DestinationsSectionTitle,
                    Destinations = destinations

                };
            }

            homeModel.HeroSliderImages = new List<ImageModel>{
                new ImageModel
            {
                ImagePath = home.HeroImage,
                ImageCaption = home.HeroCaption
            }};
            
            homeModel.HeroSliderImages.AddRange(GetGalleryImages(home.PageAliasPath));

            if (!home.FeatureTourCodes.IsNullOrEmpty())
            {
                homeModel.FeatureTours = new TourListingModel
                {
                    Tours = _tourService.GetTiledTours(home.FeatureTourCodes.Split(';').ToList()),
                    ViewAllUrl = homeModel.Page.FeatureTourViewAllUrl
                };
            }
            if (!home.FeatureTourCodes2.IsNullOrEmpty())
            {
                homeModel.FeatureTours2 = new TourListingModel
                {
                    Tours = _tourService.GetTiledTours(home.FeatureTourCodes2.Split(';').ToList()),
                    ViewAllUrl = homeModel.Page.FeatureTourViewAllUrl2,
                    ViewAllButtonLabel = homeModel.Page.FeatureTourViewAllButtonLabel2
                };
            }

            if (homeModel.FeatureTours == null || homeModel.FeatureTours.Tours.IsNullOrEmpty())
            {
                homeModel.FeatureTours = new TourListingModel
                {
                    Tours = _tourService.GetTiledTours(8)
                };
            }

            var allExperiences = _experienceService.GetAllExperienceSummaries();
            homeModel.Experiences = allExperiences.Where(a=> home.SelectedExperiences.Contains(a.NodeGuid));
            
            var articles = _articleService.GetLatestArticles(4);
            if (!articles.IsNullOrEmpty())
            {
                homeModel.TravelBlogsComponent = new ArticleListingModel
                {
                    Title = home.TravelBlogsSectionTitle,
                    Description = home.TravelBlogsSectionDescription,
                    Articles = articles,
                    ViewAllUrl = homeModel.Page.TravelBlogsSectionViewAllUrl
                };
            }

            homeModel.TourIndexName = _searchConfiguration.IndexTour;
            
            //FAQS
            var faqsPath = "/Shared/FAQs";
            homeModel.FAQs = _cacheService.GetDocumentMultipleDependency(() => _faqRepository.GetFAQs(path,20),
                $"faqwidget_{faqsPath}_{20}", path);


            return homeModel;
        }
        
        protected List<ImageModel> GetGalleryImages(string path)
        {
            return _cacheService.GetDocumentDependentOnChildrenPath(
                () => _imageRepository.GetImages($"{path}/Hero-Images"), "GetGalleryImages", path);
        }
        
    }
}