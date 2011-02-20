using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Support
{
    public class FluentGenericApplicationContext : GenericApplicationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericApplicationContext"/> class.
        /// </summary>
        public FluentGenericApplicationContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericApplicationContext"/> class.
        /// </summary>
        /// <param name="caseSensitive">if set to <c>true</c> names in the context are case sensitive.</param>
        public FluentGenericApplicationContext(bool caseSensitive)
            : base(caseSensitive)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericApplicationContext"/> class.
        /// </summary>
        /// <param name="objectFactory">The object factory instance to use for this context.</param>
        public FluentGenericApplicationContext(DefaultListableObjectFactory objectFactory)
            : base(objectFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericApplicationContext"/> class.
        /// </summary>
        /// <param name="parent">The parent application context.</param>
        public FluentGenericApplicationContext(IApplicationContext parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericApplicationContext"/> class.
        /// </summary>
        /// <param name="name">The name of the application context.</param>
        /// <param name="caseSensitive">if set to <c>true</c> names in the context are case sensitive.</param>
        /// <param name="parent">The parent application context.</param>
        public FluentGenericApplicationContext(string name, bool caseSensitive, IApplicationContext parent)
            : base(name, caseSensitive, parent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericApplicationContext"/> class.
        /// </summary>
        /// <param name="objectFactory">The object factory to use for this context</param>
        /// <param name="parent">The parent applicaiton context.</param>
        public FluentGenericApplicationContext(DefaultListableObjectFactory objectFactory, IApplicationContext parent)
            : base(objectFactory, parent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericApplicationContext"/> class.
        /// </summary>
        /// <param name="name">The name of the application context.</param>
        /// <param name="caseSensitive">if set to <c>true</c> names in the context are case sensitive.</param>
        /// <param name="parent">The parent application context.</param>
        /// <param name="objectFactory">The object factory to use for this context</param>
        public FluentGenericApplicationContext(string name, bool caseSensitive, IApplicationContext parent, DefaultListableObjectFactory objectFactory)
            : base(name, caseSensitive, parent, objectFactory)
        {
        }
    }
}