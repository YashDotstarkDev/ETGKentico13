using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace ETG.Web.Models.Widgets.Table
{
    public class TableProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Title")]
        public string Title { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 2, Label = "Content")]
        public string Content { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 3, Label = "HTML table content")]
        public string HtmlTable { get; set; }
    }
}