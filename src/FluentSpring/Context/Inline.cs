using FluentSpring.Context.Configuration;
using FluentSpring.Context.Configuration.Binders;
using FluentSpring.Context.Parsers;

namespace FluentSpring.Context
{
    public class Inline
    {
        public static ICanAddDictionaryEntry<X, Y> Dictionary<X, Y>()
        {
            var configurationParser = new DictionaryConfigurationParser(typeof (X), typeof (Y));
            return new GenericDictionaryEntryBinder<X, Y>(configurationParser);
        }

        public static ICanConfigureObject<T> Object<T>() where T : class
        {
            var configurationParser = new ObjectDefinitionParser(typeof (T));
            return new ObjectBinder<T>(configurationParser);
        }

        public static ICanConfigureObject<T> Object<T>(string identifierName) where T : class
        {
            var configurationParser = new ObjectDefinitionParser(typeof (T), identifierName);
            return new ObjectBinder<T>(configurationParser);
        }

        public static ICanAddListEntry<T> List<T>()
        {
            var listDefinitionParser = new ListDefinitionParser(typeof (T));
            return new ListBinder<T>(listDefinitionParser);
        }

        public static ICanAddNameValueEntry NameValueCollection()
        {
            var nameValueCollectionParser = new NameValueCollectionParser();
            return new NameValueCollectionBinder(nameValueCollectionParser);
        }
    }
}