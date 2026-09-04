using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Services
{
    public interface ITourListingService
    {
        List<TourSummaryInfoModel> GetCombinedTours(List<string> tourCodes, bool includeHiddenUpgradeTours = false);

        List<TourSummaryInfoModel> GetCombinedToursByDestination(Guid destinationGuid, int topN);

        List<TourSummaryInfoModel> GetCombinedToursByExperience(Guid experienceGuid, int topN);

        List<TourSummaryInfoModel> GetCombinedTours(int topN);
        List<TourSummaryInfoModel> GetCombinedToursByDestinations(List<Guid> destinationGuids, int topN);
    }
}
