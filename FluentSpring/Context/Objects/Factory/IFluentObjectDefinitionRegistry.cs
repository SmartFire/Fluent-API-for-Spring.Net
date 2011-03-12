using FluentSpring.Context.Configuration;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Objects.Factory
{
    public interface IFluentObjectDefinitionRegistry
    {
        void LoadObjectDefinitions(IConfigurableListableObjectFactory listableObjectFactory);
        void RegisterObjectConfiguration(ICanContainConfiguration objectConfiguration);
        
    }
}