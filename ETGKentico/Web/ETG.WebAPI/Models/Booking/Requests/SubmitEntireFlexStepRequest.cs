using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Requests
{
    public class SubmitEntireFlexStepRequest
    {
        public bool AvailEntireFlexOption { get; set; }
    }
}