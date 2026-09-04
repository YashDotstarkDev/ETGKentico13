using ETG.Web.Controllers.Widgets;
using ETG.Web.Models.Widgets.HtmlTextWidget;
using ETG.Web.Models.Widgets.SingleColumn;
using Kentico.PageBuilder.Web.Mvc;
using System.Web.Mvc;

[assembly: RegisterWidget("ETG.Web.Widget.SingleColumn", typeof(SingleColumnController), "Single Column")]
namespace ETG.Web.Controllers.Widgets
{
    public class SingleColumnController : WidgetController<SingleColumnProperties>
    {

        public ActionResult Index()
        {
            var properties = GetProperties();

            var model = new SingleColumnViewModel
            {
                LargeContent = properties.LargeContent,
                Content = properties.Content,
                CTALabel = properties.CTALabel,
                CTAUrl = properties.CTAUrl
            };
            return PartialView("Widgets/_SingleColumn", model);
        }
    }
}