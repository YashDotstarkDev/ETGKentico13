using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Routing;

namespace Devotion.Web.Base.Extensions
{
    public static class RouteValueExtensions
    {
        public static RouteValueDictionary ToRouteValues(this NameValueCollection col, RouteValueDictionary obj)
        {
            var values = new RouteValueDictionary(obj);
            if (col == null)
            {
                return values;
            }

            foreach (string key in col)
            {
                //values passed in object override those already in collection
                if (key == null || values.ContainsKey(key))
                {
                    continue;
                }

                //For list values, querystring creates more than one value with the same key. It returns here as a comma separated values. Ex: ?q=1&q=2&q=3 returns as q=1,2,3
                //RouteValueDictionary doesn't accept same key value. We need to create a new key by adding index to it. Ex: ?q[0]=1&q[1]=2&q[2]=3
                var value = col[key];
                if (value.Contains(","))
                {
                    var paramValues = value.Split(',');
                    for (var i = 0; i < paramValues.Length; i++)
                    {
                        values.Add($"{key}[{i.ToString()}]", paramValues[i]);
                    }
                }
                else
                {
                    values[key] = col[key];
                }
            }
            return values;
        }

        public static RouteValueDictionary AddRouteValue(
            this RouteValueDictionary result, 
            string key, 
            object value)
        {
            if (result == null)
            {
                return result;
            }

            result[key] = value;
            return result;
        }

        public static RouteValueDictionary RemoveRouteValue(
            this RouteValueDictionary result, 
            string key)
        {
            result.Remove(key);
            return result;
        }

        public static RouteValueDictionary AddRouteValue<T>(
            this RouteValueDictionary result, 
            string key, 
            List<T> values)
        {
            if (values == null)
            {
                return result;
            }
            for (var i = 0; i < values.Count; i++)
            {
                result.Add($"{key}[{i}]", values[i]);
            }

            return result;
        }

    }
}