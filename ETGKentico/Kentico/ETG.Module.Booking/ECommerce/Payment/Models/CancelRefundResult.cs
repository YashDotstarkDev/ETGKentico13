using System.Net;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class CancelRefundResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string JsonResponse { get; set; }
    }
}
