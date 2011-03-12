using System;
using System.Collections.Generic;

namespace FluentSpring.Context.Configuration.Conventions
{
    public interface ICanApplyConventionToTypes
    {
        ICanApplyConventionToTypes Apply<X>() where X : IConvention;
        ICanApplyConventionToTypes Apply<X>(Action<X> convention) where X : IConvention;
        ICanApplyConventionToTypes Apply<X>(Func<X> convention);
        void To(Func<IList<Type>> types);
    }
}
