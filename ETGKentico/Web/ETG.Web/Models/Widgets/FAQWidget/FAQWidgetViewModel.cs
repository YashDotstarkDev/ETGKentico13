using System.Collections.Generic;

namespace ETG.Web.Models.Widgets.FAQWidget
{
    public class FAQWidgetViewModel
    {
        public bool ShowViewAllButton { get; set; }
        public string FAQsPageUrl { get; set; }
        public string Heading { get; set; }
        public List<FAQViewModel> FAQs { get; set; }
    }
}