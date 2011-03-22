using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Configuration.Binders;
using FluentSpring.Context.Parsers;
using FluentSpring.Context.Support;
using Spring.Context;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context
{
    /// <summary>
    /// This is the entry class to register type and configure DI
    /// 
    /// This is the class where you would change the object definition loader, or the spring object definition factory.
    /// </summary>
    public static class FluentApplicationContext
    {
        public static void Clear()
        {
            FluentStaticConfiguration.Clear();
            ConditionalBindingDefinitionParser.Clear();
        }

        public static void SetSpringDefault(AutoWiringMode wiringMode, DependencyCheckingMode dependencyCheckMode)
        {
            FluentStaticConfiguration.DefaultWiringMode = wiringMode;
            FluentStaticConfiguration.DefaultDependencyCheckingMode = dependencyCheckMode;
        }

        public static ICanConfigureObject<T> Register<T>() where T : class
        {
            return Register<T>(typeof (T).FullName);
        }

        public static ICanConfigureObject<T> Register<T>(string identifierName) where T : class
        {
            ICanContainConfiguration configurationParser = FluentStaticConfiguration.GetConfigurationParser(identifierName);
            if (configurationParser == null)
            {
                configurationParser = new ObjectDefinitionParser(typeof(T), identifierName);
            }

            var objectBinder = new ObjectBinder<T>((ObjectDefinitionParser)configurationParser);

            FluentStaticConfiguration.RegisterObjectConfiguration(configurationParser);
            return objectBinder;
        }

        public static ICanConfigureCreatedObject<T> Register<T>(Func<IApplicationContext, T> objectCreation)
        {
            return Register(objectCreation, typeof (T).FullName);
        }

        public static ICanConfigureCreatedObject<T> Register<T>(Func<IApplicationContext, T> objectCreation, string identifierName)
        {
            ICanContainConfiguration configurationParser = FluentStaticConfiguration.GetConfigurationParser(identifierName);
            if (configurationParser == null)
            {
                configurationParser = new ConstructorObjectDefinitionParser<T>(identifierName);

            }

            var objectBinder = new ConstructorObjectBinder<T>((ConstructorObjectDefinitionParser<T>)configurationParser);
            objectBinder.AddConstructorDelegate(objectCreation);

            FluentStaticConfiguration.RegisterObjectConfiguration(configurationParser);
            return objectBinder;
        }

        public static ICanFilterType RegisterConvention()
        {
            ConventionConfigurationParser conventionConfigurationParser = new ConventionConfigurationParser();
            FluentStaticConfiguration.RegisterConvention(conventionConfigurationParser);
            return new TypeFilterBinder(conventionConfigurationParser);
        }

        /// <summary>
        /// Same as Bind<typeparamref name="T"/>() method, however the registered object resolution will be executed at runtime (i.e. upon the object's request from the context).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isStatic">if set to <c>true</c> [is static].</param>
        /// <returns></returns>
        public static ICanBindInterface<T> Bind<T>(bool isStatic)
        {
            var parser = new ConditionalBindingDefinitionParser(typeof (T), isStatic);
            FluentStaticConfiguration.RegisterObjectConfiguration(parser);
            return new ConditionalBinder<T>(parser);
        }

        /// <summary>
        /// Binds the interface to a registered object. By default the mapped object will be resolved during the application context load, if you want the resolution to happen
        /// at runtime you need to use the Bind<typeparamref name="T"/>(false) method instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICanBindInterface<T> Bind<T>()
        {
            return Bind<T>(true);
        }

        /// <summary>
        /// Scans the specified assemblies for any class which has fluent configuration. 
        /// A class implementing ICanConfigureApplicationContext interface will be created and the Configure() method call.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public static void Scan(Func<IEnumerable<Assembly>> assemblies)
        {
            FluentStaticConfiguration.RegisterAssembliesToScan(assemblies);
        }
    }
}