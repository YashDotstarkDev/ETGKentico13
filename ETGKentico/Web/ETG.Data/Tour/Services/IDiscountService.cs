using ETG.Data.Tour.Models;
using System;
using System.Collections.Generic;

namespace ETG.Data.Tour.Services
{
    public interface IDiscountService
    {
        DiscountAndOfferModel GetDiscount(Guid guid, bool includeExpired = false);
        List<DiscountAndOfferModel> GetDiscounts(List<Guid> discountGuids, bool includeExpired = false);
        List<DiscountAndOfferModel> GetExpiredDiscounts(int lastNDays);
    }
}
