namespace Logic.Storage
{
    public interface IStorage
    {
        bool Contains(string key);
        bool TryGet<T>(string key, out T obj);
        bool Store<T>(string key, T obj);
    }
}
