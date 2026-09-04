using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.Base;
using CMS.Scheduler;
using CMS.Search;
using CommonServiceLocator;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Search;
using ETG.Core.Utility;
using ETG.Data.Promotion;
using ETG.Data.Tour.Repositories;
using ETG.Data.Tour.Services;
using ETG.Module.Booking.ECommerce;

namespace ETG.Module.Booking.Tasks
{
    public class PromotionExpiryCheckerTask : ITask
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IETGSearchTaskCreator _etgSearchTaskCreator;
        public PromotionExpiryCheckerTask()
        {
            _promotionRepository = ServiceLocator.Current.GetInstance<IPromotionRepository>();
            _etgSearchTaskCreator = ServiceLocator.Current.GetInstance<IETGSearchTaskCreator>();
        }

        public string Execute(TaskInfo task)
        {
            var numberOFDays = task.TaskData.ToInteger(1);
            var promotions = _promotionRepository.GetExpiredPromotions(numberOFDays);

            if (promotions.IsNullOrEmpty())
            {
                return "No recently expired";
            }

            var tours = _promotionRepository.GetToursWithPromotions(promotions);

            if (tours.IsNullOrEmpty())
            {   
                foreach (var tour in tours)
                {
                    _etgSearchTaskCreator.CreateSearchTask(tour, SearchTaskTypeEnum.Update);
                }
            }

            return $"Updated {tours.Count} tours {string.Join(",",tours.Select(a=>a.TourCode))}";

        }
    }
}
