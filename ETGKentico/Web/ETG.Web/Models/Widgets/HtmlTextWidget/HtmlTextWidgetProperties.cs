using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace ETG.Web.Models.Widgets.HtmlTextWidget
{
    public class HtmlTextWidgetProperties : IWidgetProperties
    {
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 0, Label = "Text")]
        public string Text { get; set; }
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 1, Label = "Add side padding")]
        public bool HasSidePadding { get; set; }
    }
}