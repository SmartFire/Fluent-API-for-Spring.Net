using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Common.Logging;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Objects.Factory.Support
{
    /// <summary>
    /// This class has for purpose to load an object definition it created from the fluent configuration into the spring object factory registry.
    /// </summary>
    public class FluentObjectDefinitionConfigurationRegistry : IFluentObjectDefinitionConfigurationRegistry
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (FluentObjectDefinitionConfigurationRegistry));
        private static readonly IList<ICanContainConfiguration> _springObjectConfigurations = new List<ICanContainConfiguration>();
        private static readonly IList<ICanConfigureConvention> _conventions = new List<ICanConfigureConvention>();

        private readonly IObjectDefinitionFactory _objectDefinitionFactory;

        public FluentObjectDefinitionConfigurationRegistry(IObjectDefinitionFactory objectDefinitionFactory)
        {
            _objectDefinitionFactory = objectDefinitionFactory;
        }

        #region IFluentObjectDefinitionConfigurationRegistry Members

        /// <summary>
        /// Loads the object definitions from fluent configurations, build the object definition and load it into the spring object registry.
        /// </summary>
        /// <param name="listableObjectFactory">The listable object factory.</param>
        public void LoadObjectDefinitions(IConfigurableListableObjectFactory listableObjectFactory)
        {
            var definitionService = new ObjectDefinitionService(_objectDefinitionFactory, listableObjectFactory);

            foreach (IRegistrableObject registrableObject in _springObjectConfigurations
                .OfType<IRegistrableObject>()
                .Select(configurationContainer => (configurationContainer)))
            {
                InitialiseConventions(registrableObject);

                IObjectDefinition objectDefinition = registrableObject.GetObjectDefinition(definitionService);

                listableObjectFactory.RegisterObjectDefinition(registrableObject.Identifier, objectDefinition);

                if (FluentStaticConfiguration.RegisterImplementedInterfaces)
                {
                    foreach (Type @interface in objectDefinition.ObjectType.GetInterfaces())
                    {
                        if (!listableObjectFactory.ContainsObjectDefinition(@interface.FullName))
                        {
                            listableObjectFactory.RegisterObjectDefinition(@interface.FullName, objectDefinition);
                        }
                    }
                }
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
                throw new ConfigurationErrorsException(
                    string.Format("This object is not registrable in your application context {0}",
                                  objectConfiguration.GetType().FullName));
            }

            _springObjectConfigurations.Add(objectConfiguration);
        }

        public void RegisterConvention(ICanConfigureConvention conventionParser)
        {
            _conventions.Add(conventionParser);
        }

        public void Clear()
        {
            _springObjectConfigurations.Clear();
            _conventions.Clear();
        }

        #endregion

        /// <summary>
        /// Goes through the list of conventions registered and create initial configuration.
        /// </summary>
        /// <param name="registrableObject">The registrable object.</param>
        private static void InitialiseConventions(IRegistrableObject registrableObject)
        {
            foreach (ICanConfigureConvention convention in
                _conventions.Where(convention => convention.IsApplicableToType(registrableObject.DomainObjectType)))
            {
                registrableObject.AddConvention(convention.GetConventionApplicant());
            }
        }

        internal ICanContainConfiguration GetConfigurationParser(string identifier)
        {
            return _springObjectConfigurations.FirstOrDefault(springObjectConfiguration => ((IRegistrableObject) springObjectConfiguration).Identifier.Equals(identifier,StringComparison.InvariantCultureIgnoreCase));
        }

        public bool ContainsConfiguration()
        {
            return _springObjectConfigurations.Count > 0;
        }
    }
}