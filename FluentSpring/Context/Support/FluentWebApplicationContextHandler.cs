using System;
using Common.Logging;
using FluentSpring.Context.Resources;
using Spring.Context;
using Spring.Context.Support;

namespace FluentSpring.Context.Support
{
    /// <summary>
    /// This class inherits from the spring web context handler and change the default application context type to the fluently configurable web application context.
    /// </summary>
    public class FluentWebApplicationContextHandler : WebContextHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (FluentWebApplicationContextHandler));

        protected override Type DefaultApplicationContextType
        {
            get { return typeof (FluentWebApplicationContext); }
        }

        /// <summary>
        /// Handles web specific details of context instantiation.
        /// </summary>
        protected override IApplicationContext InstantiateContext(IApplicationContext parent, object configContext, string contextName, Type contextType, bool caseSensitive, string[] resources)
        {
            if (!contextType.GetType().IsSubclassOf(typeof (FluentWebApplicationContext)))
            {
                Log.Warn(string.Format("This context type {0} does not support fluent configuration for object definitions.", contextType));
            }

            // the get assembly resources method will parse my resource strings I definied in spring config and return me an extended set
            string[] overridenResources = GetAssemblyResources(resources);
            return base.InstantiateContext(parent, configContext, contextName, contextType, caseSensitive, overridenResources);
        }

        private string[] GetAssemblyResources(string[] resources)
        {
            return ResourceParser.GetAssemblyResources(resources);
        }
    }
}