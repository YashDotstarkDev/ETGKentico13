using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Core.Extensions
{
    /// <summary>
    /// <para>
    /// Used as a wrapper to indicate that an Astro prop was dynamic (originally defined in the cut from a javascript
    /// varible rather than a hard-coded HTML attribute).
    /// </para>
    /// <para>
    /// Eg: <c>message="Hello world."</c> would be static, while <c>message="{message}"</c> would be dynamic.
    /// </para>
    /// </summary>
    /// <remarks>
    /// This is doing the same job as an attribute would, but applied to the dictionary values that define our Astro
    /// props. It should only be used on top-level properties (not nested data).
    /// </remarks>
    public class DynamicAstroProp
    {
        public DynamicAstroProp(object value)
        {
            Value = value;
        }

        public object Value { get; set; }
    }

    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts a dictionary into an object of a given type.
        /// </summary>
        public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
        {
            if (source == null)
            {
                return null;
            }

            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                    .GetProperty(item.Key)
                    .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        /// <summary>
        /// <para>
        /// In our CSHTML views, Astro/Vue props are encoded in the <c>&lt;astro-island&gt;</c> element's <c>props</c>
        /// attribute as stringified JSON (which is HTML-encoded).
        /// </para>
        /// <para>
        /// There's a quirk though: Astro is expecting each prop value to be an array with two elements, where the first
        /// element seems to be an integer that indicates whether the value is dynamic (originally defined in the cut
        /// from a javascript variable rather than a hard-coded HTML attribute).
        /// </para>
        /// <para>
        /// Use the <c>DynamicAstroProp</c> class as a wrapper to indicate that the prop was dynamic.
        /// </para>
        /// <para>Eg, when definining your Astro props dictionary, you'll have to look at the Astro source code to
        /// see how the values were originally defined:</para>
        /// <list type="bullet">
        /// <item>
        /// <term><c>message="Hello world."</c> --&gt;</term>
        /// <description><c>{ "message", "Hello world." } // Static</c></description>
        /// </item>
        /// <item>
        /// <term><c>message="{message}"</c> --&gt;</term>
        /// <description><c>{ "message", DynamicAstroProp(message) } // Dynamic</c></description>
        /// </item>
        /// </list>
        /// </summary>
        public static string AsEncodedAstroProps(this Dictionary<string, object> props)
        {
            if (props == null)
            {
                return null;
            }

            var astroProps = RecursivelyEncodeAstroProps(props);

            var json = astroProps.AsJson(
                indented: false,
                forceCamelCase: true,
                processDictionaryKeys: true);

            return json;
        }

        /// <summary>
        /// Merges the values of a dictionary of string arrays.
        /// </summary>
        /// <remarks>
        /// Does not mutates the existing array.
        /// </remarks>
        public static Dictionary<string, string[]> MergeArrayItems(
            this Dictionary<string, string[]> existing, Dictionary<string, string[]> incoming)
        {
            var output = new Dictionary<string, string[]>();
            var allKeys = existing.Keys.Union(incoming.Keys);
            foreach (var key in allKeys)
            {
                var existingValues = existing.ContainsKey(key) ? existing[key] : Array.Empty<string>();
                var incomingValues = incoming.ContainsKey(key) ? incoming[key] : Array.Empty<string>();
                output[key] = existingValues.Union(incomingValues).ToArray();
            }

            return output;
        }

        /// <remarks>
        /// We have to recursively encode the Astro props, because we could have nested data.
        /// </remarks>
        private static Dictionary<string, object[]> RecursivelyEncodeAstroProps(Dictionary<string, object> props) =>
            props.ToDictionary(
                kvp => kvp.Key,
                kvp =>
                {
                    var isDynamic = IsDynamic(kvp.Value, out object value);

                    if (value != null)
                    {
                        var type = value.GetType();

                        if (type.IsPrimitive || type == typeof(string))
                        {
                            // Just use the value.
                        }
                        else if (typeof(IEnumerable).IsAssignableFrom(type))
                        {
                            value = ((IEnumerable<object>)value)
                                .Select(x => new object[] { 0, RecursivelyEncodeAstroProps(x.AsDictionary()) })
                                .ToArray();
                        }
                        else
                        {
                            value = RecursivelyEncodeAstroProps(value.AsDictionary());
                        }
                    }

                    return new object[] { isDynamic ? 1 : 0, value };
                });

        /// <summary>
        /// Determines if a given object is a <c>DynamicAstroProp</c> and outputs the value.
        /// </summary>
        private static bool IsDynamic(object obj, out object value)
        {
            if (obj is DynamicAstroProp)
            {
                value = (obj as DynamicAstroProp).Value;
                return true;
            }

            value = obj;
            return false;
        }
    }
}
