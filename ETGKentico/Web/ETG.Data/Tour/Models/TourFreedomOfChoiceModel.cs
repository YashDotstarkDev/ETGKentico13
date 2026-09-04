using System;
using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class TourFreedomOfChoiceModel : IDataModel
    {
        public string DayCaption { get; set; }
        public string FreedomOfChoiceOptionName { get; set; }
        public string FreedomOfChoiceDescription { get; set; }
        public Guid OptionGuid { get; set; }
    }
}
