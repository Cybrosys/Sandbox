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
            var item1 = new SuperObjectWithAdvancedIndex(3);
            var item2 = new SuperObjectWithAdvancedIndex(0);
            var item3 = new SuperObjectWithAdvancedIndex(2);

            var orderedSuperObjects = new ObservableOrderedCollection<SuperObjectWithAdvancedIndex>(nameof(SuperObjectWithAdvancedIndex.Index));
            orderedSuperObjects.Add(item1);
            orderedSuperObjects.Add(item2);
            orderedSuperObjects.Add(item3);

            foreach (var item in orderedSuperObjects)
                Console.WriteLine(item.Index.Index);
            return;

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

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString() => $"{Name} - {Age}";
    }

    class SuperObjectWithAdvancedIndex
    {
        public AdvancedIndex Index { get; private set; }

        public SuperObjectWithAdvancedIndex(int index)
        {
            Index = new AdvancedIndex { Index = index };
        }
    }

    class AdvancedIndex : IComparable<AdvancedIndex>
    {
        public int Index { get; set; }
        
        public int CompareTo(AdvancedIndex other)
        {
            return Comparer<int>.Default.Compare(Index, other.Index);
        }
    }
}
