using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public partial class Factory
    {
        private static Dictionary<string, WeakReference<object>> _dictionary = new Dictionary<string, WeakReference<object>>();
        private static object thisLock = new object();

        public static T Get<T>(string id, Func<T> createInstance)
            where T : class, new()
        {
            var key = typeof(T).AssemblyQualifiedName + "&" + id;

            lock (thisLock)
            {
                WeakReference<object> weakReference;
                if (_dictionary.TryGetValue(key, out weakReference))
                {
                    object target;
                    if (!weakReference.TryGetTarget(out target))
                    {
                        target = createInstance();
                        weakReference.SetTarget(target);
                    }
                    return (T)target;
                }
                else
                {
                    var instance = createInstance();
                    weakReference = new WeakReference<object>(instance);
                    _dictionary.Add(key, weakReference);
                    return instance;
                }
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

        public static T Get<T>(string id)
            where T : class, new()
        {
            return Get<T>(id, () => Activator.CreateInstance<T>());
        }

        public static T Get<T>(int id, Func<T> createInstance)
            where T : class, new()
        {
            return Get<T>(id.ToString(), createInstance);
        }

        public static T Get<T>(long id, Func<T> createInstance)
            where T : class, new()
        {
            return Get<T>(id.ToString(), createInstance);
        }
    }
}
