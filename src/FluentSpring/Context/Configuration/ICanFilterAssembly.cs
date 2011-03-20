using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSpring.Context.Configuration
{
    public interface ICanFilterAssembly
    {
        /// <summary>
        /// Specify which assemblies the convention will apply
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        ICanFilterType In(Func<IList<Assembly>> assemblies);
    }
}