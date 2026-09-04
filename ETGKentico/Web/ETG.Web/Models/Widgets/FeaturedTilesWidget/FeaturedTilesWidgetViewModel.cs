using System.Collections.Generic;

namespace ETG.Web.Models.Widgets.FeaturedTilesWidget
{
    public class FeaturedTilesWidgetViewModel
    {
        public string Heading { get; set; }
        public List<ImageTileCtaViewModel> Ctas { get; set; }

        public int TileType { get; set; }
        public string SectionId { get; set; }
    }
}