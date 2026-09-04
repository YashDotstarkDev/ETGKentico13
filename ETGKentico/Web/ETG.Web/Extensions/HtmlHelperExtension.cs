using System.Web.Mvc;

namespace ETG.Web.Extensions
{
    public static class HtmlHelperExtension
    {
        public static string HiddenClass(this HtmlHelper htmlHelper, bool isHidden)
        {
            return isHidden ? "hidden" : "";
        }

        public static MvcHtmlString DisplayBlockStyle(this HtmlHelper htmlHelper, bool displayBlock)
        {
            return MvcHtmlString.Create(displayBlock ? "style=\"display:block\"" : "");
        }
        public static MvcHtmlString DisplayNoneStyle(this HtmlHelper htmlHelper, bool isHidden)
        {
            return MvcHtmlString.Create(isHidden ? "style=\"display:none\"" : "");
        }

        public static MvcHtmlString DisableAttrubute(this HtmlHelper htmlHelper, bool isDisabled)
        {
            return MvcHtmlString.Create(isDisabled ? "disabled" : "");
        }
    }
}