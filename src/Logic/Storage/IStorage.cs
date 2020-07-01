namespace Logic.Storage
{
    public interface IStorage
    {
        bool Contains(StorageItem key);
        bool TryGet<T>(StorageItem key, out T obj);
        bool Store<T>(StorageItem key, T obj);
    }
}
