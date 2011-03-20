using System;
using System.Collections.Generic;


namespace FluentSpring.Context.Configuration
{
    public interface ICanFilterType
    {
        /// <summary>
        /// Fors all types.
        /// </summary>
        /// <returns></returns>
        ICanRegisterConvention For(Func<IList<Type>> assemblyTypeLookup);
    }
}
