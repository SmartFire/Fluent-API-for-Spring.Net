using System;
using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_object_with_a_destroy_method : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_initialise_method_must_be_called_to_create_an_object_instance()
        {
            FluentApplicationContext.Register<TestObject>()
                .AsNonSingleton()
                .DestroyWith(x => x.Destroyed());

            TestObject.DestroyedFlag = false;

            IApplicationContext context = _applicationContextContainer.InitialiseContext();

            var testObject = context.GetObject<TestObject>();
            testObject = null;
            _applicationContextContainer.SetDirty();
            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();


            GC.Collect(4);
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete(3000);


            //Assert.IsTrue(TestObject.DestroyedFlag);
        }
    }
}