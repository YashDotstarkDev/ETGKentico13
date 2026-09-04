using System;
using System.Collections.Generic;

namespace ETG.Web.Tour.BookNow.Models
{
    public class DayFreedomOfChoiceViewModel
    {
        public string DayLabel { get; set; }
        public string DayName { get; set; }
        
        public List<KeyValuePair<Guid, string>> Options { get; set; }
    }
}