using System.Linq;
using System.Web.Mvc;
using CMS.CustomTables;
using ETG.Core.CustomTables;
using ETG.Web.Controllers.Widgets;
using ETG.Web.Models.GoogleReviews;
using ETG.Web.Models.Widgets.GoogleReviewsWidget;
using ETG.WebAPI.Models.Reviews;
using Kentico.PageBuilder.Web.Mvc;
using Newtonsoft.Json;

[assembly: RegisterWidget("ETG.Web.Widget.GoogleReviewsWidget", typeof(GoogleReviewsWidgetController), "Google Reviews Widget")]
namespace ETG.Web.Controllers.Widgets
{
    public class GoogleReviewsWidgetController: WidgetController<GoogleReviewsWidgetProperties>
    {
        public ActionResult Index()
        {
            var googleReviewItem =
                   CustomTableItemProvider.GetItems<GoogleReviewsItem>().FirstOrDefault();

            var vm = new GoogleReviewsViewModel();


            vm.ReviewListingResponse = JsonConvert.DeserializeObject<ReviewListingResponse>(googleReviewItem.Reviews);

            return PartialView("Widgets/_GoogleReviewsWidget", vm);
        }
    }
}