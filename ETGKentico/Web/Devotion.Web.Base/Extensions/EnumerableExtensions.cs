using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace Devotion.Web.Base.Extensions
{
    public static class EnumerableExtensions
    {
        public static string GetStringValue(this Dictionary<string, string> dic, string key)
        {
            if (dic == null || !dic.ContainsKey(key))
            {
                return string.Empty;
            }

            return dic[key];
        }

        public static double GetDoubleValue(this Dictionary<string, string> dic, string key)
        {
            if (dic == null || !dic.ContainsKey(key))
            {
                return 0;
            }

            double.TryParse(dic[key], out var d);
            return d;
        }

        public static int GetIntValue(this Dictionary<string, string> dic, string key)
        {
            if (dic == null || !dic.ContainsKey(key))
            {
                return 0;
            }

            int.TryParse(dic[key], out var d);
            return d;
        }
        public static List<Guid> ToGuidList(this List<string> list)
        {
            if (list.IsNullOrEmpty())
            {
                return null;
            }
            return list.Select(a=>a.ToGuid()).ToList();
        }

    }
}
