using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSApp.Custom.ETGBooking.Uploader
{
    public class UploaderBookingAdditions
    {
        public string TourCode { get; set; }
        public int NodeID { get; set; }
        public string NodeAliasPath { get; set; }
        public string CMSLabel { get; set; }
        public string CSVLabel { get; set; }
    }
}