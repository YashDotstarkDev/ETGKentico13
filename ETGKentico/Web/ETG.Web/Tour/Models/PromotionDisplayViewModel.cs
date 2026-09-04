using ETG.Web.Models;
using System;
using ETG.Data.Helpers;

namespace ETG.Web.Tour.Models
{
    public class PromotionDisplayViewModel : IViewModel
    {
        public string PromotionName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExpiryDateText => DateTimeHelper.GetGMTTime(ExpiryDate.AddSeconds(86399));

    }
}