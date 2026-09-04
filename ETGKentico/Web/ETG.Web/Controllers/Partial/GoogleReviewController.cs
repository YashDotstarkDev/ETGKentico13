using ETG.Core.PageTypes.Providers;
using ETG.Core.Services;
using ETG.Web.Models.Partial;
using System.Linq;
using System.Web.Mvc;
using ETG.Core.Http;
using PayPal.Api;
using CMS.CustomTables;
using ETG.Core.CustomTables;
using ETG.Web.Models.GoogleReviews;
using ETG.WebAPI.Models.Reviews;
using Newtonsoft.Json;
using ETG.Web.Helpers;

namespace ETG.Web.Controllers.Partial
{
    public class GoogleReviewController : Controller
    {
        private readonly ILogger _logger;
        private readonly System.Web.HttpRequest _httpRequest;

        public GoogleReviewController(ILogger logger, IHttpRequest httpRequest)
        {
            _logger = logger;
            _httpRequest = httpRequest.GetRequest();
        }


        [ChildActionOnly]
        public ActionResult GetGoogleReviews()
        {
            var googleReviewItem =
                   CustomTableItemProvider.GetItems<GoogleReviewsItem>().FirstOrDefault();

            var vm = new GoogleReviewsViewModel();


            vm.ReviewListingResponse = GoogleReviewHelper.GetGoogleReviewViewModel().ReviewListingResponse;

            return View("Widgets/_GoogleReviewsWidget", vm);

        }
    }
}