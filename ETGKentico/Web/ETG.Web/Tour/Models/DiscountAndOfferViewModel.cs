using ETG.Web.Models;
using System;
using ETG.Data.Helpers;

namespace ETG.Web.Tour.Models
{
    public class DiscountAndOfferViewModel : IViewModel
    {
        public string OfferText { get; set; }
        public string OfferHeading { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExpiryDateText =>  DateTimeHelper.GetGMTTime(ExpiryDate.AddSeconds(86399));
        public bool DisplayAsLabel { get; set; }
        public string DisplayLabel { get; set; }
        public bool IsOnSaleNow { get; set; }
        public bool FullPaymentRequired { get; set; }
        //public bool DoNotShowAsDeals { get; set; }
    }
}