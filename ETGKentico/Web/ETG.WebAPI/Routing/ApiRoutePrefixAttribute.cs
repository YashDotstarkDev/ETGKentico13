using System.Web.Http;

namespace ETG.WebAPI.Routing
{
    public class ApiRoutePrefixAttribute : RoutePrefixAttribute
    {
        public ApiRoutePrefixAttribute(string prefix) : base(prefix) { }

        public override string Prefix => $"api/{base.Prefix}";
    }
}