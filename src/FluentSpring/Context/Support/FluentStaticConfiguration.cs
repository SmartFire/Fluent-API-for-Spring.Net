using System;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Objects.Factory;
using FluentSpring.Context.Objects.Factory.Support;
using FluentSpring.Context.Parsers;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Support
{
    public class FluentStaticConfiguration
    {
        private static readonly FluentObjectDefinitionLoader _fluentObjectDefinitionRegistry = new FluentObjectDefinitionLoader(new DefaultObjectDefinitionFactory());

        private static AutoWiringMode _wiringMode = AutoWiringMode.No;
        private static DependencyCheckingMode _dependencyCheckMode = DependencyCheckingMode.None;
        

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

        public static IFluentObjectDefinitionRegistry ObjectDefinitionRegistry
        {
            get { return _fluentObjectDefinitionRegistry; }
        }

        public static void Clear()
        {
            _fluentObjectDefinitionRegistry.Clear();
        }

        internal static ICanContainConfiguration GetConfigurationParser<T>()
        {
            return _fluentObjectDefinitionRegistry.GetConfigurationParser(typeof(T).FullName);
        }

        internal static ICanContainConfiguration GetConfigurationParser(Type objectType)
        {
            return _fluentObjectDefinitionRegistry.GetConfigurationParser(objectType.FullName);
        }

        internal static ICanContainConfiguration GetConfigurationParser(string identifier)
        {
            return _fluentObjectDefinitionRegistry.GetConfigurationParser(identifier);
        }

        /// <summary>
        /// Registers the object that contains your object configuration.
        /// </summary>
        /// <param name="configurationParser">The configuration parser.</param>
        public static void RegisterObjectConfiguration(ICanContainConfiguration configurationParser)
        {
            _fluentObjectDefinitionRegistry.RegisterObjectConfiguration(configurationParser);
        }

        public static void RegisterConvention(ICanConfigureConvention conventionConfigurationParser)
        {
            _fluentObjectDefinitionRegistry.RegisterConvention(conventionConfigurationParser);
        }

    }
}
