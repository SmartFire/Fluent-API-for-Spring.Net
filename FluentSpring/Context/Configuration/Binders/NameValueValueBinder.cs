using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class NameValueValueBinder : ICanBindNameValueValue
    {
        private readonly NameValueCollectionParser _parser;
        private readonly string _keyValue;
        private readonly ICanAddNameValueEntry _nameValueCollectionBinder;

        public NameValueValueBinder(string keyValue, NameValueCollectionParser parser, ICanAddNameValueEntry nameValueCollectionBinder)
        {
            _keyValue = keyValue;
            _parser = parser;
            _nameValueCollectionBinder = nameValueCollectionBinder;
        }

        public ICanAddNameValueEntry AndValue(string actualValue)
        {
            _parser.Add(_keyValue, actualValue);
            return _nameValueCollectionBinder;
        }

    }
}