using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;

namespace ETG.Web.Models.Widgets.ImageGallery
{
    public class ImageGalleryProperties : IWidgetProperties
    {
        // Assigns a selector component to the 'PagePaths' property
        [EditingComponent(PathSelector.IDENTIFIER)]
        // Returns a list of path selector items (page paths)
        public IList<PathSelectorItem> ImagePaths { get; set; }

    }
}