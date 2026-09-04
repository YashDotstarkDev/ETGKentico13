using System.Collections.Generic;
using Castle.Core.Internal;
using CMS;
using CMS.DataEngine;
using CMS.Search;
using CommonServiceLocator;
using ETG.Core.Promotion;
using ETG.Core.Search;
using ETG.Data.Promotion;
using ETG.Module.Booking.GlobalEvents;
[assembly: RegisterModule(typeof(CustomPrommotionEvents))]
namespace ETG.Module.Booking.GlobalEvents
{
    public class CustomPrommotionEvents : CMS.DataEngine.Module
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IETGSearchTaskCreator _etgSearchTaskCreator;
        public CustomPrommotionEvents()
            : base("CustomPrommotionEvents")
        {
            try
            {
                _promotionRepository = ServiceLocator.Current.GetInstance<IPromotionRepository>();
                _etgSearchTaskCreator = ServiceLocator.Current.GetInstance<IETGSearchTaskCreator>();
            }
            catch
            {
                
            }
        }

        // Contains initialization code that is executed when the application starts
        protected override void OnInit()
        {
            base.OnInit();

            if (_promotionRepository == null || _etgSearchTaskCreator == null)
            {
                return;
            }
            PromotionInfo.TYPEINFO.Events.Insert.After += Promotion_UpdateAfter;
            PromotionInfo.TYPEINFO.Events.Update.After += Promotion_UpdateAfter;
            //PromotionInfo.TYPEINFO.Events.Update.Before += Promotion_UpdateAfter;
            //PromotionInfo.TYPEINFO.Events.Delete.Before += Promotion_UpdateAfter;
        }
 
        private void Promotion_UpdateAfter(object sender, ObjectEventArgs e)
        {
            var promotions = new List<PromotionInfo>();
            promotions.Add((PromotionInfo)e.Object);
            var tours = _promotionRepository.GetToursWithPromotions(promotions);

            if (!tours.IsNullOrEmpty())
            {   
                foreach (var tour in tours)
                {
                    _etgSearchTaskCreator.CreateSearchTask(tour, SearchTaskTypeEnum.Update);
                }
            }

        }

    }
}