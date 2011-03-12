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
        private readonly string _propertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueBinder{T,X}"/> class.
        /// </summary>
        /// <param name="configurationParser">The configuration container.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="canConfigureObjectConfiguration">The parent object configuration.</param>
        public PropertyValueBinder(ObjectDefinitionParser configurationParser, string propertyName, ICanConfigureObject<T> canConfigureObjectConfiguration)
        {
            _configurationParser = configurationParser;
            _propertyName = propertyName;
            _canConfigureObjectConfiguration = canConfigureObjectConfiguration;
        }

        #region ICanBindPropertyValue<T,X> Members

        public ICanConfigureObject<T> To<V>() where V : X
        {
            _configurationParser.SetPropertyReference(_propertyName, typeof (V).FullName);
            return _canConfigureObjectConfiguration;
        }

        public ICanConfigureObject<T> To<V>(string identifier) where V : X
        {
            _configurationParser.SetPropertyReference(_propertyName, identifier);
            return _canConfigureObjectConfiguration;
        }

        public ICanConfigureObject<T> ToDefinition(ICanReturnConfigurationParser<X> inlineDefinition)
        {
            _configurationParser.SetPropertyWithInlineDefinition(_propertyName, inlineDefinition.GetConfigurationParser());
            return _canConfigureObjectConfiguration;
        }

        public ICanConfigureObject<T> To(X propertyValue)
        {
            _configurationParser.SetPropertyValue(_propertyName, propertyValue);
            return _canConfigureObjectConfiguration;
        }

        #endregion

    }
}