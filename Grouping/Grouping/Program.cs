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

            Expression<Func<DateTime, int>> x = item => item.Minute;
            var func = x.Compile();
            Console.WriteLine(func(DateTime.Now));

            var collection = new ObservableOrderedCollection<DateTime, int>(new DateTime[] { }, item => item.Minute);
            
        }
    }
}
