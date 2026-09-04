using AutoMapper;
using Devotion.Web.Base.Extensions;
using Devotion.Web.Base.Providers;
using DevTrends.MvcDonutCaching;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.DestinationExpertTeam.Repositories;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Controllers.Base;
using ETG.Web.DestinationExpertTeam.Models;
using ETG.Web.Helpers;
using System.Web.Mvc;

namespace ETG.Web.DestinationExpertTeam
{
    public class DestinationExpertTeamController : PageController<IDestinationExpertTeamPageRepository,
        DestinationExpertTeamPageModel, DestinationExpertTeamPageViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDestinationExpertTeamLandingPageRepository _landingRepository;

        public DestinationExpertTeamController(IMapper mapper, IDestinationExpertTeamPageRepository repository,
            IDestinationExpertTeamLandingPageRepository landingRepository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _mapper = mapper;
            _landingRepository = landingRepository;
        }


        protected override void ProcessBeforeReturningView(DestinationExpertTeamPageViewModel viewModel)
        {
            if (viewModel?.DestinationExpert != null)
            {
                PageHelper.InitializePageBuilder(HttpContext, viewModel.DestinationExpert.DocumentID);
            }
        }
        
        public ActionResult Landing()
        {
            var viewModel =
                _mapper.Map<DestinationExpertTeamLandingPageViewModel>(
                    _landingRepository.Get(HttpContext.Request.Url?.PathAndQuery.GetUrlPathOnly()));

            if (!string.IsNullOrWhiteSpace(viewModel.Page.RedirectTo))
            {
                return Redirect(viewModel.Page.RedirectTo);
            }

            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View("Landing", viewModel);
        }
    }
}