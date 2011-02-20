namespace FluentSpring.Context.Configuration
{
    public interface ICanBindObjectMember<T, X>
    {
        /// <summary>
        /// Binds to a reference of <typeparamref name="V"/>
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        ICanConfigureObject<T> To<V>() where V : X;

        /// <summary>
        /// Binds to a reference of <typeparamref name="V"/> with <paramref name="identifier"/> id.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        ICanConfigureObject<T> To<V>(string identifier) where V : X;

        /// <summary>
        /// Inline binding to whatever definition is passed in <paramref name="inlineDefinition"/>.
        /// </summary>
        /// <param name="inlineDefinition">The inline definition.</param>
        /// <returns></returns>
        ICanConfigureObject<T> ToDefinition(ICanReturnConfigurationParser<X> inlineDefinition);

        /// <summary>
        /// Binds to the specified property value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        ICanConfigureObject<T> To(X propertyValue);
    }
}