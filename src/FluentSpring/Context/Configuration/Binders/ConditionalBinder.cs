using FluentSpring.Context.Parsers;

namespace FluentSpring.Context.Configuration.Binders
{
    public class ConditionalBinder<T> : ICanBindInterface<T>
    {
        private readonly ConditionalBindingDefinitionParser _parser;

        public ConditionalBinder(ConditionalBindingDefinitionParser parser)
        {
            _parser = parser;
        }

        #region ICanBindInterface<T> Members

        public ICanSetAConstraint<T> To<X>() where X : T
        {
            _parser.SetInterfaceRealObject(typeof (X));
            var constraintDefinitionParser = new ConstraintDefinitionParser<T, X>(_parser, this);
            // default condition, in case the When() isn't called after the To() :-)
            constraintDefinitionParser.When(() => true, true);

            return constraintDefinitionParser;
        }

        #endregion
    }
}