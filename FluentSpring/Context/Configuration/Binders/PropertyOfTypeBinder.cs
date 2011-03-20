using System;

using FluentSpring.Context.Conventions.Support;

namespace FluentSpring.Context.Configuration.Binders
{
    public class PropertyOfTypeBinder<T> : ICanBindPropertyOfType<T>
    {
        private readonly PropertyTypeInjectionConvention _propertyTypeInjectionConvention;
        private readonly Type _injectedPropertyType;

        public PropertyOfTypeBinder(PropertyTypeInjectionConvention propertyTypeInjectionConvention, Type injectedPropertyType)
        {
            _propertyTypeInjectionConvention = propertyTypeInjectionConvention;
            _injectedPropertyType = injectedPropertyType;
        }

        public ICanInjectPropertyOfType With<X>() where X : T
        {
            _propertyTypeInjectionConvention.AddInjectedTypeIdentifierName(_injectedPropertyType, typeof(X).FullName);
            return _propertyTypeInjectionConvention;
        }

        public ICanInjectPropertyOfType With<X>(string identifier) where X : T
        {
            _propertyTypeInjectionConvention.AddInjectedTypeIdentifierName(_injectedPropertyType, identifier);
            return _propertyTypeInjectionConvention;
        }

        public ICanInjectPropertyOfType With(T propertyValue)
        {
            _propertyTypeInjectionConvention.AddInjectedTypeValue(_injectedPropertyType, propertyValue);
            return _propertyTypeInjectionConvention;
        }
    }
}
