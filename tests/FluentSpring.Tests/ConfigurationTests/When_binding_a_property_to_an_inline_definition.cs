using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_binding_a_property_to_an_inline_definition : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_inline_definition_must_not_clash_with_other_defined_definitions_of_same_type()
        {
            const string identifier = "myobjectname";
            const string myTestValue = "mytestvalue";
            const string myOrignalValue = "original";

            FluentApplicationContext.Register<TestObject>().Bind(t => t.PropertyX).To(myOrignalValue);

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.TestObject)
                .ToDefinition(Inline.Object<TestObject>()
                                  .Bind(t => t.PropertyX).To(myTestValue));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (TestObject) applicationContext.GetObject(typeof (TestObject).FullName);

            Assert.AreEqual(myOrignalValue, initialisedObject.PropertyX);
        }

        [Test]
        public void Then_the_object_property_must_be_initialised()
        {
            const string identifier = "myobjectname";
            const string myTestValue = "mytestvalue";

            FluentApplicationContext.Register<TestObject>().Bind(t => t.PropertyX).To("somevalue");

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.TestObject)
                .ToDefinition(Inline.Object<TestObject>()
                                  .Bind(t => t.PropertyX).To(myTestValue));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithProperties) applicationContext.GetObject(identifier);

            Assert.AreEqual(myTestValue, initialisedObject.TestObject.PropertyX);
        }
    }
}