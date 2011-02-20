using System.Collections.Generic;

namespace FluentSpring.Context.Configuration
{
    public interface ICanAddDictionaryEntry<T, V> : ICanReturnConfigurationParser<IDictionary<T, V>>
    {
        ICanBindDictionaryKey<T, V> AddEntry();
    }
}