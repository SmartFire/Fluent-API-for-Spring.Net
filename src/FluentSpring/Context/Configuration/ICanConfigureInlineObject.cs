using FluentSpring.Context.Objects.Factory;

namespace FluentSpring.Context.Configuration
{
    public interface ICanConfigureInlineObject
    {
        object GetObject(IObjectDefinitionService objectDefinitionService);
    }
}