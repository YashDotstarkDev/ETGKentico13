using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CMSApp.Custom.ETGBooking.Uploader
{
    public static class UploaderHelpers
    {
        public static DateTime GetDateTime(string dateTimeString)
        {
            if (dateTimeString.IsNullOrEmpty() || dateTimeString.Split('/').Length != 3)
            {
                return DateTime.MinValue;
            }

            var arr = dateTimeString.Split('/');
            dateTimeString = arr[0].PadLeft(2, '0') + "/" + arr[1].PadLeft(2, '0') + "/" + arr[2];

            DateTime dt;


            DateTime.TryParseExact(dateTimeString, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dt);

            return dt;
        }
    }
}