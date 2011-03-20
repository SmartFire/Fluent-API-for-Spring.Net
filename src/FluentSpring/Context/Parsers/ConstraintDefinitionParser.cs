using System;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Configuration.Binders;

namespace FluentSpring.Context.Parsers
{
    public class ConstraintDefinitionParser<T, V> : ICanSetAConstraint<T>
    {
        private readonly ConditionalBindingDefinitionParser _configurationParser;
        private readonly ConditionalBinder<T> _objectBinder;

        public ConstraintDefinitionParser(ConditionalBindingDefinitionParser configurationParser, ConditionalBinder<T> objectBinder)
        {
            _configurationParser = configurationParser;
            _objectBinder = objectBinder;
        }

        #region ICanSetAConstraint<T> Members

        public ICanBindInterface<T> When(Func<bool> condition)
        {
            _configurationParser.SetCondition(condition);
            return _objectBinder;
        }

        public void When(Func<bool> condition, bool isDefault)
        {
            _configurationParser.SetDefaultCondition(condition);
        }

        #endregion
    }
}