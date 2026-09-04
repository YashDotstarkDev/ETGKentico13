using ETG.Web.Controllers.Widgets;
using ETG.Web.Models.Widgets.HtmlTextWidget;
using Kentico.PageBuilder.Web.Mvc;
using System.Web.Mvc;

[assembly: RegisterWidget("ETG.Web.Widget.HtmlTextWidget", typeof(HtmlTextWidgetController), "Html Text")]
namespace ETG.Web.Controllers.Widgets
{
    public class HtmlTextWidgetController : WidgetController<HtmlTextWidgetProperties>
    {
        public ActionResult Index()
        {
            HtmlTextWidgetProperties properties = GetProperties();

            var model = new HtmlTextWidgetViewModel
            {
                Text = properties.Text,
                HasSidePadding = properties.HasSidePadding
            };
            return PartialView("Widgets/_HtmlTextWidget", model);
        }
    }
}