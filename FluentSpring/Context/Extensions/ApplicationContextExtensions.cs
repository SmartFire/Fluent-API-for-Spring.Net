using Spring.Context;

namespace FluentSpring.Context.Extensions
{
    public static class ApplicationContextExtensions
    {
        public static T GetObject<T>(this IApplicationContext applicationContext)
        {
            return (T) applicationContext.GetObject(typeof (T).FullName);
        }

        public static T GetObject<T>(this IApplicationContext applicationContext, string identifier)
        {
            return (T) applicationContext.GetObject(identifier);
        }
    }
}