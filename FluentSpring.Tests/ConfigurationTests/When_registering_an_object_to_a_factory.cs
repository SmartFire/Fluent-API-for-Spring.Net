using System.Configuration;
using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_object_to_a_factory : FluentContextTestBaseClass
    {
        [Test]
        public void Then_it_should_create_the_object_using_the_factory_method()
        {
            FluentApplicationContext.Register<TestObjectFactory>();
            FluentApplicationContext.Register<TestObject>().IsCreatedWith<TestObjectFactory>(b => b.CreateTestObject());

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var testObject = applicationContext.GetObject<TestObject>();

            Assert.IsNotNull(testObject);
            Assert.AreEqual("Factoried", testObject.PropertyX);
        }

        [Test]
        [ExpectedException(typeof (ConfigurationErrorsException))]
        public void Then_it_should_throw_an_exception_if_the_factory_method_has_parameters()
        {
            FluentApplicationContext.Register<TestObjectFactory>();
            FluentApplicationContext.Register<TestObject>().IsCreatedWith<TestObjectFactory>(b => b.CreateTestObject("asdf"));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var testObject = applicationContext.GetObject<TestObject>();
        }
    }

    public class TestObjectFactory
    {
        public TestObject CreateTestObject()
        {
            return new TestObject {PropertyX = "Factoried"};
        }

        public TestObject CreateTestObject(string asdf)
        {
            return new TestObject {PropertyX = asdf};
        }
    }
}