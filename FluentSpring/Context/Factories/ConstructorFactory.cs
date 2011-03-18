using System;
using Spring.Context;

namespace FluentSpring.Context.Factories
{
    public class ConstructorFactory : IApplicationContextAware
    {
        private IApplicationContext _applicationContext;

        public IApplicationContext ApplicationContext
        {
            set { _applicationContext = value; }
        }

        public T CreateInstance<T>(Func<IApplicationContext, T> objectConstructor)
        {
            return objectConstructor(_applicationContext);
        }
    }
}
