using System;

namespace FluentSpring.Context.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAssignableFrom<T>(this Type objectType)
        {
            return objectType.IsAssignableFrom(typeof (T));
        }
    }
}