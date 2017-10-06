using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Grouping
{
    public class PropertyComparer<T> : Comparer<T>
    {
        private readonly Func<T, T, int> _compare;

        public PropertyComparer(string propertyName)
        {
            CreateCompareFunc(ref _compare, propertyName);
        }

        public override int Compare(T x, T y) => _compare(x, y);

        private static Func<T, T, int> CreateCompareFunc(ref Func<T, T, int> compare, string propertyName)
        {
            if (compare != null) return compare;
            var typeOfT = typeof(T);
            var property = typeOfT.GetProperty(propertyName);
            if (property == null) throw new ArgumentException($"'{typeOfT.Name}' does not contain a property with the name '{propertyName}'");
            compare = GetCompareFuncForProperty(property);
            return compare;
        }

        private static Func<T, T, int> GetCompareFuncForProperty(PropertyInfo property)
        {
            var type = property.PropertyType;
            if (type == typeof(string))
                return (x, y) => StringComparer.CurrentCulture.Compare((string)property.GetValue(x), (string)property.GetValue(y));
            else if (type == typeof(char))
                return CreateCompareFuncForProperty<char>(property);
            else if (type == typeof(bool))
                return CreateCompareFuncForProperty<bool>(property);
            else if (type == typeof(sbyte))
                return CreateCompareFuncForProperty<sbyte>(property);
            else if (type == typeof(byte))
                return CreateCompareFuncForProperty<byte>(property);
            else if (type == typeof(short))
                return CreateCompareFuncForProperty<short>(property);
            else if (type == typeof(ushort))
                return CreateCompareFuncForProperty<ushort>(property);
            else if (type == typeof(int))
                return CreateCompareFuncForProperty<int>(property);
            else if (type == typeof(uint))
                return CreateCompareFuncForProperty<uint>(property);
            else if (type == typeof(long))
                return CreateCompareFuncForProperty<long>(property);
            else if (type == typeof(ulong))
                return CreateCompareFuncForProperty<ulong>(property);
            else if (type == typeof(float))
                return CreateCompareFuncForProperty<float>(property);
            else if (type == typeof(double))
                return CreateCompareFuncForProperty<double>(property);
            else if (type == typeof(decimal))
                return CreateCompareFuncForProperty<decimal>(property);
            else if (type == typeof(DateTime))
                return CreateCompareFuncForProperty<DateTime>(property);
            else if (type == typeof(TimeSpan))
                return CreateCompareFuncForProperty<TimeSpan>(property);
            else
            {
                // Check if IComparable<>
                var genericIComparableInterface = type.GetInterface(typeof(IComparable<>).FullName);
                if (genericIComparableInterface != null)
                {
                    var compareToMethod = genericIComparableInterface.GetMethod(nameof(IComparable.CompareTo));
                    return (x, y) =>
                    {
                        var xValue = property.GetValue(x);
                        var yValue = property.GetValue(y);
                        // xValue.CompareTo(yValue)
                        return (int)compareToMethod.Invoke(xValue, new object[] { yValue });
                    };
                }
                // Default fallback
                return (x, y) => Comparer.Default.Compare(x, y);
            }
        }

        private static Func<T, T, int> CreateCompareFuncForProperty<TPropertyType>(PropertyInfo propertyInfo) => (x, y) => Comparer<TPropertyType>.Default.Compare((TPropertyType)propertyInfo.GetValue(x), (TPropertyType)propertyInfo.GetValue(y));
    }
}
