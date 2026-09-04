using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CMS.DocumentEngine;
using ETG.Core.PageTypes;
using ETG.Data.Article.Models;
using ETG.Data.Article.Services;
using ETG.Data.Cache;
using ETG.Data.Helpers;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.Pages;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Search;
using ETG.Data.Services;
using ETG.Data.Tour.Models;
using ETG.Data.TravelType.Models;
using ETG.Data.TravelType.Services;
using DateTimeHelper = CMS.Helpers.DateTimeHelper;

namespace ETG.Data.TravelType.Repositories
{
    public class TravelTypeDetailPageRepository : BasePageRepository, ITravelTypeDetailPageRepository
    {
        private readonly ITravelTypeService _travelTypeService;
        private readonly IArticleService _articleService;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public TravelTypeDetailPageRepository(ITravelTypeService travelTypeService,
            IShareLinksService shareLinksService, IImageRepository imageRepository, ICacheService cacheService,
            IArticleService articleService,
            IMapper mapper) : base(imageRepository, cacheService,
            shareLinksService)
        {
            _travelTypeService = travelTypeService;
            _articleService = articleService;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public TravelTypeDetailPageModel Get(string url, string alias = "")
        {
            var travelTypeDetail = _travelTypeService.GetTravelTypeDetail(url);
            if (travelTypeDetail?.BasicInfo == null)
            {
                return null;
            }

            var travelTypeModel = new TravelTypeDetailPageModel
            {
                Detail = travelTypeDetail,
                Hero = new PageHeroModel
                {
                    Heading = travelTypeDetail.BasicInfo.Heading,
                    HeroImage = travelTypeDetail.BasicInfo.HeroImage,
                    HeroImageAltText = travelTypeDetail.BasicInfo.HeroAltText,
                    LargeHeading = true,
                    GalleryImages = GetGalleryImages(travelTypeDetail.PageAliasPath),
                    HeroIconImage = travelTypeDetail.BasicInfo.HeroIconClass,
                    HeroIconSvg = travelTypeDetail.BasicInfo.HeroIconSvg,
                    ShareLinks = GetShareLinks(travelTypeDetail.PageAliasPath)
                },
            };
            var relatedArticles = _articleService.GetRelatedArticlesByTravelType(travelTypeDetail.NodeGuid);
            

            if (relatedArticles?.Count() > 0)
            {
                travelTypeModel.RelatedArticles = new ArticleListingModel()
                {
                    Title = $"READ WHAT WE'VE WRITTEN ON {travelTypeModel.Detail.BasicInfo.Heading}",
                    Articles = relatedArticles,
                    ViewAllUrl = "/search"
                };
            }

            if (!string.IsNullOrWhiteSpace(travelTypeDetail.BasicInfo.ThemedPackagesUrl))
            {

                var themedPackages = _travelTypeService.GetThemedPackages(travelTypeDetail.BasicInfo.ThemedPackagesUrl);
                if (themedPackages?.Count() > 0)
                {
                    travelTypeModel.ThemedPackagesInfo = new ThemedPackagesListingModel
                    {
                        Heading = travelTypeDetail.BasicInfo.ThemedPackagesHeading,
                        ThemedPackages = themedPackages.ToList()
                    };
                }
            }

            var links = travelTypeDetail.PageAliasPath.Split('/');
            if (links.Length >= 3)
            {
                travelTypeModel.BreadCrumbs =
                    GetBreadCrumbs(links[1].Replace('-', ' '), "/" + links[1].ToLower(),
                        travelTypeDetail.BasicInfo.Name);
            }


            return travelTypeModel;
        }

        public TravelTypeLandingModel GetLandingPage(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _travelTypeService.GetLanding(url),
                "traveltypelanding", url);

            if (page == null)
            {
                return null;
            }

            if (page.PageHero != null)
            {
                page.PageHero.GalleryImages = GetGalleryImages(url);
                page.PageHero.ShareLinks =
                    GetShareLinks(url, page.PageHero.HeroImage);
            }

            var travelTypes =
                _cacheService.GetDocumentDependentOnChildrenPath(() => _travelTypeService.GetTravelTypeDetails(url),
                    "travelTypeChildren", url);

            page.TravelTypeDetails = travelTypes;

            page.BreadCrumbs = GetBreadCrumbs(page.Name);


            //viewModel.BreadCrumbs = GetBreadCrumbs("Experiences");
            return page;
        }
    }
}