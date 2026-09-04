using CMS.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.Base;
using System.Globalization;

namespace ETG.Module.Booking.Extensions
{
    public static class ContainerCustomDataExtension
    {
        public static string GetStringCartItemValue(this ContainerCustomData customData, string columnName)
        {   
            return customData?.GetValue(columnName)?.ToString();
        }


        public static int GetIntCartItemValue(this ContainerCustomData customData, string columnName)
        {
            var obj = customData?.GetValue(columnName);
            if (obj == null)
            {
                return 0;
            }
            return obj.ToInteger(0);
        }
        public static double GetDoubleCartItemValue(this ContainerCustomData customData, string columnName)
        {
            var obj = customData?.GetValue(columnName);
            if (obj == null)
            {
                return 0;
            }
            return obj.ToDouble(0.0, CultureInfo.CurrentCulture.Name);
        }
        public static bool GetBooleanCartItemValue(this ContainerCustomData customData, string columnName)
        {
            var obj = customData?.GetValue(columnName);
            if (obj == null)
            {
                return false;
            }
            return obj.ToBoolean(false);
        }

        public static DateTime GetDateCartItemValue(this ContainerCustomData customData, string columnName)
        {
            var date = customData.GetStringCartItemValue(columnName);


            if (date == null)
            {
                return DateTime.MinValue;
            }
            DateTime dt;
            if (DateTime.TryParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dt))
            {
                return dt;
            }

            return DateTime.MinValue;
        }
        public static T GetObject<T>(this ContainerCustomData customData, string tagName)
        {
            if (customData == null)
            {
                return default(T);
            }

            var json = GetStringCartItemValue(customData, tagName);
            if (json.IsNullOrEmpty())
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
