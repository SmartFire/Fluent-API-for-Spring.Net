using System;
using System.Linq;
using FluentSpring.Context;
using FluentSpring.Context.Conventions.Support;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;
using FluentSpring.Context.Extensions;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_adding_a_convention : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_properties_of_type_defined_in_convention_must_be_injected()
        {
            FluentApplicationContext.RegisterConvention()
                    .For(() => AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("FluentSpring.Tests")).SelectMany(a=>a.GetTypes()).ToList())
                .Apply<PropertyTypeInjectionConvention>()
                    .Inject<IObjectWithPropertiesInterface>().With<ObjectWithProperties>();

            FluentApplicationContext.Register<ObjectWithProperties>();
            FluentApplicationContext.Register<TestObject>();

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            TestObject objectProperties = applicationContext.GetObject<TestObject>();

            Assert.IsNotNull(objectProperties.SomeObject);
        }

        [Test]
        public void Then_the_convention_must_be_applied_to_all_types_matching_the_lookup()
        {
            FluentApplicationContext.RegisterConvention()
                    .For(() => AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("FluentSpring.Tests")).SelectMany(a => a.GetTypes()).ToList())
                .Apply<PropertyTypeInjectionConvention>()
                    .Inject<IObjectWithPropertiesInterface>().With<ObjectWithProperties>();

            FluentApplicationContext.Register<ObjectWithProperties>();
            
            FluentApplicationContext.Register<TestObject>("A");
            FluentApplicationContext.Register<TestObject>("B");

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            TestObject objectA = applicationContext.GetObject<TestObject>("A");
            TestObject objectB = applicationContext.GetObject<TestObject>("B");

            Assert.AreSame(objectA.SomeObject,objectB.SomeObject);
        }

        [Test]
        public void Then_the_convention_is_not_applied_to_types_which_are_not_in_lookup()
        {
            FluentApplicationContext.RegisterConvention()
                    .For(() => AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("AnotherTestLibrary")).SelectMany(a => a.GetTypes()).ToList())
                .Apply<PropertyTypeInjectionConvention>()
                    .Inject<IObjectWithPropertiesInterface>().With<ObjectWithProperties>();

            FluentApplicationContext.Register<ObjectWithProperties>();
            FluentApplicationContext.Register<TestObject>();

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            TestObject objectProperties = applicationContext.GetObject<TestObject>();

            Assert.IsNull(objectProperties.SomeObject);
        }
    }
}
