namespace FluentSpring.Context.Configuration
{
    public interface ICanBindInterface<T>
    {
        ICanSetAConstraint<T> To<X>() where X : T;
    }
}