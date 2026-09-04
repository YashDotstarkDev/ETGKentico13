using ETG.Web.Models.FormComponents;
using ETG.Web.Models.Widgets.FeaturedContentPanel;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: RegisterWidget("ETG.Web.Widget.FeaturedContentPanel", "Feature", typeof(FeaturedContentPanelProperties), "Widgets/_FeaturedContentPanel")]
namespace ETG.Web.Models.Widgets.FeaturedContentPanel
{
    public class FeaturedContentPanelProperties : IWidgetProperties
    {
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order =0, Label = "Image (1500 x 600px)")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Images { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Heading")]
        public string Heading { get; set; }
        //[EditingComponent(CKEditorComponent.IDENTIFIER, Order = 0, Label = "Text Content")]
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 2, Label = "Text Content")]
        public string TextContent { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 3, Label = "CTA Label")]
        public string CTALabel { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "ImageCaption")]
        public string ImageCaption { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order =5)]
        public string Path { get; set; }



    }
}