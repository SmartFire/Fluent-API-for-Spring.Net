using System;
using FluentSpring.Context.Parsers;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Configuration.Binders
{
    public class DictionaryValueBinder<T, V> : ICanBindDictionaryValue<T, V>
    {
        private readonly ICanAddDictionaryEntry<T, V> _canAddDictionaryEntry;
        private readonly DictionaryConfigurationParser _dictionaryObjectParser;
        private readonly ICanReturnConfigurationParser<T> _inlineDefinition;
        private readonly string _keyReferenceIdentifier;
        private readonly Type _keyRegisteredType;
        private readonly T _keyValue;


        public DictionaryValueBinder(Type keyRegisteredType, DictionaryConfigurationParser dictionaryObjectParser, ICanAddDictionaryEntry<T, V> canAddDictionaryEntry)
        {
            _keyRegisteredType = keyRegisteredType;
            _dictionaryObjectParser = dictionaryObjectParser;
            _canAddDictionaryEntry = canAddDictionaryEntry;
        }

        public DictionaryValueBinder(string keyReferenceIdentifier, DictionaryConfigurationParser dictionaryObjectParser, ICanAddDictionaryEntry<T, V> canAddDictionaryEntry)
        {
            _keyReferenceIdentifier = keyReferenceIdentifier;
            _dictionaryObjectParser = dictionaryObjectParser;
            _canAddDictionaryEntry = canAddDictionaryEntry;
        }

        public DictionaryValueBinder(DictionaryConfigurationParser dictionaryObjectParser, T keyValue, ICanAddDictionaryEntry<T, V> canAddDictionaryEntry)
        {
            _dictionaryObjectParser = dictionaryObjectParser;
            _keyValue = keyValue;
            _canAddDictionaryEntry = canAddDictionaryEntry;
        }

        public DictionaryValueBinder(DictionaryConfigurationParser dictionaryObjectParser, ICanAddDictionaryEntry<T, V> canAddDictionaryEntry, ICanReturnConfigurationParser<T> inlineDefinition)
        {
            _dictionaryObjectParser = dictionaryObjectParser;
            _canAddDictionaryEntry = canAddDictionaryEntry;
            _inlineDefinition = inlineDefinition;
        }

        #region ICanBindDictionaryValue<T,V> Members

        public ICanAddDictionaryEntry<T, V> AndValue<V1>() where V1 : V
        {
            if (_keyRegisteredType != null)
            {
                _dictionaryObjectParser.Add(new RuntimeObjectReference(_keyRegisteredType.FullName), new RuntimeObjectReference(typeof (V1).FullName));
            }
            else if (!string.IsNullOrEmpty(_keyReferenceIdentifier))
            {
                _dictionaryObjectParser.Add(new RuntimeObjectReference(_keyReferenceIdentifier), new RuntimeObjectReference(typeof (V1).FullName));
            }
            else if (_inlineDefinition != null)
            {
                _dictionaryObjectParser.Add<V1>(f => ((ICanConfigureInlineObject) _inlineDefinition.GetConfigurationParser()).GetObject(f), new RuntimeObjectReference(typeof (V1).FullName));
            }
            else
            {
                _dictionaryObjectParser.Add(_keyValue, new RuntimeObjectReference(typeof (V1).FullName));
            }
            return _canAddDictionaryEntry;
        }

        public ICanAddDictionaryEntry<T, V> AndValue<V1>(string valueIdentifier) where V1 : V
        {
            if (_keyRegisteredType != null)
            {
                _dictionaryObjectParser.Add(new RuntimeObjectReference(_keyRegisteredType.FullName), new RuntimeObjectReference(valueIdentifier));
            }
            else if (!string.IsNullOrEmpty(_keyReferenceIdentifier))
            {
                _dictionaryObjectParser.Add(new RuntimeObjectReference(_keyReferenceIdentifier), new RuntimeObjectReference(valueIdentifier));
            }
            else if (_inlineDefinition != null)
            {
                _dictionaryObjectParser.Add<V1>(f => ((ICanConfigureInlineObject) _inlineDefinition.GetConfigurationParser()).GetObject(f), new RuntimeObjectReference(valueIdentifier));
            }
            else
            {
                _dictionaryObjectParser.Add(_keyValue, new RuntimeObjectReference(valueIdentifier));
            }
            return _canAddDictionaryEntry;
        }

        public ICanAddDictionaryEntry<T, V> AndValue(V actualValue)
        {
            if (_keyRegisteredType != null)
            {
                _dictionaryObjectParser.Add(new RuntimeObjectReference(_keyRegisteredType.FullName), actualValue);
            }
            else if (!string.IsNullOrEmpty(_keyReferenceIdentifier))
            {
                _dictionaryObjectParser.Add(new RuntimeObjectReference(_keyReferenceIdentifier), actualValue);
            }
            else if (_inlineDefinition != null)
            {
                _dictionaryObjectParser.Add(f => ((ICanConfigureInlineObject) _inlineDefinition.GetConfigurationParser()).GetObject(f), actualValue);
            }
            else
            {
                _dictionaryObjectParser.Add(_keyValue, actualValue);
            }
            return _canAddDictionaryEntry;
        }

        public ICanAddDictionaryEntry<T, V> AndValueDefinition(ICanReturnConfigurationParser<V> inlineDefinition)
        {
            if (_keyRegisteredType != null)
            {
                _dictionaryObjectParser.Add(new RuntimeObjectReference(_keyRegisteredType.FullName), f => ((ICanConfigureInlineObject)inlineDefinition.GetConfigurationParser()).GetObject(f));
            }
            else if (!string.IsNullOrEmpty(_keyReferenceIdentifier))
            {
                _dictionaryObjectParser.Add(new RuntimeObjectReference(_keyReferenceIdentifier), f => ((ICanConfigureInlineObject)inlineDefinition.GetConfigurationParser()).GetObject(f));
            }
            else if (_inlineDefinition != null)
            {
                _dictionaryObjectParser.Add(f => ((ICanConfigureInlineObject)_inlineDefinition.GetConfigurationParser()).GetObject(f), f => ((ICanConfigureInlineObject)inlineDefinition.GetConfigurationParser()).GetObject(f));
            }
            else
            {
                _dictionaryObjectParser.Add(_keyValue, f => ((ICanConfigureInlineObject)inlineDefinition.GetConfigurationParser()).GetObject(f));
            }
            return _canAddDictionaryEntry;
        }

        #endregion
    }
}