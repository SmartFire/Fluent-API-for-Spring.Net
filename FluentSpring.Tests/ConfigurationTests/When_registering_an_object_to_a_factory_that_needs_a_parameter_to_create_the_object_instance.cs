using System.Collections.Generic;
using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;
using FluentSpring.Context.Extensions;

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

    public class Repositories
    {
        public IDictionary<string, IRepository> Registry { get; set; }

        public IRepository<T> GetRepository<T>()
        {
            return (IRepository<T>)Registry[typeof(T).FullName];
        }
    }

    public class TestObjectRepository : IRepository<TestObject>
    {

        public TestObject GetById(string id)
        {
            return new TestObject {PropertyX = id};
        }
    }

    public interface IRepository<out T> : IRepository
    {
        T GetById(string id);
    }

    public interface IRepository
    {
        
    }

    public static class RepositoryExtensions
    {
        public static T GetObjectById<T>(this IApplicationContext applicationContext, string id)
        {
            var repositories = applicationContext.GetObject<Repositories>();
            IRepository<T> objectRepository = repositories.GetRepository<T>();
            return objectRepository.GetById(id);
        }
    }
}
