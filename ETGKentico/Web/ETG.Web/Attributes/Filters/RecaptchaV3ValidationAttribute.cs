using System;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Internal;
using CMS.EventLog;
using CMS.SiteProvider;
using ETG.Core.Services.Validation;
using ETG.Web.Models.Base;
using Newtonsoft.Json;

namespace ETG.Web.Attributes.Filters
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
            if (filterContext.ActionParameters.IsNullOrEmpty() || !(filterContext.ActionParameters.FirstOrDefault().Value is BasePageViewModel))
            {
                return;
            }
            
            var responseToken = ((BasePageViewModel)filterContext.ActionParameters.FirstOrDefault().Value)
                .g_recaptcha_response;
            
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