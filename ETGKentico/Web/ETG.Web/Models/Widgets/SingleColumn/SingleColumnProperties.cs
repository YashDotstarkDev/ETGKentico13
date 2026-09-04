using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace ETG.Web.Models.Widgets.SingleColumn
{
    public class SingleColumnProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Large Content")]
        public string LargeContent { get; set; }
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 2, Label = "Content")]
        public string Content { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 3, Label = "CTA Label")]
        public string CTALabel { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "CTA Url")]
        public string CTAUrl { get; set; }
    }
}