namespace FluentSpring.Context.Configuration
{
    public interface ICanInjectPropertyOfType
    {
        ICanBindPropertyOfType<X> Inject<X>();
    }
}
