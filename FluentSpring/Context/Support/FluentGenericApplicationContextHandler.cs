using System;
using Spring.Context.Support;

namespace FluentSpring.Context.Support
{
    public class FluentGenericApplicationContextHandler : ContextHandler
    {
        protected override Type DefaultApplicationContextType
        {
            get { return typeof (FluentGenericApplicationContext); }
        }
    }
}