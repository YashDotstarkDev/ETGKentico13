using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Article.Services;
using ETG.Data.Cache;
using ETG.Data.Experience.Models;
using ETG.Data.Models.Common;
using ETG.Data.Models.Pages;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Base;
using ETG.Data.Tour.Services;
using System.Collections.Generic;
using System.Linq;
using ETG.Data.Helpers;
using ETG.Data.Repositories.Image;
using ETG.Data.Search;
using ETG.Data.Services;
using ETG.Data.Tour.Models;

namespace ETG.Data.Experience.Repositories
{
    public class ExperiencePageRepository : BasePageRepository, IExperiencePageRepository
    {
        private readonly IMapper _mapper;
        private readonly IContainerRepository _containerRepository;
        private readonly IExperienceRepository _experienceRepository;
        private readonly IArticleService _articleService;
        private readonly ICacheService _cacheService;
        private readonly ITourService _tourService;
        private readonly ISearchConfiguration _searchConfiguration;

        public ExperiencePageRepository(IContainerRepository containerRepository,
            IExperienceRepository experienceRepositor, IArticleService articleService,
            ICacheService cacheService, IMapper mapper, IImageRepository imageRepository,
            ITourService tourService, IShareLinksService shareLinksService, ISearchConfiguration searchConfiguration) : base(imageRepository, cacheService,
            shareLinksService)
        {
            _mapper = mapper;
            _experienceRepository = experienceRepositor;
            _cacheService = cacheService;
            _articleService = articleService;
            _containerRepository = containerRepository;
            _tourService = tourService;
            _searchConfiguration = searchConfiguration;
        }


        public ExperiencePageModel Get(string url, string path)
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _experienceRepository.GetExperience(url),
                "Experience", url);

            if (page == null)
            {
                return null;
            }

            var model = new ExperiencePageModel
            {
                Page = page,
                Hero = new PageHeroModel
                {
                    Heading = page.Heading,
                    HeroImage = page.SummaryInfo.Image,
                    HeroForegroundImage = page.SummaryInfo.ForegroundImage,
                    DisableOverlay = page.SummaryInfo.DisableOverlay,
                    HeroImageAltText = page.SummaryInfo.HeroAltText,
                    HeroIconImage = page.HeroIconImage,
                    HeroIconSvg = page.HeroIconSVG,
                    HeroCaption = page.HeroImageAccreditation,
                    GalleryImages = GetGalleryImages(page.PageAliasPath),
                },
                RelatedArticles = _articleService.GetRelatedArticlesByExperience(page.NodeGuid)?.Take(8).ToList()
            };

            List<TourSummaryInfoModel> featureTours = null;
            if (!page.FeatureTourCodes.IsNullOrEmpty() || !page.FeatureCruiseCodes.IsNullOrEmpty())
            {
                var codes = ListUtility.GetCombinedList(';', page.FeatureTourCodes,
                    page.FeatureCruiseCodes);
                featureTours = _tourService.GetTiledTours(codes);
            }
            else
            {
                featureTours = _tourService.GetTiledToursByExperience(page.NodeGuid, 3);
            }

            model.FeatureTours = new TourListingModel
            {
                Tours = featureTours,
                ViewAllUrl = SearchUrlHelper.GetExperienceFilterUrl(_searchConfiguration.IndexTour, model.Page.SummaryInfo.Name)
            };

            model.BreadCrumbs = GetBreadCrumbs("Experiences", "/experiences", page.SummaryInfo.Name);

            model.Hero.ShareLinks = GetShareLinks(page.PageAliasPath, page.ShareImage);
            return model;
        }

        public PrimaryLandingPageModel GetLandingPage(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _containerRepository.GetContainer(url),
                "experiencespage", url);

            if (page?.Page == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(page.RedirectTo))
            {
                return new PrimaryLandingPageModel
                {
                    Page = page
                };
            }

            var experiences = _cacheService.GetDocumentDependentOnChildrenPath
                (() => _experienceRepository.GetExperiences(url), "experienceall", url);

            if (page.PageHero == null)
            {
                page.PageHero = new PageHeroModel();
            }

            page.PageHero.GalleryImages = GetGalleryImages(page.Page.PageAliasPath);
            var viewModel = new PrimaryLandingPageModel
            {
                Page = page
            };

            if (experiences != null)
            {
                viewModel.Items = _mapper.Map<List<PrimaryLandingItemModel>>(experiences);
            }

            viewModel.BreadCrumbs = GetBreadCrumbs("Experiences");
            return viewModel;
        }
    }
}