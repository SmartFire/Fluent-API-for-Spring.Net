using System;
using System.Collections.Generic;

namespace FluentSpring.Context.Configuration
{
    public interface ICanBindClass<T> where T : class
    {
        ICanSetAConstraint<T> To(Func<IList<Type>> types);
    }
}
