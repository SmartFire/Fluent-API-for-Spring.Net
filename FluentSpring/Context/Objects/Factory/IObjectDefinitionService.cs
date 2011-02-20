using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Objects.Factory
{
    public interface IObjectDefinitionService
    {
        IObjectDefinitionFactory Factory { get; }
        IConfigurableListableObjectFactory Registry { get; }
    }
}