using System;
using System.Collections.Generic;
using System.Configuration;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Configuration.Constraints;
using FluentSpring.Context.Conventions;
using FluentSpring.Context.Objects.Factory;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Parsers
{
    public class ConditionalBindingDefinitionParser : ICanContainConfiguration, IRegistrableObject
    {
        private static readonly IDictionary<string, IList<ConditionalObjectDefinition>> _constraintRegistry = new Dictionary<string, IList<ConditionalObjectDefinition>>();
        private readonly Type _interfaceType;
        private readonly bool _isStatic;
        private Type _realObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalBindingDefinitionParser"/> class.
        /// The mapped objects to the interface will parsed at load time, the associated conditions will be executed during the load.
        /// When the condition evaluated returns true, it will return an instance of that object and will never execute the conditions again.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        public ConditionalBindingDefinitionParser(Type interfaceType) :
            this(interfaceType, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalBindingDefinitionParser"/> class.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="isStatic">if set to <c>false</c> the resolving of the bound interfaces objects will be exected each time we wish to access the object mapped to the interface.</param>
        public ConditionalBindingDefinitionParser(Type interfaceType, bool isStatic)
        {
            _interfaceType = interfaceType;
            _isStatic = isStatic;
        }

        #region IRegistrableObject Members

        /// <summary>
        /// Creates and initialise the object definition that will be registered in your context.
        /// </summary>
        /// <param name="objectDefinitionService">The object definition service.</param>
        /// <returns></returns>
        public IObjectDefinition GetObjectDefinition(IObjectDefinitionService objectDefinitionService)
        {
            if (_interfaceType == null || String.IsNullOrEmpty(_interfaceType.FullName))
            {
                throw new ConfigurationErrorsException(string.Format("You are trying to bind a type to a null interface!"));
            }

            IConfigurableObjectDefinition objectDefinition;
            var conditionalList = new ManagedList();
            if (!objectDefinitionService.Registry.ContainsObjectDefinition(_interfaceType.FullName))
            {
                objectDefinition = objectDefinitionService.Factory.CreateObjectDefinition(typeof(ConstrainableDuringLoadFactoryObject).AssemblyQualifiedName, null, AppDomain.CurrentDomain);
                objectDefinition.ObjectType = typeof (ConstrainableDuringLoadFactoryObject);
                objectDefinition.PropertyValues.Add("IsSingleton", _isStatic);
                objectDefinition.PropertyValues.Add("ConditionObjectDefinitions", conditionalList);
            }
            else
            {
                objectDefinition = (IConfigurableObjectDefinition) objectDefinitionService.Registry.GetObjectDefinition(_interfaceType.FullName);
                conditionalList = (ManagedList) objectDefinition.PropertyValues.GetPropertyValue("ConditionObjectDefinitions").Value;
            }

            if (_constraintRegistry.ContainsKey(_interfaceType.FullName))
            {
                foreach (ConditionalObjectDefinition conditionalDefinition in _constraintRegistry[_interfaceType.FullName])
                {
                    IConfigurableObjectDefinition definition = objectDefinitionService.Factory.CreateObjectDefinition(typeof(ConditionalObjectDefinition).AssemblyQualifiedName, null, AppDomain.CurrentDomain);
                    definition.ConstructorArgumentValues.AddIndexedArgumentValue(0, conditionalDefinition.Condition);
                    definition.ConstructorArgumentValues.AddIndexedArgumentValue(1, conditionalDefinition.TypeName);
                    definition.ConstructorArgumentValues.AddIndexedArgumentValue(2, conditionalDefinition.IsDefault);

                    var ro = new RuntimeObjectReference(conditionalDefinition.TypeName);
                    definition.PropertyValues.Add("Instance", ro);

                    conditionalList.Add(definition);
                }
            }

            return objectDefinition;
        }

        public void AddConvention(IConvention convention)
        {
            // not supported for this parser - yet...
        }

        public string Identifier
        {
            get { return _interfaceType.FullName; }
        }

        public Type DomainObjectType
        {
            get { return _realObject; }
        }

        #endregion

        public void SetInterfaceRealObject(Type type)
        {
            _realObject = type;
        }

        public void SetCondition(Func<bool> condition)
        {
            CreateAndSetCondition(condition, false);
        }

        private void CreateAndSetCondition(Func<bool> condition, bool isDefault)
        {
            if (_interfaceType == null || String.IsNullOrEmpty(_interfaceType.FullName))
            {
                throw new ConfigurationErrorsException(string.Format("You are trying to bind a type to a null interface!"));
            }
            if (_realObject == null || String.IsNullOrEmpty(_realObject.FullName))
            {
                throw new ConfigurationErrorsException(string.Format("You are trying to bind an interface to a null type!"));
            }

            string interfaceTypeName = _interfaceType.FullName;
            if (!_constraintRegistry.ContainsKey(interfaceTypeName))
            {
                _constraintRegistry.Add(interfaceTypeName, new List<ConditionalObjectDefinition>());
            }
            else
            {
                for (int i = 0; i < _constraintRegistry[interfaceTypeName].Count; i++)
                {
                    if (_constraintRegistry[interfaceTypeName][i].IsDefault)
                    {
                        _constraintRegistry[interfaceTypeName].RemoveAt(i);
                        break;
                    }
                }
            }

            var conditionalObjectDefinition = new ConditionalObjectDefinition(condition, _realObject.FullName, isDefault);
            _constraintRegistry[interfaceTypeName].Add(conditionalObjectDefinition);
        }

        public void SetDefaultCondition(Func<bool> condition)
        {
            CreateAndSetCondition(condition, true);
        }


        public static string GetInterfaceRuntimeTypeName<T>()
        {
            if (_constraintRegistry.ContainsKey(typeof (T).FullName))
            {
                foreach (ConditionalObjectDefinition conditionalDefinition in _constraintRegistry[typeof (T).FullName])
                {
                    if (conditionalDefinition.Condition())
                    {
                        return conditionalDefinition.TypeName;
                    }
                }
            }

            throw new ConfigurationErrorsException(string.Format("Could not find any definition for interface {0}", typeof (T).FullName));
        }


        public static void Clear()
        {
            _constraintRegistry.Clear();
        }
    }
}