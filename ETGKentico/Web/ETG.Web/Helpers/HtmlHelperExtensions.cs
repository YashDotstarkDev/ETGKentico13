using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using ETG.Web.Configurations;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using MvcHtmlString = System.Web.Mvc.MvcHtmlString;

namespace ETG.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString GetUIString(this HtmlHelper htmlHelper, string key)
        {
            return htmlHelper.Raw(ResHelper.GetString(key, MembershipContext.AuthenticatedUser.PreferredUICultureCode));
        }

        public static MvcHtmlString AttachmentImage(this HtmlHelper htmlHelper, DocumentAttachment attachment, string title = "", string cssClassName = "", SizeConstraint? constraint = null)
        {
            if (attachment == null)
            {
                return MvcHtmlString.Empty;
            }

            return Image(htmlHelper, attachment.GetPath(), title, cssClassName, constraint);
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string path, string title = "", string cssClassName = "", SizeConstraint? constraint = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                return MvcHtmlString.Empty;
            }

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var image = new TagBuilder("img");
            image.MergeAttribute("src", urlHelper.Kentico().ImageUrl(path, constraint.GetValueOrDefault(SizeConstraint.Empty)));
            image.AddCssClass(cssClassName);
            image.MergeAttribute("alt", title);
            image.MergeAttribute("title", title);

            return MvcHtmlString.Create(image.ToString(TagRenderMode.SelfClosing));
        }

        public static IHtmlString ScriptTag(this HtmlHelper htmlHelper, string scriptPath, bool defer = true)
        {
            if (scriptPath.IsNullOrEmpty())
            {
                return null;
            }

            if (scriptPath.IndexOf("?") == -1)
            {
                scriptPath = $"{scriptPath}?v={GlobalConfiguration.FrontEndVersion}";
            }
            else
            {
                scriptPath = $"{scriptPath}&v={GlobalConfiguration.FrontEndVersion}";
            }

            if (defer)
            {
                return htmlHelper.Raw($"<script defer src=\"{scriptPath}\"></script>");
            }
            else
            {
                return htmlHelper.Raw($"<script src=\"{scriptPath}\"></script>");
            }
        }

        public static IHtmlString StyleTag(this HtmlHelper htmlHelper, string stylePath)
        {
            if (stylePath.IsNullOrEmpty())
            {
                return null;
            }

            if (stylePath.IndexOf("?") == -1)
            {
                stylePath = $"{stylePath}?v={GlobalConfiguration.FrontEndVersion}";
            }
            else
            {
                stylePath = $"{stylePath}&v={GlobalConfiguration.FrontEndVersion}";
            }

            return htmlHelper.Raw($"<link rel=\"stylesheet\" href=\"{stylePath}\">");
           
        }
    }
}