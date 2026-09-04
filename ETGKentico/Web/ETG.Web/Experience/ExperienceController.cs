using AutoMapper;
using Devotion.Web.Base.Providers;
using DevTrends.MvcDonutCaching;
using ETG.Core.Constants;
using ETG.Data.Experience.Models;
using ETG.Data.Experience.Repositories;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Controllers.Base;
using ETG.Web.Experience.Models;
using ETG.Web.Helpers;
using ETG.Web.Models.Pages;
using System.Web.Mvc;
using ETG.Data.Configuration;
using ETG.Data.Models.Forms;
using ETG.Data.Repositories.Forms;
using ETG.Web.Models.Common;
using ETG.Web.Models.Forms;

namespace ETG.Web.Experience
{
    public class
        ExperienceController : PageController<IExperiencePageRepository, ExperiencePageModel, ExperiencePageViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IExperiencePageRepository _repository;
        private readonly IApiKeyProvider _apiKeyProvider;
        private readonly IGenericEnquiryPageRepository _genericEnquiryPageRepository;

        public ExperienceController(IMapper mapper,
            IExperiencePageRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider,
            IApiKeyProvider apiKeyProvider,
            IGenericEnquiryPageRepository genericEnquiryPageRepository)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _mapper = mapper;
            _repository = repository;
            _apiKeyProvider = apiKeyProvider;
            _genericEnquiryPageRepository = genericEnquiryPageRepository;
        }

        protected override void ProcessBeforeReturningView(ExperiencePageViewModel viewModel)
        {
            if (viewModel != null)
            {
                viewModel.ConnectWithUs = new ConnectWithUsViewModel
                {
                    EnquireNowCta = "Enquire about Experiences",
                    EnquireNowCtaUrl = $"/experiences/enquire/{viewModel.Page?.PageAlias}"
                };

                PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.DocumentID);
            }
        }

        public ActionResult Landing()
        {
            var viewModel =
                _mapper.Map<PrimaryLandingPageViewModel>(_repository.GetLandingPage(PathConstants.PATH_EXPERIENCES));

            if (!string.IsNullOrWhiteSpace(viewModel.Page.RedirectTo))
            {
                return Redirect(viewModel.Page.RedirectTo);
            }

            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View("Landing", viewModel);
        }

        public ActionResult Enquire(string alias)
        {
            var experience = _repository.Get($"/experiences/{alias}");


            var viewModel = GetViewModel();
            if (experience != null)
            {
                viewModel.Page.PageHero = _mapper.Map<PageHeroViewModel>(experience?.Hero);
                viewModel.Page.PageHero.GalleryImages = null;
                viewModel.Form = _mapper.Map<GenericEnquiryFormViewModel>(new GenericEnquiryFormModel()
                    { PreferredExperience = experience.Page.SummaryInfo.Name });
            }

            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View("~/Views/Enquire/Index.cshtml", viewModel);
        }

        private GenericEnquiryPageViewModel GetViewModel(GenericEnquiryFormViewModel formModel = null)
        {
            ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
            ViewBag.FormAction = "enquire";
            var model = _genericEnquiryPageRepository.Get("/Enquire");

            var viewModel = _mapper.Map<GenericEnquiryPageViewModel>(model);

            if (formModel != null)
            {
                viewModel.Form = _mapper.Map<GenericEnquiryFormViewModel>(formModel);
            }

            return viewModel;
        }
    }
}