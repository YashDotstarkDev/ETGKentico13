using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Module.Models
{
    public class Rates
    {
        public double AUD { get; set; }
        public double EUR { get; set; }
        public double USD { get; set; }
    }

    public class FixerResult
    {
        public bool success { get; set; }
        public int timestamp { get; set; }
        public string @base { get; set; }
        public string date { get; set; }
        public Rates rates { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string type { get; set; }
    }

    public class FixerErrorResult
    {
        public bool success { get; set; }
        public Error error { get; set; }
    }
}