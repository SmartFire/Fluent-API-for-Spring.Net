using FluentSpring.Context.Objects.Factory;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory.Support;
using Spring.Util;

namespace FluentSpring.Context.Support
{
    public class FluentGenericApplicationContext : XmlApplicationContext
    {
        /// <summary>
        /// Creates a new instance of the
        /// <see cref="Spring.Context.Support.XmlApplicationContext"/> class,
        /// loading the definitions from the supplied XML resource locations.
        /// </summary>
        /// <remarks>The created context will be case sensitive.</remarks>
        /// <param name="configurationLocations">
        /// Any number of XML based object definition resource locations.
        /// </param>
        public FluentGenericApplicationContext(params string[] configurationLocations)
            : this(true, null, true, null, configurationLocations)
        { }

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="Spring.Context.Support.XmlApplicationContext"/> class,
		/// loading the definitions from the supplied XML resource locations.
		/// </summary>
		/// <param name="caseSensitive">Flag specifying whether to make this context case sensitive or not.</param>
		/// <param name="configurationLocations">
		/// Any number of XML based object definition resource locations.
		/// </param>
        public FluentGenericApplicationContext(bool caseSensitive,
            params string[] configurationLocations)
            : this(true, null, caseSensitive, null, configurationLocations)
        { }
        
        /// <summary>
		/// Creates a new instance of the
		/// <see cref="Spring.Context.Support.XmlApplicationContext"/> class,
		/// loading the definitions from the supplied XML resource locations.
		/// </summary>
		/// <param name="name">The application context name.</param>
        /// <param name="caseSensitive">Flag specifying whether to make this context case sensitive or not.</param>
        /// <param name="configurationLocations">
		/// Any number of XML based object definition resource locations.
		/// </param>
        public FluentGenericApplicationContext(string name, bool caseSensitive,
            params string[] configurationLocations)
            : this(true, name, caseSensitive, null, configurationLocations)
        { }

        /// <summary>
        /// Creates a new instance of the
        /// <see cref="Spring.Context.Support.XmlApplicationContext"/> class,
        /// loading the definitions from the supplied XML resource locations,
        /// with the given <paramref name="parentContext"/>.
        /// </summary>
        /// <param name="parentContext">
        /// The parent context (may be <see langword="null"/>).
        /// </param>
        /// <param name="configurationLocations">
        /// Any number of XML based object definition resource locations.
        /// </param>
        public FluentGenericApplicationContext(
            IApplicationContext parentContext,
            params string[] configurationLocations)
            : this(true, null, true, parentContext, configurationLocations)
        { }

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="Spring.Context.Support.XmlApplicationContext"/> class,
		/// loading the definitions from the supplied XML resource locations,
		/// with the given <paramref name="parentContext"/>.
		/// </summary>
		/// <param name="caseSensitive">Flag specifying whether to make this context case sensitive or not.</param>
		/// <param name="parentContext">
		/// The parent context (may be <see langword="null"/>).
		/// </param>
		/// <param name="configurationLocations">
		/// Any number of XML based object definition resource locations.
		/// </param>
        public FluentGenericApplicationContext(
            bool caseSensitive,
            IApplicationContext parentContext,
            params string[] configurationLocations)
            : this(true, null, caseSensitive, parentContext, configurationLocations)
        { }
        
        /// <summary>
		/// Creates a new instance of the
		/// <see cref="Spring.Context.Support.XmlApplicationContext"/> class,
		/// loading the definitions from the supplied XML resource locations,
		/// with the given <paramref name="parentContext"/>.
		/// </summary>
		/// <param name="name">The application context name.</param>
        /// <param name="caseSensitive">Flag specifying whether to make this context case sensitive or not.</param>
        /// <param name="parentContext">
		/// The parent context (may be <see langword="null"/>).
		/// </param>
		/// <param name="configurationLocations">
		/// Any number of XML based object definition resource locations.
		/// </param>
        public FluentGenericApplicationContext(
            string name,
            bool caseSensitive,
            IApplicationContext parentContext,
            params string[] configurationLocations)
            : this(true, name, caseSensitive, parentContext, configurationLocations)
        {}

        /// <summary>
        /// Creates a new instance of the
        /// <see cref="Spring.Context.Support.XmlApplicationContext"/> class,
        /// loading the definitions from the supplied XML resource locations,
        /// with the given <paramref name="parentContext"/>.
        /// </summary>
        /// <remarks>
        /// This constructor is meant to be used by derived classes. By passing <paramref name="refresh"/>=false, it is
        /// the responsibility of the deriving class to call <see cref="AbstractApplicationContext.Refresh()"/> to initialize the context instance.
        /// </remarks>
        /// <param name="refresh">if true, <see cref="AbstractApplicationContext.Refresh()"/> is called automatically.</param>
        /// <param name="name">The application context name.</param>
        /// <param name="caseSensitive">Flag specifying whether to make this context case sensitive or not.</param>
        /// <param name="parentContext">
        /// The parent context (may be <see langword="null"/>).
        /// </param>
        /// <param name="configurationLocations">
        /// Any number of XML based object definition resource locations.
        /// </param>
        public FluentGenericApplicationContext(
            bool refresh,
            string name,
            bool caseSensitive,
            IApplicationContext parentContext,
            params string[] configurationLocations)
            : base(refresh, name, caseSensitive, parentContext, configurationLocations )
        {
        }

        protected override void LoadObjectDefinitions(DefaultListableObjectFactory objectFactory)
        {
            base.LoadObjectDefinitions(objectFactory);
            FluentStaticConfiguration.LoadConfiguration(objectFactory);
        }
    }
}