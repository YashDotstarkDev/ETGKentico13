using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Services
{
    public interface ITourExtraDetailService
    {
        List<TourItineraryModel> GetTourItinerary(string path);
        List<NameValuePathModel> GetTourHighlights(string path);
        List<KeyValuePair<string, string>> GetTourInclusions(string path, bool onlyHighlighted = false);
        List<HotelModel> GetHotels(string guids);
        List<HotelModel> GetHotelsByHotelIds(List<int> hotelIds);
        List<HotelModel> GetHotelsByParentAliasPath(string parentAliasPath);
        List<TourFreedomOfChoiceModel> GetTourFreedomOfChoiceOptions(string tourAliasPath);
        List<TourRoomUpgradeModel> GetTourRoomUpgrades(string path);
        List<TourOptionalExtrasModel> GetTourOptionalExtras(string path);
        List<TourPackageUpgradeModel> GetTourUpgrades(string path);
        List<TourSummaryInfoModel> GetRelatedTourTiles(string path);
        List<NameValuePathModel> GetTourBonuses(string tourPageAliasPath);
    }
}
