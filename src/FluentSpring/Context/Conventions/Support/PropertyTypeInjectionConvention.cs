using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Configuration.Binders;
using Spring.Objects;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Conventions.Support
{
    public class PropertyTypeInjectionConvention : ICanInjectPropertyOfType, IConvention
    {
        private readonly IDictionary<Type, object> _typeInjections = new Dictionary<Type, object>();

        public ICanBindPropertyOfType<X> Inject<X>()
        {
            if (!_typeInjections.ContainsKey(typeof(X)))
            {
                _typeInjections.Add(typeof(X), string.Empty);
            }

            return new PropertyOfTypeBinder<X>(this, typeof(X));
        }

        public void AddInjectedTypeIdentifierName(Type injectedPropertyType, string injectedIdentifierName)
        {
            _typeInjections[injectedPropertyType] = new RuntimeObjectReference(injectedIdentifierName);
        }

        public void AddInjectedTypeValue<T>(Type injectedPropertyType, T propertyValue)
        {
            _typeInjections[injectedPropertyType] = propertyValue;
        }

        public void ApplyConvention(string identifierName, IConfigurableObjectDefinition objectDefinition)
        {
            foreach(PropertyInfo propertyInfo in objectDefinition.ObjectType.GetProperties().Where(p => _typeInjections.ContainsKey(p.PropertyType)))
            {
                objectDefinition.PropertyValues.Add(new PropertyValue(propertyInfo.Name, _typeInjections[propertyInfo.PropertyType]));
            }
        }

    }
}
