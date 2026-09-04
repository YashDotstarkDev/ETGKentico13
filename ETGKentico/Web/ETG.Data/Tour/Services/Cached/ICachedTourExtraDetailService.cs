using System.Collections.Generic;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Services.Cached
{
    public interface ICachedTourExtraDetailService
    {
        List<TourItineraryModel> GetTourItinerary(string path);
        List<NameValuePathModel> GetTourHighlights(string path);

        List<KeyValuePair<string, string>> GetTourInclusions(string path);

        List<HotelModel> GetHotels(string guids);
    }
}
