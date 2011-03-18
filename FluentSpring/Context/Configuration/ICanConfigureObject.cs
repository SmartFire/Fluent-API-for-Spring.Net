namespace FluentSpring.Context.Configuration
{
    public interface ICanConfigureObject<T> : ICanBindProperty<T>,
                                              ICanConstruct<T>,
                                              ICanReturnConfigurationParser<T>,
                                              ICanUseFactory<T>,
                                              ICanScope<ICanConfigureObject<T>>,
                                              ICanDefineAsPrototype<ICanConfigureObject<T>>,
                                              ICanDefineAsAbstract<ICanConfigureObject<T>>,
                                              ICanDefineAsSingleton<ICanConfigureObject<T>>,
                                              ICanCheckDependency<ICanConfigureObject<T>>,
                                              ICanAutoWireObject<ICanConfigureObject<T>>,
                                              ICanDestroy<T>,
                                              ICanDependOn<ICanConfigureObject<T>>
                                              
    {
        ICanConfigureObject<T> WithParentDefinition<X>();
        ICanConfigureObject<T> WithParentDefinition<X>(string identifier);
    }
}