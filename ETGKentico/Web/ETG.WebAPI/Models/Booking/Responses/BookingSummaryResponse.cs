using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Module.Booking.Booking;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class BookingSummaryResponse : BaseResponse
    {
        public CartBookingSummary BookingSummary { get; set; }

    }
}