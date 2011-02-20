using System;
using System.Collections.Generic;
using System.Linq;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Objects.Factory;
using FluentSpring.Context.Support;
using Spring.Objects;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Parsers
{
    public class ObjectDefinitionParser : ICanContainConfiguration, IRegistrableObject, ICanConfigureInlineObject, IObjectDefinitionParser
    {
        private readonly Type _domainObjectType;
        private readonly string _identifier;
        private readonly IList<Action<IConfigurableObjectDefinition, IObjectDefinitionService>> _objectDefinitionActions = new List<Action<IConfigurableObjectDefinition, IObjectDefinitionService>>();
        private readonly IList<string> _objectDependencies = new List<string>();
        private AutoWiringMode _autoWiringMode;
        private DependencyCheckingMode _dependencyCheckMode;
        private string _parentDefinition;
        private ObjectScope _objectScope = ObjectScope.Singleton;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDefinitionParser"/> class.
        /// </summary>
        /// <param name="domainObjectType">Type of the domain object you are going to configure for DI.</param>
        public ObjectDefinitionParser(Type domainObjectType)
            : this(domainObjectType, domainObjectType.FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDefinitionParser"/> class.
        /// </summary>
        /// <param name="domainObjectType">Type of the domain object you are going to configure for DI.</param>
        /// <param name="identifier">An identifier to use when declaring several configuration for the same domain type.</param>
        public ObjectDefinitionParser(Type domainObjectType, string identifier)
        {
            _domainObjectType = domainObjectType;
            _identifier = identifier;
            _autoWiringMode = FluentStaticConfiguration.DefaultWiringMode;
            _dependencyCheckMode = FluentStaticConfiguration.DefaultDependencyCheckingMode;
        }

        #region ICanConfigureInlineObject Members

        public object GetObject(IObjectDefinitionService objectDefinitionService)
        {
            return GetObjectDefinition(objectDefinitionService);
        }

        #endregion

        #region IObjectDefinitionParser Members

        public void AsNonSingleton()
        {
            _objectDefinitionActions.Add((a, f) => a.IsSingleton = false);
        }

        public void SetPropertyReference(string propertyName, string registeredTypeName)
        {
            _objectDefinitionActions.Add((a, f) => a.PropertyValues.Add(new PropertyValue(propertyName, new RuntimeObjectReference(registeredTypeName))));
        }

        public void SetPropertyWithInlineDefinition(string propertyName, ICanContainConfiguration inlineDefinitionParser)
        {
            _objectDefinitionActions.Add((a, f) => a.PropertyValues.Add(new PropertyValue(propertyName, ((ICanConfigureInlineObject) inlineDefinitionParser).GetObject(f))));
        }

        public void SetPropertyValue<T>(string propertyName, T propertyValue)
        {
            _objectDefinitionActions.Add((a, f) => a.PropertyValues.Add(new PropertyValue(propertyName, propertyValue)));
        }

        public void SetConstructorArgumentWithNameToReference(string construtorArgumentName, string registeredTypeName)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddNamedArgumentValue(construtorArgumentName, new RuntimeObjectReference(registeredTypeName)));
        }

        public void SetConstructorArgumentWithTypeToReference(Type construtorArgumentType, string registeredTypeName)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddGenericArgumentValue(new RuntimeObjectReference(registeredTypeName), construtorArgumentType.FullName));
        }

        public void SetConstructorArgumentIndexToReference(int construtorArgumentIndex, string registeredTypeName)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddIndexedArgumentValue(construtorArgumentIndex, new RuntimeObjectReference(registeredTypeName)));
        }

        public void SetConstructorArgumentWithNameToValue<T>(string construtorArgumentName, T propertyValue)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddNamedArgumentValue(construtorArgumentName, propertyValue));
        }

        public void SetConstructorArgumentWithTypeToValue<T>(Type construtorArgumentType, T propertyValue)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddGenericArgumentValue(propertyValue, construtorArgumentType.FullName));
        }

        public void SetConstructorArgumentIndexToValue<T>(int construtorArgumentIndex, T propertyValue)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddIndexedArgumentValue(construtorArgumentIndex, propertyValue));
        }

        public void SetConstructorArgumentWithNameToInlineDefinition(string construtorArgumentName, ICanContainConfiguration inlineDefinitionParser)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddNamedArgumentValue(construtorArgumentName, ((ICanConfigureInlineObject) inlineDefinitionParser).GetObject(f)));
        }

        public void SetConstructorArgumentWithTypeToInlineDefinition(Type construtorArgumentType, ICanContainConfiguration inlineDefinitionParser)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddGenericArgumentValue(((ICanConfigureInlineObject) inlineDefinitionParser).GetObject(f), construtorArgumentType.FullName));
        }

        public void SetConstructorArgumentWithIndexToInlineDefinition(int construtorArgumentIndex, ICanContainConfiguration inlineDefinition)
        {
            _objectDefinitionActions.Add((a, f) => a.ConstructorArgumentValues.AddIndexedArgumentValue(construtorArgumentIndex, ((ICanConfigureInlineObject) inlineDefinition).GetObject(f)));
        }

        public void SetFactoryMethod(string objectRegisteredName, string methodName)
        {
            _objectDefinitionActions.Add((a, f) => a.FactoryObjectName = objectRegisteredName);
            _objectDefinitionActions.Add((a, f) => a.FactoryMethodName = methodName);
        }

        public void AsAbstract()
        {
            _objectDefinitionActions.Add((a, f) => a.IsAbstract = true);
        }

        public void SetDependencyLevel(DependencyCheckingMode dependencyMode)
        {
            _dependencyCheckMode = dependencyMode;
        }

        public void AutoWire(AutoWiringMode wiringMode)
        {
            _autoWiringMode = wiringMode;
        }

        public void InitialiseObjectWithObjectMethod(string methodName)
        {
            _objectDefinitionActions.Add((a, f) => a.InitMethodName = methodName);
        }

        public void DestroyObjectWith(string methodName)
        {
            _objectDefinitionActions.Add((a, f) => a.DestroyMethodName = methodName);
        }

        public void IsLazyInitialised()
        {
            _objectDefinitionActions.Add((a, f) => a.IsLazyInit = true);
        }

        public void AddDependencyOn(string fullName)
        {
            _objectDependencies.Add(fullName);
        }

        public void AddParentDefinition(string parentDefinition)
        {
            _parentDefinition = parentDefinition;
        }

        #endregion

        #region IRegistrableObject Members

        /// <summary>
        /// Gets the type of the domain object registered.
        /// </summary>
        /// <value>The type of the domain object.</value>
        public Type DomainObjectType
        {
            get { return _domainObjectType; }
        }

        /// <summary>
        /// Gets the identifier name of the registered type. By default it will be the domain object type full name.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Creates and initialise the object definition that will be registered in your context.
        /// </summary>
        /// <param name="objectDefinitionService">The object definition service.</param>
        /// <returns></returns>
        public IObjectDefinition GetObjectDefinition(IObjectDefinitionService objectDefinitionService)
        {
            IConfigurableObjectDefinition objectDefinition;
            
            if (String.IsNullOrEmpty(_parentDefinition))
            {
                objectDefinition = objectDefinitionService.Factory.CreateObjectDefinition(DomainObjectType.AssemblyQualifiedName, null, AppDomain.CurrentDomain);
            }
            else
            {
                objectDefinition = objectDefinitionService.Factory.CreateObjectDefinition(DomainObjectType.AssemblyQualifiedName, _parentDefinition, AppDomain.CurrentDomain);
            }

            if (objectDefinition is RootWebObjectDefinition || objectDefinition is ChildWebObjectDefinition)
            {
                ((IWebObjectDefinition) objectDefinition).Scope = _objectScope;
            }
            else
            {
                // not sure if that should really be used here.
                objectDefinition.Scope = _objectScope.ToString();
            }

            foreach (var definitionAction in _objectDefinitionActions)
            {
                definitionAction(objectDefinition, objectDefinitionService);
            }

            // set the defaults or the one specified.
            objectDefinition.DependencyCheck = _dependencyCheckMode;
            objectDefinition.AutowireMode = _autoWiringMode;

            if (_objectDependencies.Count > 0)
            {
                objectDefinition.DependsOn = _objectDependencies.ToArray();
            }

            return objectDefinition;
        }

        #endregion

        public void SetScope(ObjectScope objectScope)
        {
            _objectScope = objectScope;
        }
    }
}