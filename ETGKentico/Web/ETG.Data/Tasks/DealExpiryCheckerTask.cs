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
using ETG.Data.Tour.Repositories;
using ETG.Data.Tour.Services;

namespace ETG.Data.Tasks
{
    public class DealExpiryCheckerTask : ITask
    {
        private readonly IDiscountService _discountService;
        private readonly ITourProductRepository _tourProductRepository;
        private readonly IETGSearchTaskCreator _etgSearchTaskCreator;
        public DealExpiryCheckerTask()
        {
            _discountService = ServiceLocator.Current.GetInstance<IDiscountService>();
            _tourProductRepository = ServiceLocator.Current.GetInstance<ITourProductRepository>();
            _etgSearchTaskCreator = ServiceLocator.Current.GetInstance<IETGSearchTaskCreator>();
        }

        public string Execute(TaskInfo task)
        {
            var numberOFDays = task.TaskData.ToInteger(1);
            var discounts = _discountService.GetExpiredDiscounts(numberOFDays);

            if (discounts.IsNullOrEmpty())
            {
                return "No recently expired";
            }

            var tours = _tourProductRepository.GetToursWithDiscounts(discounts.Select(a => a.DiscountAndOfferGuid).ToList());

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
