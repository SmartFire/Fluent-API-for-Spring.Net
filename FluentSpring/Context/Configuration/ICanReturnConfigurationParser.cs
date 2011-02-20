namespace FluentSpring.Context.Configuration
{
    public interface ICanReturnConfigurationParser<T>
    {
        ICanContainConfiguration GetConfigurationParser();
    }
}