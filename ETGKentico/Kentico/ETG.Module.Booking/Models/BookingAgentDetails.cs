using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;

namespace ETG.Module.Booking.Models
{
    public class BookingAgentDetails
    {
        public string AgentAgencyName { get; set; }

        public string AgentConsultantName { get; set; }

        public string AgentPostcode { get; set; }
        public string AgentPhone { get; set; }

        public string AgentEmail { get; set; }
        public string Comment { get; set; }

        
        public bool IsValid()
        {
            return !AgentAgencyName.IsNullOrEmpty() && !AgentConsultantName.IsNullOrEmpty();
        }
    }
}