using System.Linq;
using ETG.Web.Controllers.Widgets;
using ETG.Web.Models.Widgets.HtmlTextWidget;
using ETG.Web.Models.Widgets.SingleColumn;
using Kentico.PageBuilder.Web.Mvc;
using System.Web.Mvc;
using Castle.Core.Internal;
using ETG.Web.Helpers;
using ETG.Web.Models.Widgets.SingleColumnCalloutWithFeatures;

[assembly:
    RegisterWidget("ETG.Web.Widget.SingleColumnCalloutWithFeatures", typeof(SingleColumnCalloutWithFeaturesController),
        "Single Column Callout with Features")]

namespace ETG.Web.Controllers.Widgets
{
    public class SingleColumnCalloutWithFeaturesController : WidgetController<SingleColumnCalloutWithFeaturesProperties>
    {
        public ActionResult Index()
        {
            SingleColumnCalloutWithFeaturesProperties properties = GetProperties();

            var model = new SingleColumnCalloutWithFeaturesViewModel
            {
                Heading = properties.Heading,
                CalloutHeading = properties.CalloutHeading,
                Description = properties.CalloutDescription,
                ImagePath = !properties.CalloutImages.IsNullOrEmpty()
                    ? MediaFileHelper.GetImageMediaFile(properties.CalloutImages.FirstOrDefault().FileGuid).ImagePath
                    : null,
                Features = properties.Features?.Split(';').ToList()
            };
            return PartialView("Widgets/_SingleColumnCalloutWithFeatures", model);
        }
    }
}