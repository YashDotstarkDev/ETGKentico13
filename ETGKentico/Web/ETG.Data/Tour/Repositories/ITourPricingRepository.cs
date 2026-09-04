using System;
using ETG.Data.Tour.Models;
using System.Collections.Generic;

namespace ETG.Data.Tour.Repositories
{
    public interface ITourPricingRepository
    {
        List<PricingModel> GetTourPricings(string aliasPath);
        List<PricingModel> GetTourPricings(List<string> aliasPaths);
        List<PricingModel> GetTourPricings(List<Guid> tourGuids);
    }
}
