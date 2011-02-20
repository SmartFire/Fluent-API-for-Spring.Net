using System;
using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class ConstructorArgumentBinder<T, X> : ICanBindConstructorArgument<T, X>
    {
        private readonly ObjectDefinitionParser _configurationParser;
        private readonly int _construtorArgumentIndex;
        private readonly string _construtorArgumentName;
        private readonly Type _construtorArgumentType;
        private readonly ObjectBinder<T> _objectBinder;

        public ConstructorArgumentBinder(ObjectDefinitionParser configurationParser, Type construtorArgumentType, ObjectBinder<T> objectBinder)
        {
            _configurationParser = configurationParser;
            _construtorArgumentType = construtorArgumentType;
            _objectBinder = objectBinder;
        }

        public ConstructorArgumentBinder(ObjectDefinitionParser configurationParser, string construtorArgumentName, ObjectBinder<T> objectBinder)
        {
            _configurationParser = configurationParser;
            _construtorArgumentName = construtorArgumentName;
            _objectBinder = objectBinder;
        }

        public ConstructorArgumentBinder(ObjectDefinitionParser configurationParser, int construtorArgumentIndex, ObjectBinder<T> objectBinder)
        {
            _configurationParser = configurationParser;
            _construtorArgumentIndex = construtorArgumentIndex;
            _objectBinder = objectBinder;
        }

        #region ICanBindConstructorArgument<T,X> Members

        public ICanConfigureObject<T> To<V>() where V : X
        {
            if (!string.IsNullOrEmpty(_construtorArgumentName))
            {
                _configurationParser.SetConstructorArgumentWithNameToReference(_construtorArgumentName, typeof (T).FullName);
            }
            else if (_construtorArgumentType != null)
            {
                _configurationParser.SetConstructorArgumentWithTypeToReference(_construtorArgumentType, _construtorArgumentType.FullName);
            }
            else
            {
                _configurationParser.SetConstructorArgumentIndexToReference(_construtorArgumentIndex, typeof (T).FullName);
            }

            return _objectBinder;
        }

        public ICanConfigureObject<T> To<V>(string identifier) where V : X
        {
            if (!string.IsNullOrEmpty(_construtorArgumentName))
            {
                _configurationParser.SetConstructorArgumentWithNameToReference(_construtorArgumentName, identifier);
            }
            else if (_construtorArgumentType != null)
            {
                _configurationParser.SetConstructorArgumentWithTypeToReference(_construtorArgumentType, identifier);
            }
            else
            {
                _configurationParser.SetConstructorArgumentIndexToReference(_construtorArgumentIndex, identifier);
            }

            return _objectBinder;
        }

        public ICanConfigureObject<T> To(X propertyValue)
        {
            if (!string.IsNullOrEmpty(_construtorArgumentName))
            {
                _configurationParser.SetConstructorArgumentWithNameToValue(_construtorArgumentName, propertyValue);
            }
            else if (_construtorArgumentType != null)
            {
                _configurationParser.SetConstructorArgumentWithTypeToValue(_construtorArgumentType, propertyValue);
            }
            else
            {
                _configurationParser.SetConstructorArgumentIndexToValue(_construtorArgumentIndex, propertyValue);
            }
            return _objectBinder;
        }

        public ICanConfigureObject<T> ToDefinition(ICanReturnConfigurationParser<X> inlineDefinition)
        {
            if (!string.IsNullOrEmpty(_construtorArgumentName))
            {
                _configurationParser.SetConstructorArgumentWithNameToInlineDefinition(_construtorArgumentName, inlineDefinition.GetConfigurationParser());
            }
            else if (_construtorArgumentType != null)
            {
                _configurationParser.SetConstructorArgumentWithTypeToInlineDefinition(_construtorArgumentType, inlineDefinition.GetConfigurationParser());
            }
            else
            {
                _configurationParser.SetConstructorArgumentWithIndexToInlineDefinition(_construtorArgumentIndex, inlineDefinition.GetConfigurationParser());
            }

            return _objectBinder;
        }

        #endregion
    }
}