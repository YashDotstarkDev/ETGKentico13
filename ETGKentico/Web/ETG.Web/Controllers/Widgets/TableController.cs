using ETG.Web.Controllers.Widgets;
using Kentico.PageBuilder.Web.Mvc;
using System.Web.Mvc;
using ETG.Web.Models.Widgets.Table;

[assembly: RegisterWidget("ETG.Web.Widget.Table", typeof(TableController), "Table content")]
namespace ETG.Web.Controllers.Widgets
{
    public class TableController : WidgetController<TableProperties>
    {

        public ActionResult Index()
        {
            TableProperties properties = GetProperties();

            var model = new TableViewModel()
            {
                Title = properties.Title,
                Content = properties.Content,
                HtmlTableContent = properties.HtmlTable
            };
            
            return PartialView("Widgets/_Table", model);
        }
    }
}