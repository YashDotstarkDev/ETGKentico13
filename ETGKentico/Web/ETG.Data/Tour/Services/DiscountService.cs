using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Utility;
using ETG.Data.Cache;
using ETG.Data.Destination.Repositories;
using ETG.Data.Factories;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ICacheService _cacheService;
        private readonly IClock _clock;

        public DiscountService(ICacheService cacheService, IClock clock)
        {
            _cacheService = cacheService;
            _clock = clock;
        }

        private List<DiscountAndOfferModel> GetAllDiscountsInternal()
        {
            return DiscountAndOfferProvider.GetDiscountAndOffers().OnCurrentSite().Select(
                ModelFactory.CreateDiscountAndOfferModel).ToList();
        }

        private List<DiscountAndOfferModel> GetAllCacheDiscounts()
        {
            return _cacheService.GetDocumentDependentOnAll(GetAllDiscountsInternal,
                "allDiscounts", Core.PageTypes.DiscountAndOffer.CLASS_NAME);
        }

        public DiscountAndOfferModel GetDiscount(Guid guid, bool includeExpired = false)
        {
            if (!includeExpired)
            {
                return GetAllCacheDiscounts().FirstOrDefault(a => a.DiscountAndOfferGuid == guid &&
                                                                  (a.ExpiryDate == DateTime.MinValue ||
                                                                   a.ExpiryDate > _clock.Today));
            }

            return GetAllCacheDiscounts().FirstOrDefault(a => a.DiscountAndOfferGuid == guid);
        }

        public List<DiscountAndOfferModel> GetDiscounts(List<Guid> discountGuids, bool includeExpired = false)
        {
            if (!includeExpired)
            {
                return GetAllCacheDiscounts().Where(a => discountGuids.Contains(a.DiscountAndOfferGuid) &&
                                                         (a.ExpiryDate == DateTime.MinValue ||
                                                          a.ExpiryDate > _clock.Today)).ToList();
            }

            return GetAllCacheDiscounts().Where(a => discountGuids.Contains(a.DiscountAndOfferGuid)).ToList();
        }

        public List<DiscountAndOfferModel> GetExpiredDiscounts(int lastNDays)
        {
            return GetAllCacheDiscounts().Where(a=> a.ExpiryDate > DateTime.MinValue &&
                                                    a.ExpiryDate <= _clock.Today &&
                                                     a.ExpiryDate.AddDays(lastNDays) >= _clock.Today).ToList();
        }
    }
}
