using System.Collections.Generic;
using FluentSpring.Tests.TestObjects;
using Spring.Context;
using FluentSpring.Context.Extensions;

namespace FluentSpring.Tests.ConfigurationTests
{
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
