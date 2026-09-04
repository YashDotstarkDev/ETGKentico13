using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Castle.Core.Internal;
using CMS.ContactManagement;
using CMS.Core;
using CMS.EventLog;
using CMS.Helpers;
using Devotion.Web.Base.Attributes;
using Devotion.Web.Base.Extensions;
using ETG.Booking.Pricing.Enums;
using ETG.Booking.Pricing.Services;
using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Destination.Models;
using ETG.Data.Destination.Services;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Image;
using ETG.Data.Search;
using ETG.Data.Services;
using ETG.Data.Settings;
using ETG.Data.Tour;
using ETG.Web.Models.Common;
using ETG.Web.Models.Competition;
using ETG.Web.Models.Forms;
using ETG.Web.Models.Layout;
using ETG.Web.Models.PageTypes;
using ETG.Web.Services.Menu.Cached;
using Ninject.Activation;

namespace ETG.Web.Controllers.Partial
{
    public class MasterPageController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICachedMenuService _menuService;
        private readonly IContactService _contactService;
        private readonly IKenticoContactService _kenticoContactService;
        private readonly IImageRepository _imageRepository;
        private readonly ICacheService _cacheService;
        private readonly IDestinationService _destinationService;
        private readonly IConsentService _consentService;
        private readonly IETGSettingsService _settingsService;
        private readonly IHomepageRepository _homepageRepository;
        private readonly ISearchConfiguration _searchConfiguration;
        private readonly ICurrencyService _currencyService;
        public MasterPageController(IMapper mapper, ICachedMenuService menuService, IContactService contactService,
            IDestinationService destinationService, IKenticoContactService kenticoContactService,
            IImageRepository imageRepository, IConsentService consentService, ICacheService cacheService,
            IETGSettingsService settingsService, IHomepageRepository homepageRepository, ISearchConfiguration searchConfiguration, ICurrencyService currencyService)
        {
            _consentService = consentService;
            _menuService = menuService;
            _contactService = contactService;
            _mapper = mapper;
            _kenticoContactService = kenticoContactService;
            _cacheService = cacheService;
            _imageRepository = imageRepository;
            _destinationService = destinationService;
            _settingsService = settingsService;
            _homepageRepository = homepageRepository;
            _searchConfiguration = searchConfiguration;
            _currencyService = currencyService;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Agree")]
        public ActionResult Agree()
        {
            var currentCookieLevelProvider = Service.Resolve<ICurrentCookieLevelProvider>();
            currentCookieLevelProvider.SetCurrentCookieLevel(CookieLevel.All);
            _consentService.Agree(ContactManagementContext.GetCurrentContact());
            return PartialView("Partial/MasterPage/_gdprResult");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Decline")]
        public ActionResult Decline()
        {
            var currentCookieLevelProvider = Service.Resolve<ICurrentCookieLevelProvider>();
            currentCookieLevelProvider.SetCurrentCookieLevel(CookieLevel.None);
            _consentService.Decline(ContactManagementContext.GetCurrentContact());
            return PartialView("Partial/MasterPage/_gdprResult");
        }

        [HttpPost]
        public ActionResult Consent(string submitbutton)
        {
            switch (submitbutton)
            {
                case "accept":
                    _consentService.Agree(ContactManagementContext.GetCurrentContact());
                    break;
                case "decline":
                    _consentService.Decline(ContactManagementContext.GetCurrentContact());
                    break;
                default:
                    return null;
            }

            return PartialView("Partial/MasterPage/_gdprResult");
        }

        [ChildActionOnly]
        public ActionResult MainHeader()
        {
            var mainMenus = _menuService.GetMainMenuViewModel();
            var urlPath = Request.RawUrl.GetUrlPathOnly();

            var arr = urlPath.Split('/');
            DestinationSummaryModel destination = null;

            if ((arr.Length == 3 || arr.Length == 4) && arr[1].ToLower().Equals("destinations"))
            {
                destination = _destinationService.GetDestinationByNodeAlias(arr[2]);
            }
            var contactInfo = _contactService.GetETGContactInfo();
            var showConsent = Service.Resolve<ICurrentCookieLevelProvider>().GetCurrentCookieLevel() == 0;
            var currentCurrency = _currencyService.GetCurrentCurrency();
            var currencyPricing = new CurrentCurrencyPricing(_currencyService);

            string host = HttpContext.Request.Url.Host; 


            return View("Partial/MasterPage/_MainHeader", new HeaderViewModel
            {
                Destination = destination?.Name,
                MainLinks = mainMenus,
                PeaceOfMindUrl = contactInfo?.PeaceOfMindUrl,
                SafeTravelUrl = contactInfo?.SafeTravelUrl,
                FreedomOfChoiceUrl = contactInfo?.FreedomOfChoiceUrl,
                BookNowUrl = contactInfo?.BookNowUrl,
                GDPRConsent = new GDPRConsentViewModel
                {
                    ShowGDPRConsent = showConsent,
                    ConsentContent = _consentService.GetConsentText()
                },
                TourSearchIndex = _searchConfiguration.IndexTour,
                Currencies = _currencyService.GetSupportedCurrencies().Where(a => !a.Equals(currentCurrency)).OrderBy(a => a).ToList(),
                CurrentCurrency = currentCurrency,
                ConversionRate = _currencyService.GetConversionRate(currentCurrency),
                CurrentCurrencySymbol = CurrencyConstants.GetSymbol(currentCurrency),
                CurrencyApplyDiscounts = currencyPricing.CurrencyAppliesDiscounts,
                CurrentCountryCode = host.ToLowerInvariant().Contains("nz") ? "NZ" : "AU",
                HeaderPhoneNumber = host.ToLowerInvariant().Contains("nz") ? contactInfo?.HeaderPhoneNz : contactInfo?.HeaderPhoneAu
            });
        }

        [ChildActionOnly]
        public ActionResult MainFooter()
        {
            var contactDetails = _mapper.Map<ContactViewModel>(_contactService.GetETGContactInfo());
            var viewModel = new FooterViewModel
            {
                Contact = contactDetails,
                TopMenus = _menuService.GetFooterTopMenus(),
                BottomMenus = _menuService.GetFooterBottomMenus(),
                BottomLogos = _mapper.Map<List<ImageViewModel>>(
                    _cacheService.GetDocumentDependentOnChildrenPath(
                        () => _imageRepository.GetImages(PathConstants.PATH_BRAND_LOGOS), "GetBrandLogos",
                        PathConstants.PATH_BRAND_LOGOS)
                )
            };

            return View("Partial/MasterPage/_MainFooter", viewModel);
        }

        [ChildActionOnly]
        public ActionResult MainFooterProofPoints()
        {
            return View("Partial/MasterPage/_MainFooterProofPoints", _menuService.GetFooterProofPoints());
        }

        [ChildActionOnly]
        public ActionResult MainFooterSubscription()
        {
            var homepage = _mapper.Map<HomepageViewModel>(_cacheService.GetDocumentDependentOnPath(
                () => _homepageRepository.GetHomepage("/home"), "home",
                "/home"));
            var viewModel = new NewsletterSubscriptionViewModel();
            if (homepage.PopupCompetition != null && homepage.PopupCompetition.ShowCompetitionPopup)
            {
                viewModel.CompetitionForm = new PopupCompetitionViewModel
                {
                    CompetitionCookieName = homepage.PopupCompetition.CompetitionCookieName,
                    CompetitionTermsUrl = homepage.PopupCompetition.CompetitionTermsUrl,
                    PopupTitle = homepage.PopupCompetition.PopupTitle,
                    PopupSubTitle = homepage.PopupCompetition.PopupSubTitle,
                    ShowCompetitionPopup = homepage.PopupCompetition.ShowCompetitionPopup
                };
            }
            return View("Partial/MasterPage/_MainFooterSubscription", viewModel);
        }

        [ChildActionOnly]
        public ActionResult HeadScripts()
        {
            return View("Partial/MasterPage/_HeadScripts", _settingsService.GetSettings());

        }

        [ChildActionOnly]
        public ActionResult BodyTopScripts()
        {
            return View("Partial/MasterPage/_BodyTopScripts", _settingsService.GetSettings());

        }

        [HttpPost]
        public ActionResult NewsletterSubmit(NewsletterSubscriptionViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            if (ModelState.IsValid)
            {
                if (model.Email.IsNullOrEmpty())
                {
                    return null;
                }

                _kenticoContactService.AddContact(model.Email, model.IsAgent);
                return PartialView("Partial/Common/_SubscriptionThankYou");
            }

            return PartialView("Partial/Common/_DisplayMessage", string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)));
        }

