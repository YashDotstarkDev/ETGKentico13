using System;
using System.Collections.Generic;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Repositories.Cached
{
    public interface ICachedTourPricingRepository
    {
        List<PricingModel> GetTourPricings(string aliasPath);

        List<PricingModel> GetTourPricings(List<string> aliasPaths);

        List<PricingModel> GetTourPricings(List<Guid> tourGuids);
    }
}