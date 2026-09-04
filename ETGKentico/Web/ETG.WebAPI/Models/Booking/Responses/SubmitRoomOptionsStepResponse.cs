using ETG.Data.Extensions;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class SubmitRoomOptionsStepResponse : BaseResponse
    {
        public int Rooms { get; set; }
        public double RoomOptionSubTotalPrice { get; set; }
        
        public string CurrencySymbol { get; set; }
        public string RoomOptionSubTotalDisplayPrice => RoomOptionSubTotalPrice.FormatPrice(false, CurrencySymbol);

    }
}