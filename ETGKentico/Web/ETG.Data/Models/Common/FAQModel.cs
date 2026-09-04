using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class FAQModel : IDataModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
