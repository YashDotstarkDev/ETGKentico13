using System.Collections.Generic;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace ETG.Web.Models.Widgets.BrochureCTAWidget
{
    public class BrochureCTAWidgetProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER,Label = "Heading")]
        public string Heading { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER,Label = "CTA Label")]
        public string CtaLabel { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER,Label = "CTA Url")]
        public string CtaUrl { get; set; }
    }
}