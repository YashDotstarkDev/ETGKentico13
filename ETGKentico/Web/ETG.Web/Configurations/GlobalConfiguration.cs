using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CMS.Helpers;

namespace ETG.Web.Configurations
{
    public static class GlobalConfiguration
    {
        public static string FrontEndVersion
        {
            get
            {
                if (HttpContext.Current.Application["FEVersion"] == null)
                {
                    HttpContext.Current.Application["FEVersion"] = ConfigurationManager.AppSettings["FEVersion"];
                }

                return ValidationHelper.GetString(HttpContext.Current.Application["FEVersion"], string.Empty);
            }
        }
    }
}