using ETG.Web.Models;
using System;

namespace ETG.Web.Tour.Models
{
    public class TourFreedomOfChoiceViewModel : IViewModel
    {
        public string DayCaption { get; set; }
        public string FreedomOfChoiceOptionName { get; set; }
        public string FreedomOfChoiceDescription { get; set; }
        public Guid OptionGuid { get; set; }
    }
}
