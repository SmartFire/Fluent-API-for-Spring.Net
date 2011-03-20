using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class GenericDictionaryEntryBinder<X, Y> : ICanAddDictionaryEntry<X, Y>
    {
        private readonly DictionaryConfigurationParser _configurationParser;

        public GenericDictionaryEntryBinder(DictionaryConfigurationParser configurationParser)
        {
            _configurationParser = configurationParser;
        }

        #region ICanAddDictionaryEntry<X,Y> Members

        public ICanBindDictionaryKey<X, Y> AddEntry()
        {
            return new DictionaryKeyBinder<X, Y>(_configurationParser, this);
        }

        public ICanContainConfiguration GetConfigurationParser()
        {
            return _configurationParser;
        }

        #endregion
    }

}