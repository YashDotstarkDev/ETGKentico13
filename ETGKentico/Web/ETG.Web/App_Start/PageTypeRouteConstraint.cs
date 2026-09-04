using System.Linq;
using System.Web;
using System.Web.Routing;
using CMS.DocumentEngine;
using Devotion.Web.Base.Extensions;

namespace ETG.Web
{
    public class PageTypeRouteConstraint : IRouteConstraint
    {
        private readonly string _type;

        public PageTypeRouteConstraint(string type)
        {
            _type = type;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if ((routeDirection == RouteDirection.IncomingRequest) &&
                parameterName == "path")
            {
                var className = DocumentHelper.GetDocuments().AllCultures()
                    .WhereEquals(nameof(TreeNode.NodeAliasPath), values["path"].ToString().BeginWithSlash())
                    .Columns(nameof(TreeNode.ClassName)).FirstOrDefault()?.ClassName ?? string.Empty;

                return className == _type;
            }

            return false;
        }
    }
}