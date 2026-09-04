using ETG.Web.Models.Widgets.FullWidthImage;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: RegisterWidget("ETG.Web.Widget.FullWidthImage", "Full width Image", typeof(FullWidthImageProperties), "Widgets/_FullWidthImage")]
namespace ETG.Web.Models.Widgets.FullWidthImage
{
    public class FullWidthImageProperties : IWidgetProperties
    {
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Label = "Image", Order =0)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Image { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Caption")]
        public string Caption { get; set; }

    }
}