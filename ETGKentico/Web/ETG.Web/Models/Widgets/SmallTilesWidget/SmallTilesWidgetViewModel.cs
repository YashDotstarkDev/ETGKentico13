using System.Collections.Generic;

namespace ETG.Web.Models.Widgets.SmallTilesWidget
{
    public class SmallTilesWidgetViewModel
    {
        public string Heading { get; set; }
        public List<ImageTileCtaViewModel> Ctas { get; set; }
        public string SectionId { get; set; }
    }
}