using ETG.Data.Extensions;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class SubmitExtraOptionsStepResponse : BaseResponse
    {
        public string CurrencySymbol { get; set; }
        public double ExtrasSubTotalPrice { get; set; }

        public string ExtrasSubTotalDisplayPrice => ExtrasSubTotalPrice.FormatPrice(false, CurrencySymbol);
    }
}