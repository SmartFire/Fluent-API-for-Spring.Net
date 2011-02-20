using System;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Objects.Factory;
using FluentSpring.Context.Objects.Factory.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Support
{
    public class FluentStaticConfiguration
    {
        private static readonly FluentObjectDefinitionLoader _fluentObjectDefinitionLoader = new FluentObjectDefinitionLoader(new DefaultObjectDefinitionFactory());
        private static AutoWiringMode _wiringMode = AutoWiringMode.No;
        private static DependencyCheckingMode _dependencyCheckMode = DependencyCheckingMode.None;

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


        public static IFluentObjectDefinitionRegistry ObjectDefinitionLoader
        {
            get { return _fluentObjectDefinitionLoader; }
        }


        public static void Clear()
        {
            _fluentObjectDefinitionLoader.Clear();
        }

        /// <summary>
        /// Registers the object that contains your object configuration.
        /// </summary>
        /// <param name="configurationParser">The configuration parser.</param>
        public static void RegisterObjectConfiguration(ICanContainConfiguration configurationParser)
        {
            _fluentObjectDefinitionLoader.RegisterObjectConfiguration(configurationParser);
        }
    }
}
