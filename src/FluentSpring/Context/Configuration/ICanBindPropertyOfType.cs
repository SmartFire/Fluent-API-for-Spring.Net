

namespace FluentSpring.Context.Configuration
{
    public interface ICanBindPropertyOfType<in T>
    {
        ICanInjectPropertyOfType With<X>() where X : T;
        ICanInjectPropertyOfType With<X>(string identifier) where X : T;
        ICanInjectPropertyOfType With(T propertyValue);
    }
}
