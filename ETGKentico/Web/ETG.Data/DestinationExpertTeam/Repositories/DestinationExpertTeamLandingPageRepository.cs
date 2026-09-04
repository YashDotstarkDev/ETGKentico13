using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Repositories;
using System.Collections.Generic;
using ETG.Data.Cache;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.DestinationExpertTeam.Repositories
{
    public class DestinationExpertTeamLandingPageRepository : BasePageRepository,
        IDestinationExpertTeamLandingPageRepository
    {
        private readonly ICacheService _cacheService;
        private readonly IContainerRepository _containerRepository;
        private readonly IDestinationExpertTeamRepository _expertTeamRepository;

        public DestinationExpertTeamLandingPageRepository(IContainerRepository containerRepository,
            IDestinationExpertTeamRepository expertTeamRepository, ICacheService cacheService,
            IImageRepository imageRepository,
            IShareLinksService shareLinksService) : base(imageRepository, cacheService, shareLinksService)
        {
            _cacheService = cacheService;
            _expertTeamRepository = expertTeamRepository;
            _containerRepository = containerRepository;
        }

        public DestinationExpertTeamLandingPageModel Get(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _containerRepository.GetContainer(url),
                "expertslandingpage", url);

            if (page == null)
            {
                return null;
            }

            var expertTeams =
                _cacheService.GetDocumentDependentOnChildrenPath<List<DestinationExpertTeamSummaryModel>>(
                    () => _expertTeamRepository.GetExpertTeams(url), "expertslandingitems", url);

            if (expertTeams == null)
            {
                return null;
            }

            var pageModel = new DestinationExpertTeamLandingPageModel
            {
                Page = page,
                DestinationExperts = expertTeams,
                BreadCrumbs = GetBreadCrumbs("Destination experts")
            };

            return pageModel;
        }
    }
}