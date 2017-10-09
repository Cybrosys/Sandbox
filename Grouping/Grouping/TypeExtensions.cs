using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grouping
{
    public static class TypeExtensions
    {
        public static PropertyInfo GetPropertyOrDefault(this Type instance, string name, PropertyInfo @default = null)
        {
            if (instance == null) return @default;
            return instance.GetProperty(name);
        }
    }
}
