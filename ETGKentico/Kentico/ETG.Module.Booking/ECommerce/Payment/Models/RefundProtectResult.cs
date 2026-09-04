using System.Net;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class RefundProtectResult
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string JsonResponse { get; set; }
    }
}
