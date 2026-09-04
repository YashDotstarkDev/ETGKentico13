using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using ETG.Web.Models.Widgets.SingleColumnCalloutWithFeatures;

[assembly:
    RegisterWidget("ETG.Web.Widget.SingleColumnCalloutWithFeatures", "Single Callout with Features",
        typeof(SingleColumnCalloutWithFeaturesProperties))]

namespace ETG.Web.Models.Widgets.SingleColumnCalloutWithFeatures
{
    public class SingleColumnCalloutWithFeaturesProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 2, Label = "Heading")]
        public string Heading { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 3, Label = "Callout Heading")]
        public string CalloutHeading { get; set; }

        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order = 4, Label = "Callout Image")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> CalloutImages { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 5, Label = "Callout Description")]
        public string CalloutDescription { get; set; }

        // [EditingComponent(TextInputComponent.IDENTIFIER, Order = 6, Label = "Callout Url")]
        // public string CalloutUrl { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 6, Label = "Features(separate by ';')")]
        public string Features { get; set; }
    }
}