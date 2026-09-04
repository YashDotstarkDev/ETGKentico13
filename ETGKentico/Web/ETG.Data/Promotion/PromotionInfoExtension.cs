using ETG.Core.Promotion;
using ETG.Data.Models.Booking;

namespace ETG.Data.Promotion
{
    public static class PromotionInfoExtension
    {
        public static PromotionItem MapToPromotionItem(this PromotionInfo promotion)
        {
            if (promotion == null || string.IsNullOrWhiteSpace(promotion.PromotionName))
            {
                return null;
            }
            return new PromotionItem
            {
                PromotionID = promotion.PromotionID,
                PromotionName = promotion.PromotionName,
                PromotionDollarDiscount = promotion.PromotionDollarDiscount,
                PromotionPercentDiscount = promotion.PromotionPercentDiscount,
                ValidFrom = promotion.PromotionFromDate,
                ValidTo = promotion.PromotionToDate
            };
        }
    }
}
