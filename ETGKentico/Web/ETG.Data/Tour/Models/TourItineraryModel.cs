using System.Collections.Generic;
using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class TourItineraryModel : IDataModel
    {
        public int NodeID { get; set; }
        public int NodeOrder { get; set; }
        public string Title { get; set; }
        public string DayCaption { get; set; }
        public string Details { get; set; }
        public string Image { get; set; }
        public string Inclusion { get; set; }
        public string ImageCaption { get; set; }

        public string LocationUrl { get; set; }
        public string LocationName { get; set; }
        public string LocationSummary { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public List<TourFreedomOfChoiceModel> FreedomOfChoices { get; set; }
    }
}
