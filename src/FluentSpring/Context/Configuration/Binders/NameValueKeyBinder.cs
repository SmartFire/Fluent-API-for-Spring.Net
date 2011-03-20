using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class NameValueKeyBinder : ICanBindNameValueKey
    {
        private readonly NameValueCollectionParser _parser;
        private readonly ICanAddNameValueEntry _nameValueCollectionBinder;

        public NameValueKeyBinder(NameValueCollectionParser parser, ICanAddNameValueEntry nameValueCollectionBinder)
        {
            _parser = parser;
            _nameValueCollectionBinder = nameValueCollectionBinder;
        }

        public ICanBindNameValueValue WithKey(string keyValue)
        {
            return new NameValueValueBinder(keyValue, _parser, _nameValueCollectionBinder);
        }
    }
}