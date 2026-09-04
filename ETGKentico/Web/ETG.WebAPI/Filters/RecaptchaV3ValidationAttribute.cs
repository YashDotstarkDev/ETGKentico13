using System;
using System.Web.Mvc;
using CMS.EventLog;
using CMS.SiteProvider;
using ETG.Core.Services.Validation;
using Newtonsoft.Json;

namespace ETG.WebAPI.Attributes.Filters
{
    public class ReCaptchaV3ValidationAttribute : ActionFilterAttribute
    {
        private readonly IReCaptchaV3ValidatorService _recaptchaValidatorService;

        public ReCaptchaV3ValidationAttribute()
        {
            _recaptchaValidatorService = new ReCaptchaV3ValidatorService();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var responseToken = filterContext.RequestContext.HttpContext.Request["g_recaptcha_response"];// _httpRequest.GetRequest()["g-recaptcha-response"];
            

            var response = _recaptchaValidatorService.ValidateCaptchaToken(responseToken, true);
            
            if (!response.Success)
            {
                EventLogProvider.LogWarning(
                    nameof(ReCaptchaV3ValidationAttribute), 
                    "RECAPTCHAV3_FAILED", 
                    new Exception("Recaptcha V3 validation failed or did not pass the threshold."), 
                    SiteContext.CurrentSiteID,
                    null
                    );
                      filterContext.Controller.ViewData.ModelState.AddModelError("Error","Invalid Token");
            }
        }
    }
}