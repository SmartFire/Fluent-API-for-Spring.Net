namespace FluentSpring.Context.Configuration
{
    public interface ICanDefineAsAbstract<out T>
    {
        /// <summary>
        /// This object cannot be instantiated
        /// </summary>
        /// <returns></returns>
        T AsAbstract();
    }
}