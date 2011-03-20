using System;
using System.Collections.Generic;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Objects.Factory;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Parsers
{
    public class DictionaryConfigurationParser : ICanContainConfiguration, ICanConfigureInlineObject, IDictionaryConfigurationParser
    {
        private readonly Type _keyType;
        private readonly IList<Action<ManagedDictionary, IObjectDefinitionService>> _managedDictionaryAction = new List<Action<ManagedDictionary, IObjectDefinitionService>>();
        private readonly Type _valueType;

        public DictionaryConfigurationParser(Type keyType, Type valueType)
        {
            _keyType = keyType;
            _valueType = valueType;
        }

        #region ICanConfigureInlineObject Members

        public object GetObject(IObjectDefinitionService objectDefinitionService)
        {
            var managedDictionary = new ManagedDictionary();
            managedDictionary.KeyTypeName = _keyType.AssemblyQualifiedName;
            managedDictionary.ValueTypeName = _valueType.AssemblyQualifiedName;

            foreach (var action in _managedDictionaryAction)
            {
                action(managedDictionary, objectDefinitionService);
            }
            return managedDictionary;
        }

        #endregion

        #region IDictionaryConfigurationParser Members

        public void Add(RuntimeObjectReference keyObjectReference, RuntimeObjectReference valueObjectReference)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyObjectReference, valueObjectReference));
        }

        public void Add<T>(T keyValue, RuntimeObjectReference runtimeObjectReference)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyValue, runtimeObjectReference));
        }

        public void Add<V>(RuntimeObjectReference keyObjectReference, V actualValue)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyObjectReference, actualValue));
        }

        public void Add<T, V>(T keyValue, V actualValue)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyValue, actualValue));
        }

        public void Add(RuntimeObjectReference keyObjectReference, Func<IObjectDefinitionService, object> runtimeObjectReference)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyObjectReference, runtimeObjectReference(f)));
        }

        public void Add<T>(T keyValue, Func<IObjectDefinitionService, object> runtimeObjectReference)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyValue, runtimeObjectReference(f)));
        }

        public void Add(Func<IObjectDefinitionService, object> keyObjectReference, Func<IObjectDefinitionService, object> valueReference)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyObjectReference(f), valueReference(f)));
        }

        public void Add<T>(Func<IObjectDefinitionService, object> keyObjectReference, T actualValue)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyObjectReference(f), actualValue));
        }

        public void Add<T>(Func<IObjectDefinitionService, object> keyObjectReference, RuntimeObjectReference actualValue)
        {
            _managedDictionaryAction.Add((d, f) => d.Add(keyObjectReference(f), actualValue));
        }

        #endregion
    }
}