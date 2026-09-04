using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Tour.Models;

namespace ETG.Data.TravelDeals.Services
{
    public interface ITravelDealsService
    {
        List<TourSummaryInfoModel> GetToursWithTravelDeals();
    }
}
