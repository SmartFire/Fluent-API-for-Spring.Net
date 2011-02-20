using System;

namespace FluentSpring.Context.Configuration.Constraints
{
    public static class Is
    {
        public static Func<bool> RunningEnvironment(string environmentName)
        {
            return () => environmentName.Equals(Environment.GetEnvironmentVariable("EDISON_ENVIRONMENT"));
        }
    }
}