using Logic.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Storage
{
    public class InMemoryStorage : IStorage
    {
        private readonly Dictionary<string, object> _objectDictionary = new Dictionary<string, object>();

        public bool Contains(string key)
        {
            return _objectDictionary.ContainsKey(key);
        }

        public bool TryGet<T>(string key, out T obj)
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

        public bool Store<T>(string key, T obj)
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
