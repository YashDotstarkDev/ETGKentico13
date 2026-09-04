using ETG.Module.Booking.Models;
using System.Collections.Generic;
using ETG.Data.Extensions;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class SubmitNumberOfTravellerStepResponse : BaseResponse
    {
        public SubmitNumberOfTravellerStepResponse()
        {
            RoomOptions = new List<RoomOptionDropdown>();
        }
        public string CurrencySymbol { get; set; }
        public double TotalPrice { get; set; }
        
        public string TotalDisplayPrice => TotalPrice.FormatPrice(false, CurrencySymbol);

        public int NumberOfTravellers { get; set; }
        public int TotalRooms { get; set; }

        public List<RoomOptionDropdown> RoomOptions { get; set; }
        
        public bool HasExtras { get; set; }
        public List<RoomOption> ExtraOptions { get; set; }
    }
}