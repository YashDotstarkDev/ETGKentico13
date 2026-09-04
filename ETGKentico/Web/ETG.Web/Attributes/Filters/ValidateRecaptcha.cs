using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETG.Core.Http;
using ETG.Data.Recaptcha;
using ETG.Web.Validation;

namespace ETG.Web.Attributes.Filters
{
    public class ValidateRecaptcha : ActionFilterAttribute
    {
        private readonly IHttpRequest _httpRequest;
        private readonly IRecaptchaValidator _recaptchaValidator;
        public ValidateRecaptcha()
        {
            _httpRequest = DependencyResolver.Current.GetService<IHttpRequest>();
            _recaptchaValidator = DependencyResolver.Current.GetService<IRecaptchaValidator>();

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var responseToken = filterContext.RequestContext.HttpContext.Request["g-recaptcha-response"];// _httpRequest.GetRequest()["g-recaptcha-response"];


            if (!_recaptchaValidator.Validate(responseToken))
            {

                filterContext.Controller.ViewData.ModelState.AddModelError("Error", "Please prove you are not a robot.");
            }
        }
    }
}