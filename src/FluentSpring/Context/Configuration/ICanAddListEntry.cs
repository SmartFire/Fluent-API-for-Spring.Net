using System.Collections.Generic;

namespace FluentSpring.Context.Configuration
{
    public interface ICanAddListEntry<T> : ICanReturnConfigurationParser<IList<T>>
    {
        ICanAddListEntry<T> Add<X>() where X : T;
        ICanAddListEntry<T> Add<X>(string identifier) where X : T;
        ICanAddListEntry<T> Add(T listValue);
        ICanAddListEntry<T> AddDefinition(ICanReturnConfigurationParser<T> inlineDefinition);
    }
}