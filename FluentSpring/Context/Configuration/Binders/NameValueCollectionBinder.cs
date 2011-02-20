using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class NameValueCollectionBinder : ICanAddNameValueEntry
    {
        private readonly NameValueCollectionParser _parser;

        public NameValueCollectionBinder(NameValueCollectionParser parser)
        {
            _parser = parser;
        }

        public ICanBindNameValueKey AddEntry()
        {
            return new NameValueKeyBinder(_parser, this);
        }

        public ICanContainConfiguration GetConfigurationParser()
        {
            return _parser;
        }
    }
}
