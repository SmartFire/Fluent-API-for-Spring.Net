namespace FluentSpring.Context.Configuration
{
    public interface ICanBindNameValueKey
    {
        ICanBindNameValueValue WithKey(string keyValue);
    }
}
