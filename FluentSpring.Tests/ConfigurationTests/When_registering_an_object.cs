using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_object : FluentContextTestBaseClass
    {
        [Test]
        public void Then_it_must_return_a_different_instance()
        {
            const string identifierA = "myobjectname";

            FluentApplicationContext.Register<ObjectWithProperties>(identifierA).AsNonSingleton();

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectA = applicationContext.GetObject<ObjectWithProperties>(identifierA);
            var objectB = applicationContext.GetObject<ObjectWithProperties>(identifierA);

            Assert.AreNotSame(objectB, objectA);
        }

        [Test]
        public void Then_it_must_return_the_singleton_instance()
        {
            const string identifierA = "myobjectname";

            FluentApplicationContext.Register(() => Inline.Object<ObjectWithProperties>(identifierA));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectA = applicationContext.GetObject<ObjectWithProperties>(identifierA);
            var objectB = applicationContext.GetObject<ObjectWithProperties>(identifierA);

            Assert.AreSame(objectB, objectA);
        }

        [Test]
        public void Then_it_should_still_be_there_when_the_context_is_refreshed()
        {
            const string identifierA = "myobjectname";

            FluentApplicationContext.Register<ObjectWithProperties>(identifierA).AsNonSingleton();

            _applicationContextContainer.InitialiseContext();

            _applicationContextContainer.SetDirty();

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectA = applicationContext.GetObject<ObjectWithProperties>(identifierA);

            Assert.IsNotNull(objectA);
        }

        [Test]
        public void Then_the_object_must_be_defined_in_spring_object_definitions()
        {
            FluentApplicationContext.Register<ObjectWithProperties>()
                .Bind(x => x.AStringProperty).To("somevalue");

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            bool definitionPresent = applicationContext.ContainsObjectDefinition(typeof (ObjectWithProperties).FullName);

            Assert.IsTrue(definitionPresent);
        }

        [Test]
        public void Then_the_object_must_be_registered_when_using_the_configurer_delegate()
        {
            const string identifier = "myobjectname";

            FluentApplicationContext.Register(() => Inline.Object<ObjectWithProperties>(identifier));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            bool definitionPresent = applicationContext.ContainsObjectDefinition(identifier);

            Assert.IsTrue(definitionPresent);
        }

        [Test]
        public void Then_the_object_must_registered_with_the_identifier_passed_in()
        {
            const string identifier = "myobjectname";

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.AStringProperty).To("somevalue");

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            bool definitionPresent = applicationContext.ContainsObjectDefinition(identifier);

            Assert.IsTrue(definitionPresent);
        }

        [Test]
        public void Then_the_object_of_same_type_with_different_identifiers_must_be_registered()
        {
            const string identifierA = "myobjectname";
            const string identifierB = "mysecondobjectname";

            FluentApplicationContext.Register(() => Inline.Object<ObjectWithProperties>(identifierA));
            FluentApplicationContext.Register(() => Inline.Object<ObjectWithProperties>(identifierB));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            bool definitionAPresent = applicationContext.ContainsObjectDefinition(identifierA);
            bool definitionBPresent = applicationContext.ContainsObjectDefinition(identifierB);

            Assert.IsTrue(definitionAPresent);
            Assert.IsTrue(definitionBPresent);
        }
    }
}