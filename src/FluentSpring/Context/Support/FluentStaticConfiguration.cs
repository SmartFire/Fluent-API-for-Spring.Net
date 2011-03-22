using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Objects.Factory.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Support
{
    public class FluentStaticConfiguration
    {
        private static readonly FluentObjectDefinitionConfigurationRegistry _configurationRegistry = new FluentObjectDefinitionConfigurationRegistry(new DefaultObjectDefinitionFactory());

        private static AutoWiringMode _wiringMode = AutoWiringMode.No;
        private static DependencyCheckingMode _dependencyCheckMode = DependencyCheckingMode.None;
        private static readonly IList<Func<IEnumerable<Assembly>>> _assembliesList = new List<Func<IEnumerable<Assembly>>>();

        static FluentStaticConfiguration()
        {
            RegisterImplementedInterfaces = false;
        }

        internal static AutoWiringMode DefaultWiringMode
        {
            get { return _wiringMode; }
            set { _wiringMode = value; }
        }

        internal static DependencyCheckingMode DefaultDependencyCheckingMode
        {
            get { return _dependencyCheckMode; }
            set { _dependencyCheckMode = value; }
        }

        internal static bool RegisterImplementedInterfaces { get; set; }

        internal static ICanContainConfiguration GetConfigurationParser(string identifier)
        {
            return _configurationRegistry.GetConfigurationParser(identifier);
        }

        /// <summary>
        /// Clears all registered configuration.
        /// </summary>
        public static void Clear()
        {
            _configurationRegistry.Clear();
        }

        /// <summary>
        /// Registers the object that contains your object configuration.
        /// </summary>
        /// <param name="configurationParser">The configuration parser.</param>
        public static void RegisterObjectConfiguration(ICanContainConfiguration configurationParser)
        {
            _configurationRegistry.RegisterObjectConfiguration(configurationParser);
        }

        /// <summary>
        /// Registers the convention to be applied to your configuration upon object definition creation in spring.
        /// </summary>
        /// <param name="conventionConfigurationParser">The convention configuration parser.</param>
        public static void RegisterConvention(ICanConfigureConvention conventionConfigurationParser)
        {
            _configurationRegistry.RegisterConvention(conventionConfigurationParser);
        }

        /// <summary>
        /// Register an assemblies list which will be used to search all class containing FluentSpring configuration.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public static void RegisterAssembliesToScan(Func<IEnumerable<Assembly>> assemblies)
        {
            _assembliesList.Add(assemblies);
        }

        /// <summary>
        /// Load all registered configuration into spring. This method should only be called internally by the FluentApplicationContext, i.e. you shouldn't call it.
        /// </summary>
        /// <param name="objectFactory">The object factory.</param>
        public static void LoadConfiguration(DefaultListableObjectFactory objectFactory)
        {
            // This check is mainly for backward compability and avoid people trying to register their configuration twice.
            if (!_configurationRegistry.ContainsConfiguration())
            {
                if (_assembliesList.Count == 0)
                {
                    _assembliesList.Add(() => AppDomain.CurrentDomain.GetAssemblies());
                }

                IList<Type> applicationContextConfigurerTypes = new List<Type>();
                // only load the configuration once (in case the assembly was registered twice)
                foreach (Type type in from assemblies in _assembliesList
                                      from assembly in assemblies()
                                      from type in assembly.GetTypes()
                                      where type.GetInterfaces().Contains(typeof (ICanConfigureApplicationContext))
                                      where !applicationContextConfigurerTypes.Contains(type)
                                      select type)
                {
                    applicationContextConfigurerTypes.Add(type);
                }
                // load each class containing configuration.
                foreach (ICanConfigureApplicationContext contextConfigurer in
                    applicationContextConfigurerTypes.Select(applicationContextConfigurerType => (ICanConfigureApplicationContext) Activator.CreateInstance(applicationContextConfigurerType)))
                {
                    contextConfigurer.Configure();
                }
            }

            _configurationRegistry.LoadObjectDefinitions(objectFactory);
        }
    }
}