namespace FluentSpring.Context.Configuration
{
    public interface ICanScope<out T>
    {
        /// <summary>
        /// This object will be live until it goes out of scope
        /// </summary>
        /// <returns></returns>
        T AsNonSingleton();
        /// <summary>
        /// This object cannot be instantiated
        /// </summary>
        /// <returns></returns>
        T AsAbstract();
        T AsPrototype();
        /// <summary>
        /// This object will be live for the session duration
        /// </summary>
        /// <returns></returns>
        T ForSession();
        /// <summary>
        /// This object will be live for the request duration
        /// </summary>
        /// <returns></returns>
        T ForRequest();
        /// <summary>
        /// This object will be live for the application
        /// </summary>
        /// <returns></returns>
        T ForApplication();

        /// <summary>
        /// Determines whether the singletons will be initialised during the application context load, or on demand.
        /// </summary>
        /// <returns></returns>
        T IsLazyInitialised();
        
    }
}