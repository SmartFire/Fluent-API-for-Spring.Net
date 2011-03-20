using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Configuration
{
    public interface ICanAutoWireObject<out T>
    {
        T AutoWire(AutoWiringMode wiringMode);
    }
}