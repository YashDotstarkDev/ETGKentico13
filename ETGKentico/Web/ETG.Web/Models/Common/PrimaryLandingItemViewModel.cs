using Devotion.Automapper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Web.Models.Common
{
    public class PrimaryLandingItemViewModel : IViewModel
    {
        public string Heading { get; set; }
        public string Image { get; set; }
        public string Summary { get; set; }
        public string Path { get; set; }
    }
}
