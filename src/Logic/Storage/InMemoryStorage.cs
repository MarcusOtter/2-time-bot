using System.Collections.Generic;

namespace Logic.Storage
{
    public class InMemoryStorage : IStorage
    {
        private readonly Dictionary<StorageItem, object> _objectDictionary = new Dictionary<StorageItem, object>();

        public InMemoryStorage()
        {
            // TODO: Make new persistent storage
            Store(StorageItem.ConsumerKey,    "REDACTED");
            Store(StorageItem.ConsumerSecret, "REDACTED");
            Store(StorageItem.AccessToken,    "REDACTED");
            Store(StorageItem.AccessSecret,   "REDACTED");
        }

        public bool Contains(StorageItem key)
        {
            return _objectDictionary.ContainsKey(key);
        }

        public bool TryGet<T>(StorageItem key, out T obj)
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

        public bool Store<T>(StorageItem key, T obj)
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
