using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ETG.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts any object to a dictionary.
        /// </summary>
        public static Dictionary<string, object> AsDictionary(
            this object source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance) =>
            source.GetType().GetProperties(bindingAttr).ToDictionary(
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null));

        /// <summary>
        /// Returns the object serialised as a JSON string.
        /// </summary>
        /// <param name="source">This object.</param>
        /// <param name="indented">Use white space in output.</param>
        /// <param name="forceCamelCase">
        /// <para>
        /// <c>true</c> = force camelCase output.
        /// </para>
        /// <para>
        /// <c>false</c> = Use <c>JsonProperty</c> attribute or property name verbatim.
        /// </para>
        /// </param>
        /// <param name="processDictionaryKeys">Convert dictionary keys to camelCase too.</param>
        /// <returns>A JSON string.</returns>
        public static string AsJson(
            this object source,
            bool indented = true,
            bool forceCamelCase = true,
            bool processDictionaryKeys = false)
        {
            var contractResolver = forceCamelCase
                ? new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                    {
                        ProcessDictionaryKeys = processDictionaryKeys,
                    },
                }
                : null;

            var formatting = indented
                ? Formatting.Indented
                : Formatting.None;

            return JsonConvert.SerializeObject(source, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = formatting,
            });
        }
    }
}
