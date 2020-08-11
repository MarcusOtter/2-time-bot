using System.Diagnostics.CodeAnalysis;

namespace Logic.Storage
{
    public interface IStorage
    {
        bool Contains(StorageItemType key);
        T Get<T>(StorageItemType key);
        bool TryGet<T>(StorageItemType key, [NotNullWhen(true)] [MaybeNullWhen(false)] out T obj);
        bool Store<T>(StorageItemType key, T obj);
    }
}
