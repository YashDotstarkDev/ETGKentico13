using CMS.CustomTables;
using ETG.Core.CustomTables;
using ETG.Web.Models.GoogleReviews;
using ETG.WebAPI.Models.Reviews;
using Newtonsoft.Json;
using System.Linq;

namespace ETG.Web.Helpers
{
    public static class GoogleReviewHelper
    {
        public static GoogleReviewsViewModel GetGoogleReviewViewModel()
        {
            var googleReviewItem = CustomTableItemProvider.GetItems<GoogleReviewsItem>().FirstOrDefault();
            var vm = new GoogleReviewsViewModel();
            vm.ReviewListingResponse = JsonConvert.DeserializeObject<ReviewListingResponse>(googleReviewItem.Reviews);
            return vm;
        }
    }
}