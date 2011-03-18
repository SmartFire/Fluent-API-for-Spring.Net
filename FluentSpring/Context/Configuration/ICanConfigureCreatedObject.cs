namespace FluentSpring.Context.Configuration
{
    public interface ICanConfigureCreatedObject<T> : ICanReturnConfigurationParser<T>,
                                                     ICanDefineAsSingleton<ICanConfigureCreatedObject<T>>
                                                     
    {
    }
}