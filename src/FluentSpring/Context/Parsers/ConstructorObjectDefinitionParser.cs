using System;
using System.Collections;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Conventions;
using FluentSpring.Context.Factories;
using FluentSpring.Context.Objects.Factory;
using Spring.Context;
using Spring.Objects;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Parsers
{
    public class ConstructorObjectDefinitionParser<T> : ICanContainConfiguration, IRegistrableObject
    {
        private readonly string _identifier;
        private static readonly string _constructorFactoryIdentifier = typeof (ConstructorFactory).FullName;
        private Func<IApplicationContext, T> _objectCreation;
        private bool _isSingleton = true;

        public ConstructorObjectDefinitionParser(string identifier)
        {
            _identifier = identifier;
        }

        public ConstructorObjectDefinitionParser() 
        {
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        public Type DomainObjectType
        {
            get { return typeof(T); }
        }

        public void AddConstructor(Func<IApplicationContext, T> objectCreation)
        {
            _objectCreation = objectCreation;
        }

        public IObjectDefinition GetObjectDefinition(IObjectDefinitionService objectDefinitionService)
        {
            if (!objectDefinitionService.Registry.ContainsObjectDefinition(_constructorFactoryIdentifier))
            {
                IConfigurableObjectDefinition constructorDefinition = objectDefinitionService.Factory.CreateObjectDefinition(typeof(ConstructorFactory).FullName, null, AppDomain.CurrentDomain);
                objectDefinitionService.Registry.RegisterObjectDefinition(_constructorFactoryIdentifier, constructorDefinition);
            }

            IConfigurableObjectDefinition objectDefinition = objectDefinitionService.Factory.CreateObjectDefinition(typeof(MethodInvokingFactoryObject).AssemblyQualifiedName, null, AppDomain.CurrentDomain);
            objectDefinition.PropertyValues.Add(new PropertyValue("TargetObject",new RuntimeObjectReference(_constructorFactoryIdentifier)));
            objectDefinition.PropertyValues.Add(new PropertyValue("TargetMethod", string.Format("CreateInstance<{0}>",typeof(T))));
            objectDefinition.PropertyValues.Add(new PropertyValue("Arguments", new ArrayList {_objectCreation}));
            objectDefinition.PropertyValues.Add(new PropertyValue("IsSingleton", _isSingleton));

            return objectDefinition;
        }

        public void AddConvention(IConvention convention)
        {
            // not supported by this parser.
        }

        public void AsNonSingleton()
        {
            _isSingleton = false;
        }
    }
}
