using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;
using Spring.Objects.Factory;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_binding_a_constructor : FluentContextTestBaseClass
    {
        [ExpectedException(typeof (ObjectCreationException))]
        [Test]
        public void Then_it_will_fail_if_the_argument_index_are_wrong()
        {
            FluentApplicationContext.Register<ObjectWithConstructor>("object")
                .BindConstructorArgumentAtIndex<string>(2).To("value")
                .BindConstructorArgumentAtIndex<int>(1).To(1);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            applicationContext.GetObject("object");
        }

        [Test]
        public void Then_the_object_must_be_initialised_with_the_right_values_when_passed_by_index()
        {
            FluentApplicationContext.Register<ObjectWithConstructor>("object")
                .BindConstructorArgumentAtIndex<string>(0).To("value")
                .BindConstructorArgumentAtIndex<int>(1).To(1);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithConstructor) applicationContext.GetObject("object");

            Assert.AreEqual("value", initialisedObject.First);
            Assert.AreEqual(1, initialisedObject.Second);
        }

        [Test]
        public void Then_the_object_must_be_initialised_with_the_right_values_when_passed_by_index_and_inline_definition()
        {
            FluentApplicationContext.Register<ObjectWithConstructor>("object")
                .BindConstructorArgumentAtIndex<TestObject>(0).ToDefinition(
                    Inline.Object<TestObject>().Bind(t => t.PropertyX).To("somevalue"));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithConstructor) applicationContext.GetObject("object");

            Assert.AreEqual("somevalue", initialisedObject.TestObject.PropertyX);
        }

        [Test]
        public void Then_the_object_must_be_initialised_with_the_right_values_when_passed_by_index_and_reference()
        {
            FluentApplicationContext.Register<TestObject>("testobject")
                .Bind(t => t.PropertyX).To("somevalue");

            FluentApplicationContext.Register<ObjectWithConstructor>("object")
                .BindConstructorArgumentAtIndex<TestObject>(0).To<TestObject>("testobject")
                .BindConstructorArgumentAtIndex<string>(1).To("const");

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithConstructor) applicationContext.GetObject("object");

            Assert.AreEqual("somevalue", initialisedObject.TestObject.PropertyX);
            Assert.AreEqual("const", initialisedObject.First);
        }


        [Test]
        public void Then_the_object_must_be_initialised_with_the_right_values_when_passed_by_name()
        {
            FluentApplicationContext.Register<ObjectWithConstructor>("object")
                .BindConstructorArgument<string>("first").To("value")
                .BindConstructorArgument<int>("second").To(1);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithConstructor) applicationContext.GetObject("object");

            Assert.AreEqual("value", initialisedObject.First);
        }

        [Test]
        public void Then_the_object_must_be_initialised_with_the_right_values_when_passed_by_name_and_inline_definition()
        {
            FluentApplicationContext.Register<ObjectWithConstructor>("object")
                .BindConstructorArgument<TestObject>("testObject").ToDefinition(
                    Inline.Object<TestObject>().Bind(t => t.PropertyX).To("somevalue"));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithConstructor) applicationContext.GetObject("object");

            Assert.AreEqual("somevalue", initialisedObject.TestObject.PropertyX);
        }

        [Test]
        public void Then_the_object_must_be_initialised_with_the_right_values_when_passed_by_name_and_reference()
        {
            FluentApplicationContext.Register<TestObject>()
                .Bind(t => t.PropertyX).To("somevalue");

            FluentApplicationContext.Register<ObjectWithConstructor>("object")
                .BindConstructorArgument<TestObject>().To<TestObject>();

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithConstructor) applicationContext.GetObject("object");

            Assert.AreEqual("somevalue", initialisedObject.TestObject.PropertyX);
        }

        [Test]
        public void Then_the_object_must_be_initialised_with_the_right_values_when_passed_by_type()
        {
            FluentApplicationContext.Register<ObjectWithConstructor>("object").BindConstructorArgument<string>().To("value");

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithConstructor) applicationContext.GetObject("object");

            Assert.AreEqual("value", initialisedObject.First);
        }

        [Test]
        public void Then_the_object_must_be_initialised_with_the_right_values_when_passed_by_type_and_inline_definition()
        {
            FluentApplicationContext.Register<ObjectWithConstructor>("object")
                .BindConstructorArgument<TestObject>().ToDefinition(
                    Inline.Object<TestObject>().Bind(t => t.PropertyX).To("somevalue"));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithConstructor) applicationContext.GetObject("object");

            Assert.AreEqual("somevalue", initialisedObject.TestObject.PropertyX);
        }
    }
}