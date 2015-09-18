using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public partial class Factory
    {
        private static Dictionary<string, Dictionary<string, WeakReference<object>>> _typeDictionaries = new Dictionary<string, Dictionary<string, WeakReference<object>>>();
        private static object thisLock = new object();

        private static Dictionary<string, WeakReference<object>> GetDictionaryForType<T>()
            where T : class, new()
        {
            var typeId = typeof(T).AssemblyQualifiedName;
            if (!_typeDictionaries.ContainsKey(typeId))
            {
                var dictionary = new Dictionary<string, WeakReference<object>>();
                _typeDictionaries.Add(typeId, dictionary);
                return dictionary;
            }
            return _typeDictionaries[typeId];
        }

        public static T Get<T>(string id)
            where T : class, new()
        {
            lock (thisLock)
            {
                var dictionary = GetDictionaryForType<T>();
                if (!dictionary.ContainsKey(id))
                {
                    var instance = Activator.CreateInstance<T>();
                    dictionary.Add(id, new WeakReference<object>(instance));
                    return instance;
                }

                WeakReference<object> weakReference = dictionary[id];
                object target;
                if (!weakReference.TryGetTarget(out target))
                {
                    target = Activator.CreateInstance<T>();
                    weakReference.SetTarget(target);
                }
                return (T)target;
            }
        }
    }

    public partial class Factory
    {
        public static T Get<T>(int id)
            where T : class, new()
        {
            return Get<T>(id.ToString());
        }

        public static T Get<T>(long id)
            where T : class, new()
        {
            return Get<T>(id.ToString());
        }
    }
}
