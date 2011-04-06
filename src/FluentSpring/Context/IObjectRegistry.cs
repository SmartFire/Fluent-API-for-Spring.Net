namespace FluentSpring.Context
{
    public interface IObjectRegistry
    {
        T GetObject<T>();
        T GetObject<T>(string identifier);
    }
}
