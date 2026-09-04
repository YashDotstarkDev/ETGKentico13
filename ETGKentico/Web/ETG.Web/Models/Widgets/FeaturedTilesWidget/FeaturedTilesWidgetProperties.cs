using System.Collections.Generic;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace ETG.Web.Models.Widgets.FeaturedTilesWidget
{
    public class FeaturedTilesWidgetProperties : IWidgetProperties
    {

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1)]
        public string Heading { get; set; }

        // Assigns a selector component to the 'PagePaths' property
        [EditingComponent(PathSelector.IDENTIFIER, Order = 2, ExplanationText = "Select Image Tile CTA folder")]
        // Returns a list of path selector items (page paths)
        public IList<PathSelectorItem> Path { get; set; }


        [EditingComponent(DropDownComponent.IDENTIFIER, Order = 3)]

        [EditingComponentProperty(nameof(DropDownProperties.DataSource), "0;Tiles are links\r\n1;Tiles are not links")]
        public string TileType { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "Section ID")]
        public string SectionId { get; set; }


    }
}