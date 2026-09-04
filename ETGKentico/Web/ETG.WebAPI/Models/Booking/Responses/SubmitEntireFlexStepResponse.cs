using ETG.Data.Extensions;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class SubmitEntireFlexStepResponse : BaseResponse
    {
        public double EntireFlexSubTotalPrice
        {
            get;
            set;
        }

        public string EntireFlexDisplaySubTotalPrice => EntireFlexSubTotalPrice.FormatPrice();
    }
}