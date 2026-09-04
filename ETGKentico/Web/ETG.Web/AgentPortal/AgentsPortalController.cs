using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Devotion.Web.Base.Providers;
using ETG.Data.AgentPortal.Models;
using ETG.Data.AgentPortal.Repositories;
using ETG.Data.Article.Models;
using ETG.Data.Article.Repositories;
using ETG.Web.AgentPortal.Models;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Models.Common;
using ETG.Web.Controllers.Base;
using ETG.Web.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace ETG.Web.AgentPortal
{
    public class AgentsPortalController : PageController<IAgentsPortalPageRepository, AgentPortalPageModel, AgentPortalPageViewModel>
    {

        private readonly IMapper _mapper;
        private readonly IAgentIncentiveRepository _agentIncentiveRepository;
        public AgentsPortalController(IMapper mapper, IAgentIncentiveRepository agentIncentiveRepository,
            IAgentsPortalPageRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _mapper = mapper;
            _agentIncentiveRepository = agentIncentiveRepository;
        }

        [HandleError]
        public ActionResult Incentive(string alias)
        {
            var model = _agentIncentiveRepository.GetIncentive(alias);

            if (model == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var viewModel = new AgentIncentivePageViewModel
            {
                Page = _mapper.Map<AgentIncentiveViewModel>(model),
                BreadCrumbs = new List<SimpleLinkViewModel>
                {
                    new SimpleLinkViewModel{Label = "Agent Portal", Url = "/agents"},
                    new SimpleLinkViewModel{Label = model.Name}
                }
            };
            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.DocumentID);
            return View("Incentive", viewModel);
        }
    }
}