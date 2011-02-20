using System;
using System.Collections;
using Common.Logging;
using Spring.Context;
using Spring.Context.Support;
using Spring.Util;

namespace FluentSpring.Tests.Spring
{
    public abstract class AbstractSpringContextTests
    {
        /// <summary>
        /// Map of context keys returned by subclasses of this class, to
        /// Spring contexts.
        /// </summary>
        private static readonly IDictionary contextKeyToContextMap;

        /// <summary>
        /// Logger available to subclasses.
        /// </summary>
        protected readonly ILog logger;

        /// <summary>
        /// Indicates, whether context instances should be automatically registered with the global <see cref="ContextRegistry"/>.
        /// </summary>
        private bool registerContextWithContextRegistry = true;

        /// <summary>
        /// Static ctor to avoid "beforeFieldInit" problem.
        /// </summary>
        static AbstractSpringContextTests()
        {
            contextKeyToContextMap = new Hashtable();
        }

        /// <summary>
        /// Default constructor for AbstractSpringContextTests.
        /// </summary>
        protected AbstractSpringContextTests()
        {
            logger = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// Controls, whether application context instances will
        /// be registered/unregistered with the global <see cref="ContextRegistry"/>.
        /// Defaults to <c>true</c>.
        /// </summary>
        public bool RegisterContextWithContextRegistry
        {
            get { return registerContextWithContextRegistry; }
            set { registerContextWithContextRegistry = value; }
        }

        /// <summary>
        /// Disposes any cached context instance and removes it from cache.
        /// </summary>
        public static void ClearContextCache()
        {
            foreach (IApplicationContext ctx in contextKeyToContextMap.Values)
            {
                ctx.Dispose();
            }
            contextKeyToContextMap.Clear();
        }

        /// <summary>
        /// Set custom locations dirty. This will cause them to be reloaded
        /// from the cache before the next test case is executed.
        /// </summary>
        /// <remarks>
        /// Call this method only if you change the state of a singleton
        /// object, potentially affecting future tests.
        /// </remarks>
        /// <param name="locations">Locations </param>
        protected void SetDirty(string[] locations)
        {
            SetDirty((object) locations);
        }

        /// <summary>
        /// Set context with <paramref name="contextKey"/> dirty. This will cause 
        /// it to be reloaded from the cache before the next test case is executed.
        /// </summary>
        /// <remarks>
        /// Call this method only if you change the state of a singleton
        /// object, potentially affecting future tests.
        /// </remarks>
        /// <param name="contextKey">Locations </param>
        protected void SetDirty(object contextKey)
        {
            String keyString = ContextKeyString(contextKey);
            var ctx =
                (IConfigurableApplicationContext) contextKeyToContextMap[keyString];
            contextKeyToContextMap.Remove(keyString);

            if (ctx != null)
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// Returns <c>true</c> if context for the specified 
        /// <paramref name="contextKey"/> is cached.
        /// </summary>
        /// <param name="contextKey">Context key to check.</param>
        /// <returns>
        /// <c>true</c> if context for the specified 
        /// <paramref name="contextKey"/> is cached, 
        /// <c>false</c> otherwise.
        /// </returns>
        protected bool HasCachedContext(object contextKey)
        {
            string keyString = ContextKeyString(contextKey);
            return contextKeyToContextMap.Contains(keyString);
        }

        /// <summary>
        /// Converts context key to string.
        /// </summary>
        /// <remarks>
        /// Subclasses can override this to return a string representation of
        /// their contextKey for use in logging.
        /// </remarks>
        /// <param name="contextKey">Context key to convert.</param>
        /// <returns>
        /// String representation of the specified <paramref name="contextKey"/>.  Null if 
        /// contextKey is null.
        /// </returns>
        protected virtual string ContextKeyString(object contextKey)
        {
            if (contextKey == null)
            {
                return null;
            }
            if (contextKey is string[])
            {
                return StringUtils.ArrayToCommaDelimitedString((string[]) contextKey);
            }
            else
            {
                return contextKey.ToString();
            }
        }

        /// <summary>
        /// Caches application context.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <param name="context">Context to cache.</param>
        public void AddContext(object key, IConfigurableApplicationContext context)
        {
            AssertUtils.ArgumentNotNull(context, "context", "ApplicationContext must not be null");
            string keyString = ContextKeyString(key);
            contextKeyToContextMap.Add(keyString, context);

            if (RegisterContextWithContextRegistry
                && !ContextRegistry.IsContextRegistered(context.Name))
            {
                ContextRegistry.RegisterContext(context);
            }
        }

        /// <summary>
        /// Returns cached context if present, or loads it if not.
        /// </summary>
        /// <param name="key">Context key.</param>
        /// <returns>Spring application context associated with the specified key.</returns>
        protected IConfigurableApplicationContext GetContext(object key)
        {
            string keyString = ContextKeyString(key);
            var ctx = (IConfigurableApplicationContext) contextKeyToContextMap[keyString];
            if (ctx == null)
            {
                if (key is string[])
                {
                    ctx = LoadContextLocations((string[]) key);
                }
                else
                {
                    ctx = LoadContext(key);
                }
                AddContext(key, ctx);
            }
            return ctx;
        }


        /// <summary>
        /// Loads application context from the specified resource locations.
        /// </summary>
        /// <param name="locations">Resources to load object definitions from.</param>
        protected virtual IConfigurableApplicationContext LoadContextLocations(string[] locations)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info("Loading config for: " + StringUtils.ArrayToCommaDelimitedString(locations));
            }
            return new FluentTestableXmlApplicationContext(locations);
        }

        /// <summary>
        /// Loads application context based on user-defined key.
        /// </summary>
        /// <remarks>
        /// Unless overriden by the user, this method will alway throw 
        /// a <see cref="NotSupportedException"/>. 
        /// </remarks>
        /// <param name="key">User-defined key.</param>
        protected virtual IConfigurableApplicationContext LoadContext(object key)
        {
            throw new NotSupportedException("Subclasses may override this");
        }
    }
}