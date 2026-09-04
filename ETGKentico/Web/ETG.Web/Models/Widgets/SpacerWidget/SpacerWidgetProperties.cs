using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace ETG.Web.Models.Widgets.SpacerWidget
{
    public class SpacerWidgetProperties : IWidgetProperties
    {
        [EditingComponent(IntInputComponent.IDENTIFIER, Order = 1, Label = "Padding", ExplanationText = "By default only 0, 20, 40, and 60 can be used")]
        public int Padding { get; set; }
    }
}