using System;
using System.Collections.Generic;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Conventions;

namespace FluentSpring.Context.Parsers
{
    public class ConventionConfigurationParser : ICanConfigureConvention
    {
        private Func<IList<Type>> _typeFilter;
        private IConvention _conventionApplicant;


        public void AddAssemblyTypeFilter(Func<IList<Type>> typeFilter)
        {
            _typeFilter = typeFilter;
        }

        public void AddConventionApplicant(IConvention convention)
        {
            _conventionApplicant = convention;
        }

        public bool IsApplicableToType(Type objectType)
        {
            bool isApplicable = false;
            foreach (Type type in _typeFilter())
            {
                isApplicable = type.Equals(objectType);
                if (isApplicable)
                    break;
            }
            return isApplicable;
        }

        public IConvention GetConventionApplicant()
        {
            return _conventionApplicant;
        }
    }
}
