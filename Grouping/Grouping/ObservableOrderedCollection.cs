using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grouping
{
    public class ObservableOrderedCollection<T> : ObservableCollection<T>
    {
        private readonly Comparer<T> _comparer;

        public ObservableOrderedCollection()
        {
            _comparer = Comparer<T>.Default;
        }

        public ObservableOrderedCollection(List<T> list) : base(list.OrderBy(item => item).ToList())
        {
            _comparer = Comparer<T>.Default;
        }

        public ObservableOrderedCollection(IEnumerable<T> collection) : base(collection.OrderBy(item => item))
        {
            _comparer = Comparer<T>.Default;
        }

        protected override void InsertItem(int index, T item)
        {
            // Find index to insert on
            for (int i = 0; i < Count; ++i)
            {
                if (_comparer.Compare(this[i], item) != -1) // If stored value at index is greater or equal
                {
                    base.InsertItem(i, item);
                    return;
                }
            }

            // Insert last, all items are less
            base.InsertItem(Count, item);
        }
    }
}
