using System;
using FluentSpring.Context;
using FluentSpring.Context.Configuration;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;
using FluentSpring.Context.Extensions;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_assembly_containing_a_configuration_class : FluentContextTestBaseClass
    {
        [Test]
        public void Then_it_should_load_the_configuration_contain_in_the_container_class()
        {
            FluentApplicationContext.Scan(() => AppDomain.CurrentDomain.GetAssemblies());

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            Assert.IsNotNull(context.GetObject<TestObject>());
        }

        [Test]
        public void Then_it_should_not_try_and_load_the_same_configuration_container_twice()
        {
            FluentApplicationContext.Scan(() => AppDomain.CurrentDomain.GetAssemblies());
            FluentApplicationContext.Scan(() => AppDomain.CurrentDomain.GetAssemblies());

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            Assert.IsNotNull(context.GetObject<TestObject>());
        }

        [Test]
        public void Then_it_should_load_all_configuration_containers()
        {
            FluentApplicationContext.Scan(() => AppDomain.CurrentDomain.GetAssemblies());

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            Assert.IsNotNull(context.GetObject<TestObject>());
            Assert.IsNotNull(context.GetObject<TestObject>("secondObject"));
        }

        [Test]
        public void Then_it_should_load_all_configuration_containers_from_current_domain_by_default()
        {
            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            Assert.IsNotNull(context.GetObject<TestObject>());
            Assert.IsNotNull(context.GetObject<TestObject>("secondObject"));
        }

    }

    public class ConfigurationContainer : ICanConfigureApplicationContext
    {
        public void Configure()
        {
            FluentApplicationContext.Register<TestObject>();
        }
    }

    public class SecondConfigurationContainer : ICanConfigureApplicationContext
    {
        public void Configure()
        {
            FluentApplicationContext.Register<TestObject>("secondObject");
        }
    }

}
