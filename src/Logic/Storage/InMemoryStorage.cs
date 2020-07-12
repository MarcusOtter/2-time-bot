using System.Collections.Generic;

namespace Logic.Storage
{
    public class InMemoryStorage : IStorage
    {
        private readonly Dictionary<StorageItemType, object> _objectDictionary = new Dictionary<StorageItemType, object>();

        public InMemoryStorage()
        {
            // TODO: Make new persistent storage
            Store(StorageItemType.ConsumerKey,    "REDACTED");
            Store(StorageItemType.ConsumerSecret, "REDACTED");
            Store(StorageItemType.AccessToken,    "REDACTED");
            Store(StorageItemType.AccessSecret,   "REDACTED");
        }

        public bool Contains(StorageItemType key)
        {
            return _objectDictionary.ContainsKey(key);
        }

        public bool TryGet<T>(StorageItemType key, out T obj)
        {
            obj = default!;

            if (!Contains(key))
            {
                return false;
            }

            if (_objectDictionary[key] is T output)
            {
                obj = output;
            }

            return true;
        }

        public bool Store<T>(StorageItemType key, T obj)
        {
            if (!Contains(key))
            {
                _objectDictionary[key] = obj!;
            }
            else
            {
                _objectDictionary.Add(key, obj!);
            }

            return true;
        }
    }
}
