namespace FluentSpring.Context.Configuration
{
    public interface ICanBindDictionaryKey<T, X>
    {
        ICanBindDictionaryValue<T, X> WithKey<V>() where V : T;
        ICanBindDictionaryValue<T, X> WithKey<V>(string keyIdentifier) where V : T;
        ICanBindDictionaryValue<T, X> WithKey(T keyValue);
        ICanBindDictionaryValue<T, X> WithKeyDefinition(ICanReturnConfigurationParser<T> inlineDefinition);
    }


}