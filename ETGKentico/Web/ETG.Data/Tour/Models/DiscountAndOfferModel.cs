using Devotion.Automapper.Common;
using System;
using Castle.Core.Internal;
using CMS.Helpers;

namespace ETG.Data.Tour.Models
{
    public class DiscountAndOfferModel : IDataModel
    {
        public Guid DiscountAndOfferGuid { get; set; }
        public string OfferText { get; set; }
        public string OfferHeading { get; set; }
        public DateTime ExpiryDate { get; set; }
        
        public bool DisplayAsLabel { get; set; }
        public string DisplayLabel { get; set; }
        public bool IsOnSaleNow { get; set; }
        public bool FullPaymentRequired { get; set; }
        //public bool DoNotShowAsDeals { get; set; }
        public override string ToString()
        {
            var text = new System.Text.StringBuilder();

            if (!OfferHeading.IsNullOrEmpty())
            {
                text.AppendLine(OfferHeading);

            }

            if (!OfferText.IsNullOrEmpty())
            {
                text.AppendLine(HTMLHelper.StripTags(OfferText));
            }

            if (ExpiryDate != DateTime.MinValue)
            {
                text.AppendLine($"Expires: {ExpiryDate:dd/MM/yyyy}");
            }

            return text.ToString();

        }
    }
}