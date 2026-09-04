using System.Web.Routing;

namespace Devotion.Web.Base.Extensions
{
    public static class RouteDataExtensions
    {
        public static string GetString(this RouteData routeData, string key)
        {
            var value = routeData.Values?[key]?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            return value;
        }

        public static string GetAlias(this RouteData routeData)
        {
            return routeData.GetString("alias");
        }
    }
}