using System;
using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class TourOptionalExtrasModel : IDataModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string ImageCaption { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string PriceStatements { get; set; }
        public string CostPerPerson { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public string AvailableDaysOfWeek { get; set; }
    }
}
