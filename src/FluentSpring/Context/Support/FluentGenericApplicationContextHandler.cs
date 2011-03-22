using System;
using FluentSpring.Context.Resources;
using Spring.Context;
using Spring.Context.Support;

namespace FluentSpring.Context.Support
{
    public class FluentGenericApplicationContextHandler : ContextHandler
    {
        protected override Type DefaultApplicationContextType
        {
            get { return typeof (FluentGenericApplicationContext); }
        }

        protected override IApplicationContext InstantiateContext(IApplicationContext parentContext, object configContext, string contextName, System.Type contextType, bool caseSensitive, string[] resources)
        {
            string[] overridenResources = GetAssemblyResources(resources);

            return base.InstantiateContext(parentContext, configContext, contextName, contextType, caseSensitive, overridenResources);
        }

        private static string[] GetAssemblyResources(string[] resources)
        {
            return ResourceParser.GetAssemblyResources(resources);
        }
    }
}