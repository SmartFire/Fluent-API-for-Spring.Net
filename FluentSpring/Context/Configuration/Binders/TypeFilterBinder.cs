using System;
using System.Collections.Generic;
using System.Reflection;


namespace FluentSpring.Context.Configuration.Binders
{
    public class TypeFilterBinder : ICanFilterType
    {
        private readonly ICanConfigureConvention _conventionParser;

        public TypeFilterBinder(ICanConfigureConvention conventionParser)
        {
            _conventionParser = conventionParser;
        }

        public ICanRegisterConvention For(Func<IList<Type>> assemblyTypeLookup)
        {
            _conventionParser.AddAssemblyTypeFilter(assemblyTypeLookup);
            return new ApplyConventionBinder(_conventionParser);
        }
    }
}
