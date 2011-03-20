using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_object_to_a_repository : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_associated_repository_must_be_used()
        {
            FluentApplicationContext.Register<TestObjectRepository>();
            FluentApplicationContext.Register<TestObject>();

            FluentApplicationContext.Register<Repositories>()
                .Bind(x => x.Registry).ToDefinition(
                    Inline.Dictionary<string, IRepository>()
                        .AddEntry()
                        .WithKey(typeof(TestObject).FullName).AndValue<TestObjectRepository>()
                );

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            string id = "1";

            var otherTestObject = context.GetObject<TestObject>();
            var testObject = context.GetObjectById<TestObject>(id);

            Assert.AreNotEqual(otherTestObject, testObject);
            Assert.AreEqual(id, testObject.PropertyX);
        }
    }
}