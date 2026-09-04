using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Search.Models
{
    public abstract class BaseSearchObject
    {
        public string DocumentName { get; set; }
        public string classname { get; set; }
        public string Url { get; set; }
    }
}
