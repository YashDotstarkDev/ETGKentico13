using ETG.Web.Models.Widgets.YoutubeVideo;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;

[assembly: RegisterWidget("ETG.Web.Widget.YoutubeVideo", "Youtube Video", typeof(YoutubeVideoProperties), "Widgets/_YoutubeVideo")]
namespace ETG.Web.Models.Widgets.YoutubeVideo
{
    public class YoutubeVideoProperties : IWidgetProperties
    {
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Label = "Image")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Image { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 0, Label = "Youtube ID")]
        public string YoutubeID { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 2, Label = "Caption")]
        public string Caption { get; set; }

        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 3, Label = "Open In New Window")]
        public bool OpenInNewWindow { get; set; }
    }
}