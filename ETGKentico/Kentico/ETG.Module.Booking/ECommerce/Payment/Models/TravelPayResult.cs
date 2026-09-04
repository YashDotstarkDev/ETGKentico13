using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class TravelPayResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string JsonResponse { get; set; }
    }
}
