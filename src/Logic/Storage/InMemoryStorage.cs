using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Logic.Storage
{
    public class InMemoryStorage : IStorage
    {
        private readonly Dictionary<StorageItemType, object> _objectDictionary = new Dictionary<StorageItemType, object>();

        public bool Contains(StorageItemType key)
        {
            return _objectDictionary.ContainsKey(key);
        }

        public bool TryGet<T>(StorageItemType key, [NotNullWhen(true)] [MaybeNullWhen(false)] out T obj)
        {
            obj = default;

            if (!Contains(key))
            {
                return false;
            }

            if (_objectDictionary[key] is T output) 
            {
                obj = output;
                return true;
            }

            return false;
        }

        public bool Store<T>(StorageItemType key, T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Cannot store null items.");
            }

            if (!Contains(key))
            {
                _objectDictionary[key] = obj;
            }
            else
            {
                _objectDictionary.Add(key, obj);
            }

            return true;
        }

        public T Get<T>(StorageItemType key)
        {
            return (T) _objectDictionary[key];
        }
    }
}
