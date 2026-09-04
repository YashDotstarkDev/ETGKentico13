using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;

namespace CMSApp.Custom.ETGBooking.Uploader
{
    public class CSVValidatorResult
    {
        public CSVValidatorResult()
        {
            Errors = new List<string>();
        }

        public bool Success
        {
            get
            {
                return Errors.IsNullOrEmpty();
            }
        }

        public List<string> Errors { get; set; }
    }
}