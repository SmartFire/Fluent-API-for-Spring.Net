using System;
using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class ListBinder<T> : ICanAddListEntry<T>
    {
        private readonly ListDefinitionParser _parser;

        public ListBinder(ListDefinitionParser parser)
        {
            _parser = parser;
        }

        #region ICanAddListEntry<T> Members

        public ICanAddListEntry<T> Add<X>() where X : T
        {
            _parser.AddEntryReference(typeof (X).FullName);
            return this;
        }

        public ICanAddListEntry<T> Add<X>(string identifier) where X : T
        {
            _parser.AddEntryReference(identifier);
            return this;
        }

        public ICanAddListEntry<T> Add(T listValue)
        {
            _parser.AddEntryValue(listValue);
            return this;
        }

        public ICanAddListEntry<T> AddDefinition(ICanReturnConfigurationParser<T> inlineDefinition)
        {
            _parser.AddInlineEntry(f => ((ICanConfigureInlineObject)inlineDefinition.GetConfigurationParser()).GetObject(f));
            return this;
        }

        public ICanContainConfiguration GetConfigurationParser()
        {
            return _parser;
        }

        #endregion
    }
}