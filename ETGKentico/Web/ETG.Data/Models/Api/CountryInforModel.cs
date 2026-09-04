using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Models.Api
{
    public class CountryInforModel
    {
        public bool Status { get; set; } = false;
        public string Country { get; set; }
        public string Code { get; set; }
        //public string Ip { get; set; }
    }
}
