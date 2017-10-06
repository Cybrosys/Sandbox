using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grouping
{
    class Program
    {
        static void Main(string[] args)
        {


            




            /*
ObservableGroupingCollection
	Normal collection changed events are because of the groups, their sort order is based
	on their keys.
	Additional events are to notify about when items are added/removed from/to groups,
	and moved between groups.
            */

            #region Value type sorting
            // Value type sorting
            var numbers = new List<int>(10);
            var random = new Random();
            for (int i = 0; i < 10; ++i)
                numbers.Add(random.Next() % 10);
            numbers.ForEach(Console.WriteLine);

            Console.WriteLine();
            var orderedNumbers = new ObservableOrderedCollection<int>(numbers);
            foreach (var item in orderedNumbers)
                Console.WriteLine(item);

            Console.WriteLine();
            orderedNumbers = new ObservableOrderedCollection<int>();
            numbers.ForEach(orderedNumbers.Add);
            foreach (var item in orderedNumbers)
                Console.WriteLine(item);

            Console.WriteLine();
            orderedNumbers[9] = 0;
            foreach (var item in orderedNumbers)
                Console.WriteLine(item);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            #endregion

            #region Complex object sorting
            var people = new List<Person>(10);
            var names = new string[]
            {
                "Ann",
                "Anna",
                "Bob",
                "Billy",
                "Christian",
                "Christopher",
                "Dan",
                "Daniel",
                "Fred",
                "Fredrik"
            };
            for (int i = 0; i < 10; ++i)
            {
                people.Add(new Person
                {
                    Name = names[random.Next() % names.Length],
                    Age = random.Next(10, 61)
                });
            }
            people.ForEach(Console.WriteLine);
            Console.WriteLine();

            var ageOrderedPeople = new ObservableOrderedCollection<Person>(nameof(Person.Age));
            people.ForEach(ageOrderedPeople.Add);
            Console.WriteLine("Age ordered people (Add only)");
            foreach (var item in ageOrderedPeople)
                Console.WriteLine(item);

            //Console.WriteLine();
            //ageOrderedPeople = new ObservableOrderedCollection<Person>(people, nameof(Person.Age));
            //Console.WriteLine("Age ordered people (orderBy constructor");
            //foreach (var item in ageOrderedPeople)
            //    Console.WriteLine(item);
            #endregion

        }
    }

    public class PropertyComparer<T> : Comparer<T>
    {
        private readonly Func<T, T, int> _compare;

        public PropertyComparer(string propertyName)
        {
            CreateCompare(ref _compare, propertyName);
        }

        public override int Compare(T x, T y) => _compare(x, y);

        private static Func<T, T, int> CreateCompare(ref Func<T, T, int> func, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            var type = property.PropertyType;

            if (type == typeof(string))
                func = (x, y) => StringComparer.CurrentCulture.Compare((string)property.GetValue(x), (string)property.GetValue(y));
            else if (type == typeof(char))
                func = _CC<char>(property);
            else if (type == typeof(bool))
                func = _CC<bool>(property);
            else if (type == typeof(sbyte))
                func = _CC<sbyte>(property);
            else if (type == typeof(byte))
                func = _CC<byte>(property);
            else if (type == typeof(short))
                func = _CC<short>(property);
            else if (type == typeof(ushort))
                func = _CC<ushort>(property);
            else if (type == typeof(int))
                func = _CC<int>(property);
            else if (type == typeof(uint))
                func = _CC<uint>(property);
            else if (type == typeof(long))
                func = _CC<long>(property);
            else if (type == typeof(ulong))
                func = _CC<ulong>(property);
            else if (type == typeof(float))
                func = _CC<float>(property);
            else if (type == typeof(double))
                func = _CC<double>(property);
            else if (type == typeof(decimal))
                func = _CC<decimal>(property);
            else if (type == typeof(DateTime))
                func = _CC<DateTime>(property);
            else if (type == typeof(TimeSpan))
                func = _CC<TimeSpan>(property);

            return func;
        }

        private static Func<T, T, int> _CC<TPropertyType>(PropertyInfo propertyInfo) => (x, y) => Comparer<TPropertyType>.Default.Compare((TPropertyType)propertyInfo.GetValue(x), (TPropertyType)propertyInfo.GetValue(y));
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString() => $"{Name} - {Age}";
    }
}
