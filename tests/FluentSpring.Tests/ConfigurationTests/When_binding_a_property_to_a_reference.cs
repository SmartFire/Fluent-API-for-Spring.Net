using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_binding_a_property_to_a_reference : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_property_must_be_initialised_by_its_registered_identifier()
        {
            const string identifier = "myobjectname";

            FluentApplicationContext.Register<TestObject>("testobject").Bind(t => t.PropertyX).To("somevalue");
            FluentApplicationContext.Register<TestObject>("anothertestobject").Bind(t => t.PropertyX).To("somedifferentvalue");

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.TestObject).To<TestObject>("testobject");

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithProperties) applicationContext.GetObject(identifier);
            var testObject = (TestObject) applicationContext.GetObject("testobject");

            Assert.AreSame(testObject, initialisedObject.TestObject);
            Assert.AreEqual("somevalue", initialisedObject.TestObject.PropertyX);
        }

        [Test]
        public void Then_the_property_must_be_initialised_by_its_registered_reference()
        {
            const string identifier = "myobjectname";

            FluentApplicationContext.Register<TestObject>().Bind(t => t.PropertyX).To("somevalue");

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.TestObject).To<TestObject>();

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithProperties) applicationContext.GetObject(identifier);
            var testObject = (TestObject) applicationContext.GetObject(typeof (TestObject).FullName);

            Assert.AreSame(testObject, initialisedObject.TestObject);
        }
    }
}