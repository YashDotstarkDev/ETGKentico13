using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.DocumentEngine;
using CMS.Newsletters;
using CMS.SiteProvider;
using Devotion.Web.Base.Extensions;
using ETG.Core.Http;
using ETG.Web.Helpers;
using ETG.Web.Models.Forms;
using ETG.Web.Models.Newsletter;
using ETG.Web.Controllers.Base;
using ETG.Data.Services;
using ETG.Data.Configuration;
using AutoMapper;
using ETG.Data.Repositories;
using ETG.Data.Models.Forms;
using CMS.CustomTables;
using ETG.Core.CustomTables;
using System.Collections.Generic;
using CMS.Helpers;
using ETG.Core.PageTypes;
using CMS.EventLog;

namespace ETG.Web.Controllers
{
    public class NewsletterController : FormController
    {
        private readonly IMapper _mapper;
        private readonly IEmailHashValidator _emailHashValidatorService;
        private readonly IUnsubscriptionProvider _unsubscriptionProvider;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IHttpRequest _httpRequest;
        private readonly IApiKeyProvider _apiKeyProvider;
        private readonly INewsletterRepository _newsletterRepository;
        private readonly IKenticoContactService _contactService;

        public NewsletterController(IMapper mapper, INewsletterRepository newsletterRepository, IHttpRequest httpRequest, IApiKeyProvider apiKeyProvider, IKenticoContactService contactService) : base(contactService)
        {
            _mapper = mapper;
            _emailHashValidatorService = CMS.Core.Service.Resolve<IEmailHashValidator>();
            _unsubscriptionProvider = CMS.Core.Service.Resolve<IUnsubscriptionProvider>();
            _subscriptionService = CMS.Core.Service.Resolve<ISubscriptionService>();
            _httpRequest = httpRequest;
            _apiKeyProvider = apiKeyProvider;
            _newsletterRepository = newsletterRepository;
            _contactService = contactService;
        }

        public ActionResult Unsubscribe(Models.Newsletter.UnsubscribeViewModel model)
        {
            var viewModel = GetViewModel();
            if (viewModel == null)
            {
                throw new HttpException(404, "Page not found");
            }
            if (model != null)
            {
                viewModel.Form.Email = model.Email;
                viewModel.Form.NewsletterGuid = model.NewsletterGuid;
                viewModel.Form.IssueGuid = model.IssueGuid;
                viewModel.Form.Hash = model.Hash;
                viewModel.Form.UnsubscribeFromAll = model.UnsubscribeFromAll;
            }
            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UnsubscribeNewsletter(UnsubscribeViewModel model)
        {
            // Verifies that the unsubscription request contains all required parameters
            if (ModelState.IsValid)
            {
                // Confirms whether the hash in the unsubscription request is valid for the given email address
                // Provides protection against forged unsubscription requests
                if (_emailHashValidatorService.ValidateEmailHash(model.Hash, model.Email))
                {

                    var nonCampaignNewsletters = NewsletterInfo.Provider.Get().WhereNotLike(nameof(NewsletterInfo.NewsletterName), "campaign%");
                    int? issueId = IssueInfo.Provider.Get(model.IssueGuid, SiteContext.CurrentSiteID)?.IssueID;

                    ISubscriptionService subscriptionService = CMS.Core.Service.Resolve<ISubscriptionService>();
                    foreach (var newsletter in nonCampaignNewsletters)
                    {
                        if (newsletter != null)
                        {
                            if (!subscriptionService.IsUnsubscribed(model.Email, newsletter.NewsletterID))
                            {
                                if (newsletter.NewsletterGUID == model.NewsletterGuid)
                                {
                                    subscriptionService.UnsubscribeFromSingleNewsletter(model.Email, newsletter.NewsletterID, issueId, sendConfirmationEmail: false);
                                }
                                else
                                {
                                    subscriptionService.UnsubscribeFromSingleNewsletter(model.Email, newsletter.NewsletterID, null, sendConfirmationEmail: false);
                                }
                            }
                        }
                    }

                    _contactService.Unsubscribe(model.Email, model.ReasonId.ToString());

                    // Displays a view to inform the user that they were unsubscribed
                    return Redirect("/unsubscribed");
                }
            }

            // If the unsubscription was not successful, displays a view to inform the user
            // Failure can occur if the request does not provide all required parameters or if the hash is invalid
            return Redirect("/unsubscription-failed");
        }

        #region "Private"

        private UnsubscribePageViewModel GetViewModel()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            ViewBag.RecaptchaSiteKey = _apiKeyProvider.RecaptchaV3SiteKey;
            ViewBag.FormAction = "unsubscribe";
            var model = _newsletterRepository.GetUnsubscribePage("/Newsletter/Unsubscribe");


            var viewModel = _mapper.Map<UnsubscribePageViewModel>(model);

            var resons = CustomTableItemProvider.GetItems<UnsubscribeReasonsItem>().ToList();
            if (resons.Any())
            {
                selectListItems = resons.Select(p => new SelectListItem
                {
                    Value = ValidationHelper.GetString(p.GetValue("ItemID"), string.Empty),
                    Text = ValidationHelper.GetString(p.GetValue("Reason"), string.Empty)
                }).ToList();

                viewModel.Form.Reasons = selectListItems;
                viewModel.Form.ReasonId = 1;

            }

            return viewModel;
        }

        #endregion
    }
}