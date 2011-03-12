using System;
using System.Reflection;

namespace FluentSpring.Context.Configuration.Conventions.Filters
{
    public interface ICanFilterType
    {
        /// <summary>
        /// For all types which match the condition.
        /// </summary>
        /// <param name="typeCondition">The type condition.</param>
        /// <returns></returns>
        ICanFilterAssembly For(Func<Type, bool> typeCondition);

        /// <summary>
        /// Specify which assembly the convention will apply
        /// </summary>
        /// <param name="assemblyCondition">The assembly match condition.</param>
        void In(Func<Assembly, bool> assemblyCondition);

    }
}
