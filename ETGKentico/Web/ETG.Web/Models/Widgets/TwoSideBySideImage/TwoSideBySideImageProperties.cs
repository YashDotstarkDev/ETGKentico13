using ETG.Web.Models.Widgets.TwoSideBySideImage;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: RegisterWidget("ETG.Web.Widget.TwoSideBySideImage", "Two Side by Side Image", typeof(TwoSideBySideImageProperties), "Widgets/_TwoSideBySideImage")]
namespace ETG.Web.Models.Widgets.TwoSideBySideImage
{
    public class TwoSideBySideImageProperties : IWidgetProperties
    {
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Label = "Image 1", Order = 0)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Image1 { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Caption 1")]
        public string Caption1 { get; set; }

        [EditingComponent(MediaFilesSelector.IDENTIFIER, Label = "Image 2", Order = 2)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Image2 { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 3, Label = "Caption 2")]
        public string Caption2 { get; set; }

        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 4, Label = "Add side padding")]
        public bool HasSidePadding { get; set; }
    }
}