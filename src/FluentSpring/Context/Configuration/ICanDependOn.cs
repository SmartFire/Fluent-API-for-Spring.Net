namespace FluentSpring.Context.Configuration
{
    public interface ICanDependOn<out T>
    {
        T DependsOn<X>();
        T DependsOn<X>(string identifier);
    }
}