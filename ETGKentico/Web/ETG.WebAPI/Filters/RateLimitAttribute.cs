using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace ETG.WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)] 
    public class RateLimitAttribute : ActionFilterAttribute
    {
        public const int RateLimitInSeconds = 20;
        public const string ModelStateKey = "RateLimit";
        public int Seconds { get; set; }
        
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.ModelState.Remove(ModelStateKey);
            var key = $"{actionContext.ControllerContext.ControllerDescriptor.ControllerName}{actionContext.ActionDescriptor.ActionName}{actionContext.Request.RequestUri.Host}";
            
            var allowExecute = false;

            if (HttpRuntime.Cache[key] == null)
            {
                HttpRuntime.Cache.Add(key,
                    true,
                    null,
                    DateTime.Now.AddSeconds(Seconds),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Low,
                    null);
                allowExecute = true;
            }

            if (!allowExecute)
            {
                var modelState = new ModelState();
                modelState.Errors.Add("Exceeded rate limit");
                actionContext.ModelState.Add(ModelStateKey, modelState);
            }
            
        }
    }
}