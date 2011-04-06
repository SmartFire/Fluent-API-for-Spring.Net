using FluentSpring.Context.Support;
using Spring.Context.Support;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Tests.Spring
{
    public class FluentTestableXmlApplicationContext : XmlApplicationContext
    {
        public FluentTestableXmlApplicationContext(string[] locations) : base(locations)
        {
        }

        protected override void LoadObjectDefinitions(DefaultListableObjectFactory objectFactory)
        {
            FluentStaticConfiguration.LoadConfiguration(objectFactory);
            base.LoadObjectDefinitions(objectFactory);
        }
    }
}