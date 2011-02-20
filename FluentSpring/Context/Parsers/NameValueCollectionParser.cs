using System;
using System.Collections.Generic;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Objects.Factory;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Parsers
{
    public class NameValueCollectionParser : ICanConfigureInlineObject, ICanContainConfiguration
    {
        private readonly IList<Action<ManagedNameValueCollection, IObjectDefinitionService>> _nameValueCollectionActions = new List<Action<ManagedNameValueCollection, IObjectDefinitionService>>();


        public object GetObject(IObjectDefinitionService objectDefinitionService)
        {
            ManagedNameValueCollection nameValueCollection = new ManagedNameValueCollection();

            foreach (var nameValueCollectionAction in _nameValueCollectionActions)
            {
                nameValueCollectionAction(nameValueCollection, objectDefinitionService);
            }

            return nameValueCollection;
        }

        public void Add(string keyValue, string actualValue)
        {
            _nameValueCollectionActions.Add((m, f) => m.Add(keyValue, actualValue));
        }
    }
}