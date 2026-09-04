using System;
using System.Collections.Generic;
using ETG.Core.Promotion;
using ETG.Data.Tour.Models; 

namespace ETG.Data.Promotion
{
    public interface IPromotionRepository
    {
        PromotionInfo GetPromotionInfoForNonAgent(TourModel tour, DateTime date);
        PromotionInfo GetPromotionInfoForAgent(TourModel tour, string email, DateTime date);
        List<PromotionInfo> GetExpiredPromotions(int lastNDays);
        
        List<Core.PageTypes.Tour> GetToursWithPromotions(List<PromotionInfo> promotions);
        List<PromotionInfo> GetAllPromotions(DateTime date);
        
        PromotionInfo GetPromotionInfoByPromoCode(string promoCode, TourModel tour, DateTime bookingDate, DateTime depatureDate);

        PromotionInfo GetPromotionInfoForNonAgentForEDM(TourModel tour, DateTime date);
    }
}