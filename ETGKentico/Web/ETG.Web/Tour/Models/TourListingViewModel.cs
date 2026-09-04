using System.Collections.Generic;
using ETG.Web.Models;

namespace ETG.Web.Tour.Models
{
    public class TourListingViewModel : IViewModel
    {
        public List<TourSummaryInfoViewModel> Tours { get; set; }
        public string ViewAllButtonLabel { get; set; }
        public string ViewAllUrl { get; set; }
        public bool HideViewAllButton { get; set; }
    }
}
