using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GlobalLink.Connect.TargetServiceRef;

namespace CMSApp.Custom.Models
{
    public class EnquiryLegacyModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PreferredContactMethod { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string SubscribeToNewsletter { get; set; }
        public string Comments { get; set; }
        public string TourCode { get; set; }
        public string TourName { get; set; }
        public string TourLink { get; set; }
        public string PreferredDestination { get; set; }
        public string FormInserted { get; set; }
        public string Status { get; set; }
        
    }
}