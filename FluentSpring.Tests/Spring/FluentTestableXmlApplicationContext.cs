using FluentSpring.Context.Objects.Factory;
using FluentSpring.Context.Support;
using Spring.Context.Support;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Tests.Spring
{
    public class FluentTestableXmlApplicationContext : XmlApplicationContext
    {
        private readonly IFluentObjectDefinitionRegistry _objectDefinitionLoader = FluentStaticConfiguration.ObjectDefinitionLoader;

        public FluentTestableXmlApplicationContext(string[] locations) : base(locations)
        {
        }

        protected override void LoadObjectDefinitions(DefaultListableObjectFactory objectFactory)
        {
            _objectDefinitionLoader.LoadObjectDefinitions(objectFactory);
            base.LoadObjectDefinitions(objectFactory);
        }
    }
}