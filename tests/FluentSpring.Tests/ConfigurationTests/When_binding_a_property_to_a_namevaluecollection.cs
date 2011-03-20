using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;
using FluentSpring.Context.Extensions;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_binding_a_property_to_a_namevaluecollection : FluentContextTestBaseClass
    {
        [Test]
        public void Then_I_should_get_a_name_value_collection()
        {
            FluentApplicationContext.Register<TestObject>().Bind(t => t.NameValueCollection).ToDefinition(
                Inline.NameValueCollection().AddEntry().WithKey("string").AndValue("value"));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var testObject = applicationContext.GetObject<TestObject>();

            Assert.IsNotNull(testObject.NameValueCollection);
            Assert.AreEqual("value", testObject.NameValueCollection[0]);
        }
    }
}
