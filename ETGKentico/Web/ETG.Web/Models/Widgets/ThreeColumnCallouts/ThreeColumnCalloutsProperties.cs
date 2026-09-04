using ETG.Web.Models.Widgets.ThreeColumnCallouts;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;

[assembly: RegisterWidget("ETG.Web.Widget.ThreeColumnCallouts", "3 col callouts", typeof(ThreeColumnCalloutsProperties), "Widgets/_ThreeColumnCallouts")]
namespace ETG.Web.Models.Widgets.ThreeColumnCallouts
{
    public class ThreeColumnCalloutsProperties : IWidgetProperties
    {
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 0, Label = "Right align?")]
        public bool StartFromRight { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Section Id Name (Used in QuickLinks)")]
        public string SectionIdName { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 2, Label = "Heading")]
        public string Heading { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 3, Label = "Callout 1 Heading")]
        public string Callout1Heading { get; set; }

        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order = 4, Label = "Callout 1  Image")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Callout1Images { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 5, Label = "Callout 1 Description")]
        public string Callout1Description { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 6, Label = "Callout 1 Url")]
        public string Callout1Url { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 7, Label = "Callout 2 Heading")]
        public string Callout2Heading { get; set; }

        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order = 8, Label = "Callout 2 Image")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Callout2Images { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 9, Label = "Callout 2 Description")]
        public string Callout2Description { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 10, Label = "Callout 2 Url")]
        public string Callout2Url { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 11, Label = "Callout 3 Heading")]
        public string Callout3Heading { get; set; }

        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order = 12, Label = "Callout 3 Image")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Callout3Images { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 13, Label = "Callout 3 Description")]
        public string Callout3Description { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 14, Label = "Callout 3 Url")]
        public string Callout3Url { get; set; }

    }
}