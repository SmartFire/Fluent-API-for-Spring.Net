using System.Collections.Specialized;

namespace FluentSpring.Context.Configuration
{
    public interface ICanAddNameValueEntry : ICanReturnConfigurationParser<NameValueCollection>
    {
        ICanBindNameValueKey AddEntry();
    }
}
