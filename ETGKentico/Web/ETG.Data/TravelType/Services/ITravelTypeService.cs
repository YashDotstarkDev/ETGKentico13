using System.Collections.Generic;
using ETG.Data.Models.PageTypes;
using ETG.Data.TravelType.Models;

namespace ETG.Data.TravelType.Services
{
    public interface ITravelTypeService
    {
        TravelTypeLandingModel GetLanding(string nodeAliasPath);
        IEnumerable<TravelTypeDetailSummaryModel> GetTravelTypeDetails(string nodeAliasPath);
        TravelTypeDetailModel GetTravelTypeDetail(string nodeAliasPath);
        IEnumerable<ThemedPackageModel> GetThemedPackages(string nodeAliasPath);
    }
}