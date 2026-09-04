using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Helpers;

namespace ETG.Data.Helpers
{
    public class ResourceHelper
    {
        public static string GetString(string key)
        {
            return ResHelper.GetString(key, "en-au");
        }
    }
}