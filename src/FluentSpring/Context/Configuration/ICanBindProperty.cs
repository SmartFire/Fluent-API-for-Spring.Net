using System;
using System.Linq.Expressions;

namespace FluentSpring.Context.Configuration
{
    public interface ICanBindProperty<T>
    {
        ICanBindPropertyValue<T, X> Bind<X>(Expression<Func<T, X>> property);
        ICanBindPropertyValue<T, X> Bind<X>(string propertyName);
    }
}