using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using FluentSpring.Context.Extensions;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_object_with_a_delegate_for_construction : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_object_must_be_instantiated_with_the_context_injected_values()
        {
            FluentApplicationContext.Register<ObjectWithProperties>();
            FluentApplicationContext.Register(
                c => new ObjectWithConstructor(c.GetObject<ObjectWithProperties>()));

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            ObjectWithConstructor objectWithConstructor = context.GetObject<ObjectWithConstructor>();
            ObjectWithProperties childObject = context.GetObject<ObjectWithProperties>();

            Assert.IsNotNull(objectWithConstructor);
            Assert.IsNotNull(objectWithConstructor.ObjectInferface);
            Assert.AreSame(childObject, objectWithConstructor.ObjectInferface);
        }

        [Test]
        public void Then_the_object_must_be_retrievable_by_name_with_the_context_injected_values()
        {
            FluentApplicationContext.Register<ObjectWithProperties>();
            
            FluentApplicationContext.Register(
                c => new ObjectWithConstructor(c.GetObject<ObjectWithProperties>()),"objectA");
            FluentApplicationContext.Register(
                c => new ObjectWithConstructor("somestring"), "objectB");

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            ObjectWithConstructor objectA = context.GetObject<ObjectWithConstructor>("objectA");
            ObjectWithConstructor objectB = context.GetObject<ObjectWithConstructor>("objectB");

            Assert.IsNotNull(objectA.ObjectInferface);
            Assert.AreEqual("somestring", objectB.First);
        }

        [Test]
        public void Then_the_object_must_be_the_same_if_registered_as_singleton()
        {
            FluentApplicationContext.Register<ObjectWithProperties>();
            FluentApplicationContext.Register(
                c => new ObjectWithConstructor(c.GetObject<ObjectWithProperties>()));

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            ObjectWithConstructor objectA = context.GetObject<ObjectWithConstructor>();
            ObjectWithConstructor objectB = context.GetObject<ObjectWithConstructor>();

            Assert.AreSame(objectA, objectB);
        }

        [Test]
        public void Then_the_object_must_not_be_the_same_if_not_registered_as_a_singleton()
        {
            FluentApplicationContext.Register<ObjectWithProperties>();
            FluentApplicationContext.Register(
                c => new ObjectWithConstructor(c.GetObject<ObjectWithProperties>()))
                .AsNonSingleton();

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            ObjectWithConstructor objectA = context.GetObject<ObjectWithConstructor>();
            ObjectWithConstructor objectB = context.GetObject<ObjectWithConstructor>();

            Assert.AreNotSame(objectA, objectB);
        }

        [Test]
        public void Then_the_object_property_binding_must_also_be_injected()
        {
            FluentApplicationContext.Register<ObjectWithProperties>();
            FluentApplicationContext.Register(c => new TestObject
                                                       {
                                                           SomeObject = c.GetObject<ObjectWithProperties>()
                                                       });
                

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            TestObject testObject = context.GetObject<TestObject>();

            Assert.IsNotNull(testObject.SomeObject);
        }
    }
}
