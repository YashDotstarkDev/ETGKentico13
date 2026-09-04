using System;
using ETG.Data.Helpers;

namespace ETG.Data.Models.Booking
{
    public class PromotionItem
    {
        public int PromotionID { get; set; }
        public string PromotionName { get; set; }
        public double PromotionPercentDiscount { get; set; }
        public int PromotionDollarDiscount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public string ValidFromTo
        {
            get
            {
                return $"{ValidFrom:dd MMM yyyy} - {ValidTo:dd MMM yyyy}";
            }
        }
        
        public string BookByText
        {
            get
            {
                return $"Book By {ValidTo:dd MMM yyyy}";
            }
        }

        public string ExpiryDateText => DateTimeHelper.GetGMTTime(ValidTo.AddSeconds(86399));
        public double GetDiscountedPrice(double grossPrice, int totalPerson = 1)
        {
            if (PromotionDollarDiscount > 0)
            {
                return grossPrice - PromotionDollarDiscount * totalPerson;
            }

            if (PromotionPercentDiscount > 0)
            {
                return Math.Round(grossPrice * (1 - PromotionPercentDiscount * .01));
            }

            return grossPrice;
        }
        
        public double GetDiscount(double grossPrice, int totalPerson)
        {
            if (PromotionDollarDiscount > 0)
            {
                return PromotionDollarDiscount * totalPerson;
            }

            if (PromotionPercentDiscount > 0)
            {
                return Math.Round(grossPrice * ( PromotionPercentDiscount * .01));
            }

            return 0;
        }

        public bool IsValid(DateTime date)
        {
            if (date >= ValidFrom && date <= ValidTo)
            {
                return true;
            }

            return false;
        }
    }
}