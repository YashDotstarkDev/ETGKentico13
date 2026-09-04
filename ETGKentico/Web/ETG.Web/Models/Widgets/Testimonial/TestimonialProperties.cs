using ETG.Web.Models.Widgets.Testimonial;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: RegisterWidget("ETG.Web.Widget.Testimonial", "Testimonial", typeof(TestimonialProperties), "Widgets/_Testimonial")]
namespace ETG.Web.Models.Widgets.Testimonial
{
    public class TestimonialProperties : IWidgetProperties
    {
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Label = "Image (1500 x 600px)", Order =0)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        public IList<MediaFilesSelectorItem> Images { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Heading")]
        public string Heading { get; set; }
        //[EditingComponent(CKEditorComponent.IDENTIFIER, Order = 0, Label = "Text Content")]
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 2, Label = "Text Content")]
        public string TextContent { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 3, Label = "Date")]
        public string Date { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "Source")]
        public string Source { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 5, Label = "Source Location")]
        public string SourceLocation { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 6, Label = "CTA Label")]
        public string CTALabel { get; set; }


        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 7, Label = "CTA Path")]
        public string Path { get; set; }



    }
}