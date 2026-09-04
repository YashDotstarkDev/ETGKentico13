using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class AccordionItemModel : IDataModel
    {
        public string Heading { get; set; }
        public string Contents { get; set; }
    }
}
