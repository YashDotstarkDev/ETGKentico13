using System;
using Devotion.Automapper.Common;
using ETG.Data.Helpers;

namespace ETG.Data.Tour.Models
{
    public class PromotionDisplayModel : IDataModel
    {
        public string PromotionName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExpiryDateText =>  DateTimeHelper.GetGMTTime(ExpiryDate.AddSeconds(86399));
        
    }
}