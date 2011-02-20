using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Common.Logging;
using FluentSpring.Context.Configuration;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Objects.Factory.Support
{
    /// <summary>
    /// This class has for purpose to load an object definition it created from the fluent configuration into the spring object factory registry.
    /// </summary>
    public class FluentObjectDefinitionLoader : IFluentObjectDefinitionRegistry
    {
        private static readonly IList<ICanContainConfiguration> _springObjectConfigurations = new List<ICanContainConfiguration>();
        private static readonly ILog Log = LogManager.GetLogger(typeof (FluentObjectDefinitionLoader));
        private readonly IObjectDefinitionFactory _objectDefinitionFactory;

        public FluentObjectDefinitionLoader(IObjectDefinitionFactory objectDefinitionFactory)
        {
            _objectDefinitionFactory = objectDefinitionFactory;
        }

        #region IFluentObjectDefinitionRegistry Members

        /// <summary>
        /// Loads the object definitions from fluent configurations, build the object definition and load it into the spring object registry.
        /// </summary>
        /// <param name="listableObjectFactory">The listable object factory.</param>
        public void LoadObjectDefinitions(IConfigurableListableObjectFactory listableObjectFactory)
        {
            var definitionService = new ObjectDefinitionService(_objectDefinitionFactory, listableObjectFactory);

            foreach (IRegistrableObject registrableObject in _springObjectConfigurations.OfType<IRegistrableObject>().Select(configurationContainer => (configurationContainer)))
            {
                listableObjectFactory.RegisterObjectDefinition(registrableObject.Identifier, registrableObject.GetObjectDefinition(definitionService));
            }
        }

        /// <summary>
        /// Registers the fluent object configuration we will later load into the spring factory registry.
        /// </summary>
        /// <param name="objectConfiguration">The object configuration container.</param>
        public void RegisterObjectConfiguration(ICanContainConfiguration objectConfiguration)
        {
            if (!(objectConfiguration is IRegistrableObject))
            {
                throw new ConfigurationErrorsException(string.Format("This object is not registrable in your application context {0}", objectConfiguration.GetType().FullName));
            }

            _springObjectConfigurations.Add(objectConfiguration);
        }

        #endregion

        public void Clear()
        {
            _springObjectConfigurations.Clear();
        }
    }
}