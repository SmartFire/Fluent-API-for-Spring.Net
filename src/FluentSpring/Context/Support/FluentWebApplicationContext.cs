using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Support
{
    /// <summary>
    /// This is the new IApplicationContext which will call the object definition loader when the object definitions are being refreshed from configuration.
    /// </summary>
    public class FluentWebApplicationContext : WebApplicationContext
    {
        /// <summary>
        /// Create a new WebApplicationContext, loading the definitions
        /// from the given XML resource and also all fluently configured ones.
        /// </summary>
        /// <param name="configurationLocations">Names of configuration resources.</param>
        public FluentWebApplicationContext(params string[] configurationLocations)
            : base(null, false, null, configurationLocations)
        {
        }

        /// <summary>
        /// Create a new WebApplicationContext, loading the definitions
        /// from the given XML resource and also all fluently configured ones.
        /// </summary>
        /// <param name="name">The application context name.</param>
        /// <param name="caseSensitive">Flag specifying whether to make this context case sensitive or not.</param>
        /// <param name="configurationLocations">Names of configuration resources.</param>
        public FluentWebApplicationContext(string name, bool caseSensitive, params string[] configurationLocations)
            : base(name, caseSensitive, null, configurationLocations)
        {
        }

        /// <summary>
        /// Create a new WebApplicationContext with the given parent,
        /// from the given XML resource and also all fluently configured ones.
        /// </summary>
        /// <param name="name">The application context name.</param>
        /// <param name="caseSensitive">Flag specifying whether to make this context case sensitive or not.</param>
        /// <param name="parentContext">The parent context.</param>
        /// <param name="configurationLocations">Names of configuration resources.</param>
        public FluentWebApplicationContext(string name, bool caseSensitive, IApplicationContext parentContext, params string[] configurationLocations)
            : base(name, caseSensitive, parentContext)
        {
        }

        /// <summary>
        /// Loads the object definitions.
        /// 
        /// That's where the call is made to load the fluent configuration.
        /// </summary>
        /// <param name="objectFactory">The object factory.</param>
        protected override void LoadObjectDefinitions(DefaultListableObjectFactory objectFactory)
        {
            FluentStaticConfiguration.LoadConfiguration(objectFactory);
            base.LoadObjectDefinitions(objectFactory);
        }
    }
}