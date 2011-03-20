using System;
using System.Linq.Expressions;

namespace FluentSpring.Context.Configuration
{
    public interface ICanConstruct<T>
    {
        ICanBindConstructorArgument<T, X> BindConstructorArgument<X>();
        ICanBindConstructorArgument<T, X> BindConstructorArgument<X>(string constructorArgumentName);
        ICanBindConstructorArgument<T, X> BindConstructorArgumentAtIndex<X>(int index);

        ICanConfigureObject<T> InitialiseWith(Expression<Action<T>> methodInitialiser);
    }
}