using System.Collections.Generic;
using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using ETG.Data.Article.Services;
using ETG.Data.Brochure.Services;
using ETG.Data.Cache;
using ETG.Data.Destination.Services;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Helpers;
using ETG.Data.Models.Common;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Base;
using ETG.Data.Services;
using ETG.Data.Tour.Services;
using System.Linq;
using ETG.Data.Repositories.Image;
using ETG.Data.Search;
using ETG.Data.Tour.Models;

namespace ETG.Data.DestinationExpertTeam.Repositories
{
    public class DestinationExpertTeamPageRepository : BasePageRepository, IDestinationExpertTeamPageRepository
    {
        private readonly ICacheService _cacheService;
        private readonly IDestinationExpertTeamRepository _expertTeamRepository;
        private readonly ITourService _tourService;
        private readonly IDestinationService _destinationService;
        private readonly IArticleService _articleService;
        private readonly IBrochureService _brochureService;
        private readonly ISearchConfiguration _searchConfiguration;
        public DestinationExpertTeamPageRepository(IDestinationExpertTeamRepository expertTeamRepository, ITourService tourService,
            IDestinationService destinationService,
            ICacheService cacheService, IImageRepository imageRepository, IArticleService articleService,
            IShareLinksService shareLinksService, IBrochureService brochureService, ISearchConfiguration searchConfiguration) : base(imageRepository, cacheService, shareLinksService)
        {
            _cacheService = cacheService;
            _expertTeamRepository = expertTeamRepository;
            _tourService = tourService;
            _destinationService = destinationService;
            _articleService = articleService;
            _brochureService = brochureService;
            _searchConfiguration = searchConfiguration;
        }
        public DestinationExpertTeamPageModel Get(string url, string path = "")
        {
            var expertTeam = _expertTeamRepository.GetExpertTeam(url);

            if (expertTeam?.SummaryInfo == null)
            {
                return null;
            }
            var pageModel = new DestinationExpertTeamPageModel
            {
                DestinationExpert = expertTeam,

            };

            var guidList = expertTeam.SummaryInfo.Destinations.ToGuidList(';');

            var destinations = _destinationService.GetDestinations(guidList).ToList();
            var destinationGuids = destinations.Select(a => a.DestinationGuid).ToList();

            if (!expertTeam.SummaryInfo.FavouriteTourCode.IsNullOrEmpty())
            {
                pageModel.FavouriteTour = _tourService.GetTourByTourCode(expertTeam.SummaryInfo.FavouriteTourCode)?.TourSummaryInfo;
            }
            List<TourSummaryInfoModel> featureTours = null;
            if (!expertTeam.FeatureTourCodes.IsNullOrEmpty() || !expertTeam.FeatureTourCodes.IsNullOrEmpty())
            {
                var codes = ListUtility.GetCombinedList(';', expertTeam.FeatureTourCodes,
                    expertTeam.FeatureCruiseCodes);
                featureTours = _tourService.GetTiledTours(codes);
            }
            else
            {
                featureTours = _tourService.GetTiledToursByDestinations(destinationGuids, 8);
            }

            if (!expertTeam.FeatureTourCodes.IsNullOrEmpty())
            {
                featureTours = _tourService.GetTiledTours(expertTeam.FeatureTourCodes.Split(';').ToList());
            }

            pageModel.FeatureTours = new TourListingModel
            {
                Tours = featureTours,
                ViewAllUrl = SearchUrlHelper.GetDestinationFilterUrl(_searchConfiguration.IndexTour,string.Join(";", destinations.Select(a => a.Name)))
            };
            var subRegions = _destinationService.GetDestinationRegions(destinationGuids);
            if (subRegions != null)
            {
                pageModel.RegionMapItems = subRegions.Select(a => new MapItemModel
                {
                    Name = a.Name,
                    Summary = a.Summary,
                    Longitude = a.Longitude,
                    Latitude = a.Latitude,
                    Url = a.Path,

                }).ToList();
            }

            if (!destinationGuids.IsNullOrEmpty())
            {
                pageModel.Brochure = _brochureService.GetBrochureByDestinationGuid(destinationGuids[0]);
            }

            pageModel.RelatedArticles = _articleService.GetRelatedArticlesByDestinations(destinationGuids);
            pageModel.BreadCrumbs =
                GetBreadCrumbs("Destination experts", "/destination-experts", expertTeam.SummaryInfo.Name);
            return pageModel;
        }
    }
}
