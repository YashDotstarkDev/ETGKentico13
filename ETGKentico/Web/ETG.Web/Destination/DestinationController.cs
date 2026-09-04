using System.Linq;
using AutoMapper;
using Devotion.Web.Base.Providers;
using ETG.Core.Constants;
using ETG.Data.Destination.Models;
using ETG.Data.Destination.Repositories;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Controllers.Base;
using ETG.Web.Destination.Models;
using ETG.Web.Helpers;
using ETG.Web.Models.Pages;
using System.Web.Mvc;
using ETG.Data.Configuration;
using ETG.Data.Extensions;
using ETG.Data.Models.Forms;
using ETG.Data.Repositories.Forms;
using ETG.Data.Search;
using ETG.Web.Models.Common;
using ETG.Web.Models.Forms;
using ETG.Web.Models.Schemas;
using Newtonsoft.Json;
using ETG.Web.Article.Models;
using Devotion.Web.Base.Extensions;
using System.Web;

namespace ETG.Web.Destination
{
    public class DestinationController : PageController<IDestinationPageRepository, DestinationPageModel,
        DestinationPageViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDestinationPageRepository _repository;
        private readonly ISearchConfiguration _searchConfiguration;
        private readonly IApiKeyProvider _apiKeyProvider;
        private readonly IGenericEnquiryPageRepository _genericEnquiryPageRepository;

        public DestinationController(IMapper mapper,
            IDestinationPageRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider, ISearchConfiguration searchConfiguration,
            IApiKeyProvider apiKeyProvider, IGenericEnquiryPageRepository genericEnquiryPageRepository)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _mapper = mapper;
            _repository = repository;
            _searchConfiguration = searchConfiguration;
            _apiKeyProvider = apiKeyProvider;
            _genericEnquiryPageRepository = genericEnquiryPageRepository;
        }

        [HandleError]
        public ActionResult Destination()
        {
            var currentUrl = HttpContext.Request.Url?.PathAndQuery.GetUrlPathOnly();

            if (_repository.IsDestinationUnpublished(currentUrl))
            {
                return Redirect("/destinations");
            }

            var model = _repository.Get(currentUrl);

            var viewModel = _mapper.Map<DestinationPageViewModel>(model);

            if (viewModel == null)
            {
                throw new HttpException(404, "Page not found");

            }
            ProcessBeforeReturningView(viewModel);
            return View("Index", viewModel);
        }

        protected override void ProcessBeforeReturningView(DestinationPageViewModel viewModel)
        {
            // Generate schema
            if (viewModel != null)
            {
                var faqSchema = new FAQSchema
                {
                    Context = "https://schema.org",
                    Type = "FAQPage",
                    MainEntity = viewModel?.HelpfulInformation.Select(f => new MainEntity
                    {
                        Type = "Question",
                        Name = f.Heading,
                        AcceptedAnswer = new AcceptedAnswer { Type = "Answer", Text = f.Contents.StripHtml() }
                    }).ToList()
                };

                if (viewModel.Destination != null)
                {
                    viewModel.Destination.JsonSchema = JsonConvert.SerializeObject(faqSchema);
                }

                viewModel.TourSearchIndex = _searchConfiguration.IndexTour;

                viewModel.ConnectWithUs = new ConnectWithUsViewModel { EnquireNowCta = "Enquire about Destinations", EnquireNowCtaUrl = $"/destinations/enquire/{viewModel?.Destination?.PageAlias}"};
            }

            if (viewModel?.Destination != null)
            {
                PageHelper.InitializePageBuilder(HttpContext, viewModel.Destination.DocumentID);
            }
        }

        public ActionResult Landing()
        {
            var viewModel =
                _mapper.Map<PrimaryLandingPageViewModel>(_repository.GetLandingPage(PathConstants.PATH_DESTINATIONS));


            if (!string.IsNullOrWhiteSpace(viewModel.Page.RedirectTo))
            {
                return Redirect(viewModel.Page.RedirectTo);
            }

            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View("Landing", viewModel);
        }

        public ActionResult Enquire(string alias)
        {
            var destination = _repository.Get($"/destinations/{alias}");


            var viewModel = GetViewModel();
            if (destination !=null)
            {
                //viewModel.Page.PageHero = _mapper.Map<PageHeroViewModel>(destination?.Hero);
                viewModel.Page.PageHero.GalleryImages = null;
                viewModel.Form = _mapper.Map<GenericEnquiryFormViewModel>(new GenericEnquiryFormModel(){ PreferredDestination = destination.Destination.BasicInfo.Name});

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