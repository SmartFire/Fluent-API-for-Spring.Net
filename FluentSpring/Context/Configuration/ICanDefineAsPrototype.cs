namespace FluentSpring.Context.Configuration
{
    public interface ICanDefineAsPrototype<out T>
    {
        /// <summary>
        /// This object is a prototype
        /// </summary>
        /// <returns></returns>
        T AsPrototype();
    }
}