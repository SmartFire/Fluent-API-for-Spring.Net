using System;
using System.Linq.Expressions;

namespace FluentSpring.Context.Configuration
{
    public interface ICanDestroy<T>
    {
        ICanConfigureObject<T> DestroyWith(Expression<Action<T>> methodDestroyer);
    }
}