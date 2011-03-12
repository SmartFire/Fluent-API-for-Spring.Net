namespace FluentSpring.Context.Configuration
{
    public interface ICanBindDictionaryValue<T, X>
    {
        ICanAddDictionaryEntry<T, X> AndValue<V>() where V : X;
        ICanAddDictionaryEntry<T, X> AndValue<V>(string valueIdentifier) where V : X;
        ICanAddDictionaryEntry<T, X> AndValue(X actualValue);
        ICanAddDictionaryEntry<T, X> AndValueDefinition(ICanReturnConfigurationParser<X> inlineDefinition);
    }

}