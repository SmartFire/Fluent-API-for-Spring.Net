using System;
using System.Linq.Expressions;

namespace FluentSpring.Context.Configuration
{
    public interface ICanUseFactory<T>
    {
        ICanConfigureObject<T> IsCreatedWith<X>(Expression<Func<X, T>> factoryMethod);
    }
}