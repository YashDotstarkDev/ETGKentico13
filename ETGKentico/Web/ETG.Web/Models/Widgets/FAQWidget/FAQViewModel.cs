using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.Widgets.FAQWidget
{
    public class FAQViewModel : IViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}