using System;
using System.Collections.Generic;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Objects.Factory;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Parsers
{
    public class ListDefinitionParser : ICanConfigureInlineObject, ICanContainConfiguration
    {
        private readonly Type _listType;
        public IList<Action<ManagedList, IObjectDefinitionService>> _managedListActions = new List<Action<ManagedList, IObjectDefinitionService>>();

        public ListDefinitionParser(Type listType)
        {
            _listType = listType;
        }

        #region ICanConfigureInlineObject Members

        public object GetObject(IObjectDefinitionService objectDefinitionService)
        {
            var managedList = new ManagedList();
            managedList.ElementTypeName = _listType.FullName;

            foreach (var managedListAction in _managedListActions)
            {
                managedListAction(managedList, objectDefinitionService);
            }

            return managedList;
        }

        #endregion

        public void AddEntryReference(string identifier)
        {
            _managedListActions.Add((o, f) => o.Add(new RuntimeObjectReference(identifier)));
        }

        public void AddEntryValue<T>(T listValue)
        {
            _managedListActions.Add((o, f) => o.Add(listValue));
        }

        public void AddInlineEntry(Func<IObjectDefinitionService, object> inlineObject)
        {
            _managedListActions.Add((o, f) => o.Add(inlineObject(f)));
        }
    }
}