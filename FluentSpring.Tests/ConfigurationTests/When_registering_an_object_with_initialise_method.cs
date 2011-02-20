using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_object_with_initialise_method : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_initialise_method_must_be_called_to_create_an_object_instance()
        {
            FluentApplicationContext.Register<TestObject>().InitialiseWith(x => x.Initialise());

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            var testObject = context.GetObject<TestObject>();

            Assert.AreEqual("InitialisedWithMethod", testObject.PropertyX);
        }
    }
}