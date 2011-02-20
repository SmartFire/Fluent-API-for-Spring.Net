using System;
using System.Linq.Expressions;
using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    /// <summary>
    /// Inection configuration of a property belonging to an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="X"></typeparam>
    public class PropertyValueBinder<T, X> : ICanBindPropertyValue<T, X>
    {
        private readonly ICanConfigureObject<T> _canConfigureObjectConfiguration;
        private readonly ObjectDefinitionParser _configurationParser;
        private readonly Expression<Func<T, X>> _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueBinder{T,X}"/> class.
        /// </summary>
        /// <param name="configurationParser">The configuration container.</param>
        /// <param name="property">The property we will inject to.</param>
        /// <param name="canConfigureObjectConfiguration">The parent object configuration.</param>
        public PropertyValueBinder(ObjectDefinitionParser configurationParser, Expression<Func<T, X>> property, ICanConfigureObject<T> canConfigureObjectConfiguration)
        {
            _configurationParser = configurationParser;
            _property = property;
            _canConfigureObjectConfiguration = canConfigureObjectConfiguration;
        }

        #region ICanBindPropertyValue<T,X> Members

        public ICanConfigureObject<T> To<V>() where V : X
        {
            _configurationParser.SetPropertyReference(GetPropertyName(), typeof (V).FullName);
            return _canConfigureObjectConfiguration;
        }

        public ICanConfigureObject<T> To<V>(string identifier) where V : X
        {
            _configurationParser.SetPropertyReference(GetPropertyName(), identifier);
            return _canConfigureObjectConfiguration;
        }

        public ICanConfigureObject<T> ToDefinition(ICanReturnConfigurationParser<X> inlineDefinition)
        {
            _configurationParser.SetPropertyWithInlineDefinition(GetPropertyName(), inlineDefinition.GetConfigurationParser());
            return _canConfigureObjectConfiguration;
        }

        public ICanConfigureObject<T> To(X propertyValue)
        {
            _configurationParser.SetPropertyValue(GetPropertyName(), propertyValue);
            return _canConfigureObjectConfiguration;
        }

        #endregion

        private string GetPropertyName()
        {
            var propertyExpression = (MemberExpression) _property.Body;
            return propertyExpression.Member.Name;
        }
    }
}