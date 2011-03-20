using System;
using FluentSpring.Context.Parsers;
using Spring.Context;

namespace FluentSpring.Context.Configuration.Binders
{
    public class ConstructorObjectBinder<T> : ICanConfigureCreatedObject<T>
    {
        private readonly ConstructorObjectDefinitionParser<T> _configurationParser;

        public ConstructorObjectBinder(ConstructorObjectDefinitionParser<T> configurationParser)
        {
            _configurationParser = configurationParser;
        }

        public void AddConstructorDelegate(Func<IApplicationContext,T> objectConstructor)
        {
            _configurationParser.AddConstructor(objectConstructor);
        }

        public ICanContainConfiguration GetConfigurationParser()
        {
            return _configurationParser;
        }

        public ICanConfigureCreatedObject<T> AsNonSingleton()
        {
            _configurationParser.AsNonSingleton();
            return this;
        }

    }
}
