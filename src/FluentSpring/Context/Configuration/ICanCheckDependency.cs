using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Configuration
{
    public interface ICanCheckDependency<out T>
    {
        T WithDependency(DependencyCheckingMode dependencyMode);
    }
}