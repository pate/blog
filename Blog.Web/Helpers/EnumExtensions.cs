using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blog.Web.Helpers
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(
                typeof (StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match, or enum value if no match
            return attribs.Length > 0 ? attribs[0].StringValue : value.ToString();
        }

        public static IEnumerable<TSource> NullToEmpty<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                return Enumerable.Empty<TSource>();

            return source;
        }
    }
}