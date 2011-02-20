using System;

namespace FluentSpring.Context.Configuration
{
    public interface ICanSetAConstraint<T>
    {
        ICanBindInterface<T> When(Func<bool> condition);
        void When(Func<bool> condition, bool isDefault);
    }
}