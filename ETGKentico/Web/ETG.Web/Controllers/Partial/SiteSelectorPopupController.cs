using ETG.Core.PageTypes.Providers;
using ETG.Core.Services;
using ETG.Web.Models.Partial;
using System.Linq;
using System.Web.Mvc;
using ETG.Core.Http;
using PayPal.Api;

namespace ETG.Web.Controllers.Partial
{
    public class SiteSelectorPopupController : Controller
    {
        private readonly ILogger _logger;
        private readonly System.Web.HttpRequest _httpRequest;

        public SiteSelectorPopupController(ILogger logger, IHttpRequest httpRequest)
        {
            _logger = logger;
            _httpRequest = httpRequest.GetRequest();
        }


        [ChildActionOnly]
        public ActionResult GetSiteSelector()
        {
            SiteSelectorPopupModel siteSelectorModel = null;
            try
            {
                var properties  = SiteSelectorPopupProvider.GetSiteSelectorPopups().FirstOrDefault();
                if (properties != null)
                {
                    siteSelectorModel = new SiteSelectorPopupModel { 
                        Title = properties.GetStringValue("Title", string.Empty),
                        CtaTextAu = properties.GetStringValue("CtaTextAu", string.Empty),
                        CtaTextNz = properties.GetStringValue("CtaTextNz", string.Empty),
                        DomainNz = properties.GetStringValue("DomainNz", string.Empty)
                    };

                    siteSelectorModel.GetCountryEndpoint = $"{(_httpRequest.IsSecureConnection ? "https" : "http")}://{_httpRequest.Url.Host}/api/getcountry";
                    siteSelectorModel.IsNz = _httpRequest.Url.Host.ToLowerInvariant().Contains("nz");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogException("SiteSelectorPopupController", "GetSiteSelector", ex, string.Empty);
            }
            return View("Partial/SiteSelector/_SiteSelectorPopup", siteSelectorModel);

        }
    }
}