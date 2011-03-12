using FluentSpring.Context.Configuration.Conventions;

namespace FluentSpring.Context.Configuration
{
    public interface ICanApplyConvention<T>
    {
        ICanConfigureObject<T> Apply(IConvention convention);
    }
}