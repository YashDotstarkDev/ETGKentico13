using Devotion.Data;
using Devotion.Web.Base.Extensions;
using ETG.Booking.Pricing.Services;
using ETG.Core.Http;
using ETG.Core.Services;
using ETG.Data.Destination.Repositories;
using ETG.Data.Experience.Repositories;
using ETG.Data.Repositories.Pages;
using ETG.Data.Services;
using ETG.Web.Helpers;
using ETG.Web.Models.Partial;
using ETG.Web.Tour.Models;
using PayPal.Api;
using System.Net;
using System.Web.Mvc;

namespace ETG.Web.Controllers.Partial
{
    public class NonPackageStickyController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICurrencyService _currencyService;
        private readonly IContactService _contactService;
        private readonly IHttpRequest _httpRequest;
        private readonly IDestinationPageRepository _destinationRepository;
        private readonly IExperiencePageRepository _experienceRepository;

        public NonPackageStickyController(ILogger logger, ICurrencyService currencyService, IContactService contactService, 
            IContainerPageRepository containerRepository, IHttpRequest httpRequest,
            IDestinationPageRepository destinationRepository,
            IExperiencePageRepository experienceRepository)
        {
            _logger = logger;
            _currencyService = currencyService;
            _contactService = contactService;
            _httpRequest = httpRequest;
            _destinationRepository = destinationRepository;
            _experienceRepository = experienceRepository;
        }


        [ChildActionOnly]
        public ActionResult GetNonPackageSticky()
        {
            NonPackageStickyViewModel viewModel = null;
            try
            {
                string host = HttpContext.Request.Url.Host;

                var contactInfo = _contactService.GetETGContactInfo();
                var currentCurrency = _currencyService.GetCurrentCurrency();

                var googleReview = GoogleReviewHelper.GetGoogleReviewViewModel().ReviewListingResponse;

                viewModel = new NonPackageStickyViewModel {
                    CurrentCountryCode = currentCurrency == "NZD" ? "NZ" : "AU",
                    PhoneNumberDisplay = currentCurrency == "NZD" ? contactInfo?.HeaderPhoneNz : contactInfo?.HeaderPhoneAu,
                    EnquiryUrl = GetEnquiryUrl(),
                    GoogleReview = googleReview ?? googleReview,
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogException("NonPackageStickyController", "GetNonPackageSticky", ex, string.Empty);
            }
            return View("Partial/Common/_NonPackageSticky", viewModel);

        }

        private string GetEnquiryUrl()
        {
            var path = _httpRequest.GetRequest().Url.PathAndQuery.GetUrlPathOnly();
            if (path.ToLowerInvariant().Contains("/destinations/"))
            { 
                var destination = _destinationRepository.Get(path);
                if (destination != null)
                {
                    return $"/destinations/enquire/{destination?.Destination?.PageAlias}";
                }
            }
            if (path.ToLowerInvariant().Contains("/experiences/"))
            {
                var experience = _experienceRepository.Get(path);
                if (experience != null)
                {
                    return $"/experiences/enquire/{experience?.Page?.PageAlias}";
                }
            }
            return $"/enquire";
        }
    }
}