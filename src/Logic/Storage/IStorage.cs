namespace Logic.Storage
{
    public interface IStorage
    {
        bool Contains(StorageItemType key);
        bool TryGet<T>(StorageItemType key, out T obj);
        bool Store<T>(StorageItemType key, T obj);
    }
}
