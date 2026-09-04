using ETG.Web.Models.Widgets.QuoteContent;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("ETG.Web.Widget.QuoteContent", "Quote", typeof(QuoteContentProperties), "Widgets/_QuoteContent")]
namespace ETG.Web.Models.Widgets.QuoteContent
{
    public class QuoteContentProperties : IWidgetProperties
    {
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 0, Label = "Quote")]
        public string TextContent { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Source")]
        public string Source { get; set; }
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 2, Label = "Add side padding")]
        public bool HasSidePadding { get; set; }

    }
}