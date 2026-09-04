using ETG.Web.Models.Widgets.CTAPanel;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;

[assembly: RegisterWidget("ETG.Web.Widget.CTAPanel", "CTA Panel", typeof(CTAPanelProperties), "Widgets/_CTAPanel")]
namespace ETG.Web.Models.Widgets.CTAPanel
{
    public class CTAPanelProperties : IWidgetProperties
    {
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order =1, Label = "Background Image")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Images { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 2, Label = "Heading")]
        public string Heading { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 3, Label = "Text Content")]
        public string TextContent { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "CTA Label")]
        public string CTALabel { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 5)]
        public string Path { get; set; }

    }
}