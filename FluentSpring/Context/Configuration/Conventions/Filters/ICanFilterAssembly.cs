using System;
using System.Reflection;

namespace FluentSpring.Context.Configuration.Conventions.Filters
{
    public interface ICanFilterAssembly
    {
        /// <summary>
        /// Specify which assembly the convention will apply
        /// </summary>
        /// <param name="assemblyCondition">The assembly match condition.</param>
        void In(Func<Assembly, bool> assemblyCondition);
    }
}