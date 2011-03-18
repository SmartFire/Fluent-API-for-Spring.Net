using System;
using System.Collections.Generic;
using FluentSpring.Context.Conventions;

namespace FluentSpring.Context.Configuration
{
    public interface ICanConfigureConvention
    {
        void AddAssemblyTypeFilter(Func<IList<Type>> typeFilter);
        void AddConventionApplicant(IConvention convention);
        bool IsApplicableToType(Type objectType);
        IConvention GetConventionApplicant();
    }
}