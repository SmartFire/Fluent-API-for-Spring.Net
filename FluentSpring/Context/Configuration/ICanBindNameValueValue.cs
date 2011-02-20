namespace FluentSpring.Context.Configuration
{
    public interface ICanBindNameValueValue
    {
        ICanAddNameValueEntry AndValue(string actualValue);
    }
}