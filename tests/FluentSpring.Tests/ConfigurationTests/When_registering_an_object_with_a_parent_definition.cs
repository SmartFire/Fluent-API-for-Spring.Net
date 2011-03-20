using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_object_with_a_parent_definition : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_sub_properties_must_be_initialised()
        {
                FluentApplicationContext.Register<TestObject>()
                    .Bind(x => x.PropertyX).To("mysubvalue");

                FluentApplicationContext.Register<DerivedObject>()
                    .WithParentDefinition<TestObject>()
                    .Bind(x => x.SomeProperty).To("derived");

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            var testObject = context.GetObject<DerivedObject>();

            Assert.AreEqual("mysubvalue", testObject.PropertyX);
        }

        [Test]
        public void Then_the_sub_property_shouldnt_be_initialised_if_no_parent_selected()
        {
            FluentApplicationContext.Register<TestObject>()
                .Bind(x => x.PropertyX).To("mysubvalue");

            FluentApplicationContext.Register<DerivedObject>()
                .Bind(x => x.SomeProperty).To("derived");

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            var testObject = context.GetObject<DerivedObject>();

            Assert.AreNotEqual("mysubvalue", testObject.PropertyX);
        }
    }
}