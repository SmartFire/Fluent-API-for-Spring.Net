using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class DictionaryKeyBinder<T, X> : ICanBindDictionaryKey<T, X>
    {
        private readonly ICanAddDictionaryEntry<T, X> _genericDictionaryEntryBinder;
        private readonly DictionaryConfigurationParser _dictionaryObjectParser;

        public DictionaryKeyBinder(DictionaryConfigurationParser dictionaryObjectParser, ICanAddDictionaryEntry<T, X> genericDictionaryEntryBinder)
        {
            _dictionaryObjectParser = dictionaryObjectParser;
            _genericDictionaryEntryBinder = genericDictionaryEntryBinder;
        }

        #region ICanBindDictionaryKey<T,X> Members

        public ICanBindDictionaryValue<T, X> WithKey<V>() where V : T
        {
            return new DictionaryValueBinder<T, X>(typeof (V), _dictionaryObjectParser, _genericDictionaryEntryBinder);
        }

        public ICanBindDictionaryValue<T, X> WithKey<V>(string keyIdentifier) where V : T
        {
            return new DictionaryValueBinder<T, X>(keyIdentifier, _dictionaryObjectParser, _genericDictionaryEntryBinder);
        }

        public ICanBindDictionaryValue<T, X> WithKey(T keyValue)
        {
            return new DictionaryValueBinder<T, X>(_dictionaryObjectParser, keyValue, _genericDictionaryEntryBinder);
        }

        public ICanBindDictionaryValue<T, X> WithKeyDefinition(ICanReturnConfigurationParser<T> inlineDefinition)
        {
            return new DictionaryValueBinder<T, X>(_dictionaryObjectParser, _genericDictionaryEntryBinder, inlineDefinition);
        }

        #endregion
    }
}