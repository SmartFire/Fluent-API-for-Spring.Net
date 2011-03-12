namespace FluentSpring.Context.Configuration.Conventions
{
    public interface ICanBindPropertyOfType
    {
        ICanBindPropertyOfTypeToValue<X> Bind<X>();
    }

    public interface ICanBindPropertyOfTypeToValue<in T>
    {
        ICanBindPropertyOfType To<X>();
        ICanBindPropertyOfType To<X>(string identifier);
        ICanBindPropertyOfType To(T propertyValue);
    }
}
