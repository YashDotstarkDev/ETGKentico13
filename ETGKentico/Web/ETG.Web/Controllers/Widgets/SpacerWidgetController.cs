using ETG.Web.Controllers.Widgets;
using Kentico.PageBuilder.Web.Mvc;
using System.Web.Mvc;
using ETG.Web.Models.Widgets.SpacerWidget;

[assembly: RegisterWidget("ETG.Web.Widget.SpacerWidget", typeof(SpacerWidgetController), "Spacer")]
namespace ETG.Web.Controllers.Widgets
{
    public class SpacerWidgetController : WidgetController<SpacerWidgetProperties>
    {
        public ActionResult Index()
        {
            var properties = GetProperties();

            var model = new SpacerWidgetViewModel()
            {
                Padding = properties.Padding
            };
            
            return PartialView("Widgets/_SpacerWidget", model);
        }
    }
}