        [ChildActionOnly]
        public ActionResult CompetitionPopup()
        {
            if (Session["subscribepopup"] != null)
            {
                return View("Partial/Home/_HomepageCompetitionPopup", new PopupCompetitionViewModel());
            }
            var homepage = _mapper.Map<HomepageViewModel>(_cacheService.GetDocumentDependentOnPath(
                () => _homepageRepository.GetHomepage("/home"), "home",
                "/home"));

            if (homepage.PopupCompetition != null && homepage.PopupCompetition.ShowCompetitionPopup)
            {
                var viewModel = new PopupCompetitionViewModel
                {
                    CompetitionCookieName = homepage.PopupCompetition.CompetitionCookieName,
                    CompetitionTermsUrl = homepage.PopupCompetition.CompetitionTermsUrl,
                    PopupTitle = homepage.PopupCompetition.PopupTitle,
                    PopupSubTitle = homepage.PopupCompetition.PopupSubTitle,
                    ShowCompetitionPopup = homepage.PopupCompetition.ShowCompetitionPopup
                };
                return View("Partial/Home/_HomepageCompetitionPopup", viewModel);
            }
            return View("Partial/Home/_HomepageCompetitionPopup", new PopupCompetitionViewModel());
        }

        public ActionResult ChangeCurrency()
        {
            return Redirect(HttpContext.Request.UrlReferrer == null ? "/" : HttpContext.Request.UrlReferrer.PathAndQuery);
        }
    }
}