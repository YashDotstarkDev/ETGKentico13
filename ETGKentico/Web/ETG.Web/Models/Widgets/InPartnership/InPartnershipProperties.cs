using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Tour.Services;
using ETG.Web.Models.Widgets.ProductsWidget;
using ETG.Web.Tour.Models;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ETG.Web.Models.Widgets.InPartnership;
using Kentico.Components.Web.Mvc.FormComponents;

[assembly: RegisterWidget("ETG.Web.Widget.InPartnership", "In Partnership", typeof(InPartnershipProperties), "Widgets/_InPartnershipWidget")]
namespace ETG.Web.Models.Widgets.InPartnership
{
    public class InPartnershipProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 0, Label = "Heading")]
        public string Heading { get; set; }

        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order = 1, Label = "Logo 1")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Logo1Images { get; set; }
        
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order = 2, Label = "Logo 2")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Logo2Images { get; set; }
        
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Order = 3, Label = "Logo 3")]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Logo3Images { get; set; }
        
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 4, Label = "Large logos")]
        public bool LargeLogos { get; set; }
    }
}