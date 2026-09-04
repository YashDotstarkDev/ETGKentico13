using System.Collections.Generic;
using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class TourListingModel : IDataModel
    {
        public List<TourSummaryInfoModel> Tours { get; set; }
        public string ViewAllButtonLabel { get; set; }
        public string ViewAllUrl { get; set; }
    }
}
