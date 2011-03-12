using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_binding_to_a_property_indexer : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_entries_must_be_added()
        {
            FluentApplicationContext.Register<MappedPropertyResolver<string>>("mappedProperty")
                .Bind(x => x.MapKey).To("KEY")
                .Bind<string>("['DEV']").To("DEV_VALUE")
                .Bind<string>("['KEY']").To("KEY_VALUE");

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var propertyResolver = applicationContext.GetObject("mappedProperty");

            Assert.AreEqual("KEY_VALUE",propertyResolver);

        }
    }
}
