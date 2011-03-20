using System.Collections;
using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_binding_an_inline_list_to_a_property : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_list_must_be_initialised()
        {
            FluentApplicationContext.Register<TestObject>()
                .Bind(x => x.ListOfString).ToDefinition(
                    Inline.List<string>()
                        .Add("first")
                        .Add("second")
                );

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            var testObject = context.GetObject<TestObject>();

            Assert.IsNotNull(testObject.ListOfString);
            Assert.Contains("first", (ICollection)testObject.ListOfString);
            Assert.Contains("second", (ICollection)testObject.ListOfString);
        }

        [Test]
        public void Then_the_list_can_contain_inline_definition()
        {
            FluentApplicationContext.Register<TestObject>()
                .Bind(x => x.ListOfTestObjects).ToDefinition(
                    Inline.List<TestObject>()
                        .AddDefinition(Inline.Object<TestObject>().Bind(x => x.PropertyX).To("blah"))
                );

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            var testObject = context.GetObject<TestObject>();

            Assert.IsNotNull(testObject.ListOfTestObjects);
            Assert.AreEqual("blah", testObject.ListOfTestObjects[0].PropertyX);
        }

        [Test]
        public void Then_the_list_must_be_initialised_with_the_references()
        {
            FluentApplicationContext.Register<TestObject>("first");
            FluentApplicationContext.Register<TestObject>("second");

            FluentApplicationContext.Register<TestObject>()
                .Bind(x => x.ListOfTestObjects).ToDefinition(
                    Inline.List<TestObject>()
                        .Add<TestObject>("first")
                        .Add<TestObject>("second")
                );

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            var testObject = context.GetObject<TestObject>();
            var first = context.GetObject<TestObject>("first");
            var second = context.GetObject<TestObject>("second");

            Assert.IsNotNull(testObject.ListOfTestObjects);
            Assert.Contains(first, (ICollection)testObject.ListOfTestObjects);
            Assert.Contains(second, (ICollection)testObject.ListOfTestObjects);
        }
    }
}