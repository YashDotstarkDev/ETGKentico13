using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace Devotion.Web.Base.Extensions
{
    public static class StringExtensions
    {
        public static string LimitLength(this string value, int length)
        {
            if (value.IsNullOrEmpty() || value.Length < length)
            {
                return value;
            }

            return value.Substring(0, length);
        }
        public static string RemoveTilde(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return value.Replace("~", "");
        }

        public static string RemoveQueryString(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                
                return string.Empty;
            }

            var index = value.IndexOf("?");

            if (index <= 1)
            {
                return value;
            }

            return value.Substring(0, index);
        }
        public static int ToInteger(this string value)
        {
            return int.TryParse(value, out var result) ? result : 0;
        }
        public static double ToDouble(this string value)
        {
            return double.TryParse(value, out var result) ? result : 0;
        }
        public static Guid ToGuid(this string value)
        {
            return Guid.TryParse(value, out var result) ? result : Guid.Empty;
        }

        public static bool IsGuid(this string value)
        {
            if (Guid.TryParse(value, out var result))
            {
                return true;
            }

            return false;
        }

        public static List<string> ToStringList(this string value, char separator)
        {
            if (value.IsNullOrEmpty())
            {
                return null;
            }
            return value.Split(separator).ToList();
        }
        public static List<Guid> ToGuidList(this string value, char separator)
        {
            if (value.IsNullOrEmpty())
            {
                return null;
            }
            return value.Split(separator).Where(a => a.IsGuid()).Select(a => a.ToGuid()).ToList();
        }

        public static bool InGuidList(this string value, char separator, List<Guid> guidList)
        {
            if (value.IsNullOrEmpty() || guidList.IsNullOrEmpty())
            {
                return false;
            }

            var guidListToCompare = value.ToGuidList(separator);

            if (guidListToCompare.IsNullOrEmpty())
            {
                return false;
            }

            return guidList.Any(a => guidListToCompare.Contains(a));
        }

        public static bool InStringList(this string value, char separator, List<string> stringList)
        {
            if (value.IsNullOrEmpty() || stringList.IsNullOrEmpty())
            {
                return false;
            }

            var stringListToCompare = value.ToStringList(separator);

            if (stringListToCompare.IsNullOrEmpty())
            {
                return false;
            }

            return stringList.Any(a => stringListToCompare.Contains(a));
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            return char.IsUpper(value[0]) ? $"{char.ToLower(value[0])}{value.Remove(0, 1)}" : value;
        }

        public static string TrimTildePath(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? value : value.Replace("~/","/");
        }

        public static string BeginWithSlash(this string source)
        {
            return $"/{source.TrimStart('/')}";
        }

        public static string EndWithSlash(this string source)
        {
            return $"{source.TrimEnd('/')}/";
        }

        public static string GetUrlPathOnly(this string path)
        {
            if (path.IndexOf("?") > -1)
            {
                path = path.Substring(0, path.IndexOf("?"));
            }

            return path;
        }
    }
}