using System;
using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_defining_an_abstract_object : FluentContextTestBaseClass
    {
        [Test]
        [ExpectedException(typeof (InvalidCastException))]
        public void Then_I_should_not_be_able_to_get_an_instance_of_that_object()
        {
            FluentApplicationContext.Register<TestObject>().AsAbstract();

            IApplicationContext _applicationContext = _applicationContextContainer.InitialiseContext();

            var testObjec = _applicationContext.GetObject<TestObject>();
        }
    }
}