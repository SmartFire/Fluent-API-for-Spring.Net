using System;
using System.Linq.Expressions;
using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class ConstructorObjectExpressionBinder<T> : ICanConfigureCreatedObject<T>
    {
        private readonly LambdaObjectDefinitionExpressionParser<T> _configurationParser;

        public ConstructorObjectExpressionBinder(LambdaObjectDefinitionExpressionParser<T> configurationParser)
        {
            _configurationParser = configurationParser;
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

        public void AddConstructorDelegate(Expression<Func<IObjectRegistry, T>> objectCreation)
        {
            _configurationParser.AddConstructionExpression(objectCreation);
        }
    }
}
