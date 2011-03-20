using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Conventions
{
    public interface IConvention
    {
        void ApplyConvention(string identifierName, IConfigurableObjectDefinition objectDefinition);
    }
}