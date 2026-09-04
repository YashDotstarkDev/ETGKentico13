using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Tour.Factories;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Repositories;

namespace ETG.Data.TravelDeals.Services
{
    public class TravelDealsService : ITravelDealsService
    {
        private readonly ITourModelFactory _tourModelFactory;

        public TravelDealsService(ITourModelFactory tourModelFactory)
        {
            _tourModelFactory = tourModelFactory;
        }

        public List<TourSummaryInfoModel> GetToursWithTravelDeals()
        {
            var allTourAndCruise = new List<TourSummaryInfoModel>();
            var tours = TourProvider.GetTours().WhereNotNull(nameof(Core.PageTypes.Tour.TourDiscountAndOffer)).OnCurrentSite()
                .Select(_tourModelFactory.CreateSummaryInfoModel).ToList();

            allTourAndCruise.AddRange(tours);
            return allTourAndCruise;
        }
    }
}
