using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Models.Api.Postmark
{
    public class BounceApiResponse
    {
        public int TotalCount { get; set; }
        public List<Bounce> Bounces { get; set; }
    }

    public class Bounce
    {
        public string RecordType { get; set; }
        public object ID { get; set; }
        public string Type { get; set; }
        public int TypeCode { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string MessageID { get; set; }
        public int ServerID { get; set; }
        public string MessageStream { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
        public string From { get; set; }
        public DateTime BouncedAt { get; set; }
        public bool DumpAvailable { get; set; }
        public bool Inactive { get; set; }
        public bool CanActivate { get; set; }
        public string Subject { get; set; }
    }
}
