namespace FluentSpring.Context.Configuration
{
    public interface ICanDefineAsSingleton<out T>
    {
        /// <summary>
        /// This object will be live until it goes out of scope
        /// </summary>
        /// <returns></returns>
        T AsNonSingleton();
    }
}