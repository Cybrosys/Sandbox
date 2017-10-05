using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        }
    }
}
