using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ETG.WebAPI.Models.Booking.Responses
{
    public class GetFreedomOfChoicesResponse : BaseResponse
    {
        public List<DayFreedomeOfChoice> FreedomOfChoices { get; set; }
    }
}