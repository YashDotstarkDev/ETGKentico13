using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.Forms
{
    public class BookingAgentFormViewModel
    {
        public string AgentAgencyName { get; set; }

        public string AgentConsultantName { get; set; }

        public string AgentPostcode { get; set; }
        public string AgentPhone { get; set; }

        public string AgentEmail { get; set; }
        public string Comment { get; set; }
        public string AgentFormOption { get; set; }
    }
}