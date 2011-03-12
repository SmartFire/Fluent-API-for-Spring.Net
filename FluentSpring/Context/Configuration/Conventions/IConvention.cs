using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Configuration.Conventions
{
    public interface IConvention
    {
        void ApplyConvention(IConfigurableObjectDefinition objectDefinition);
    }
}
