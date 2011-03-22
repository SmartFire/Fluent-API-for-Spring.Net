using FluentSpring.Context.Configuration;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Objects.Factory
{
    public interface IFluentObjectDefinitionConfigurationRegistry
    {
        /// <summary>
        /// Loads the object definitions from fluent configurations, build the object definition and load it into the spring object registry.
        /// </summary>
        /// <param name="listableObjectFactory">The listable object factory.</param>
        void LoadObjectDefinitions(IConfigurableListableObjectFactory listableObjectFactory);

        /// <summary>
        /// Registers the fluent object configuration we will later load into the spring factory registry.
        /// </summary>
        /// <param name="objectConfiguration">The object configuration container.</param>
        void RegisterObjectConfiguration(ICanContainConfiguration objectConfiguration);

        /// <summary>
        /// Registers a convention which will be applied when an object definition is loaded onto spring.net
        /// </summary>
        /// <param name="conventionParser">The convention parser.</param>
        void RegisterConvention(ICanConfigureConvention conventionParser);

        /// <summary>
        /// Clears all conventions and configurations which has been registered.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the configuration registry contains configuration ready to be loaded into spring.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance contains configuration; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsConfiguration();
    }
}