using ETG.Core.Constants;
using ETG.Data.AgentPortal.Models;
using ETG.Data.Brochure.Repositories;
using ETG.Data.Brochure.Services;
using ETG.Data.Cache;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.AgentPortal.Repositories
{
    public class AgentsPortalPageRepository : BasePageRepository, IAgentsPortalPageRepository
    {
        private readonly ICacheService _cacheService;
        private readonly IAgentsPortalMainRepository _mainRepository;
        private readonly IAgentIncentiveRepository _agentIncentiveRepository;
        private readonly IAgentToolkitRepository _toolkitRepository;
        private readonly IAgentWebinarRepository _webinarRepository;
        private readonly IBrochureService _brochureService;
        public AgentsPortalPageRepository(IAgentsPortalMainRepository mainRepository, IAgentIncentiveRepository agentIncentiveRepository,
            IAgentToolkitRepository toolkitRepository, IAgentWebinarRepository webinarRepository,
            IBrochureService brochureService, ICacheService cacheService, IImageRepository imageRepository,
            IShareLinksService shareLinksService) : base(imageRepository, cacheService, shareLinksService)
        {
            _brochureService = brochureService;
            _webinarRepository = webinarRepository;
            _toolkitRepository = toolkitRepository;
            _agentIncentiveRepository = agentIncentiveRepository;
            _mainRepository = mainRepository;
            _cacheService = cacheService;
        }

        public AgentPortalPageModel Get(string url, string path = "")
        {
            return new AgentPortalPageModel
            {
                Brochures = _brochureService.GetAllBrochures(),
                AgentToolkits = _cacheService.GetDocumentDependentOnAll(() => _toolkitRepository.GetToolkits(PathConstants.AGENT_TOOLKITS), "GetToolkits", Core.PageTypes.Toolkit.CLASS_NAME),
                AgentIncentives = _cacheService.GetDocumentDependentOnAll(() => _agentIncentiveRepository.GetIncentives(PathConstants.AGENT_INCENTIVES), "GetIncentives", Core.PageTypes.Incentive.CLASS_NAME),
                AgentWebinars = _cacheService.GetDocumentDependentOnAll(() => _webinarRepository.GetWebinars(PathConstants.AGENT_WEBINARS), "GetIncentives", Core.PageTypes.Webinar.CLASS_NAME),
                Page = _cacheService.GetDocumentDependentOnPath(() => _mainRepository.GetAgentPortalPage(PathConstants.AGENT_PORTAL), "GetAgentPortalPage", PathConstants.AGENT_PORTAL),
               BreadCrumbs = GetBreadCrumbs("Agents' Portal")
        };
        }
    }
}
