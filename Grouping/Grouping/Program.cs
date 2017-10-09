using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            var data = new List<Car>
            {
                new Car { Name = "Audi", Type = "Sedan" },
                new Car { Name = "Audi", Type = "Combi" },
                new Car { Name = "BWM", Type = "Sedan" },
                new Car { Name = "BWM", Type = "Combi" },
                new Car { Name = "Volvo", Type = "Combi" },
                new Car { Name = "Skoda", Type = "Combi" },
                new Car { Name = "Lamborghini", Type = "Super car" },
                new Car { Name = "Ferrari", Type = "Super car" },
            };

            var cars = new ObservableOrderedGroupingCollection<Car>(data, nameof(Car.Type), nameof(Car.Name));

            cars.CollectionGroupingChanged += (s, e) =>
            {
                Console.WriteLine($"Group {e.Action}");
            };
            cars.CollectionChanged += (s, e) =>
            {
                Console.WriteLine($"Item {e.Action}");
            };

            cars.GroupBy = nameof(Car.Name);
        }
    }

    public class Car
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
