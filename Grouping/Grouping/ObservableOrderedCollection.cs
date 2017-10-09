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

        public ObservableOrderedCollection(Comparer<T> comparer)
        {
            _comparer = comparer;
        }

        public ObservableOrderedCollection(string orderBy)
        {
            _comparer = new PropertyComparer<T>(orderBy);
        }

        public ObservableOrderedCollection(List<T> list) : base(list.OrderBy(item => item, Comparer<T>.Default).ToList())
        {
            _comparer = Comparer<T>.Default;
        }

        public ObservableOrderedCollection(IEnumerable<T> collection) : base(collection.OrderBy(item => item, Comparer<T>.Default))
        {
            _comparer = Comparer<T>.Default;
        }

        public ObservableOrderedCollection(List<T> list, Comparer<T> comparer) : base(list.OrderBy(item => item, comparer).ToList())
        {
            _comparer = comparer;
        }

        public ObservableOrderedCollection(IEnumerable<T> collection, Comparer<T> comparer) : base(collection.OrderBy(item => item, comparer))
        {
            _comparer = comparer;
        }

        public ObservableOrderedCollection(List<T> list, string orderBy) : base(list.OrderBy(item => item, new PropertyComparer<T>(orderBy)).ToList())
        {
            _comparer = new PropertyComparer<T>(orderBy);
        }

        public ObservableOrderedCollection(IEnumerable<T> collection, string orderBy) : base(collection.OrderBy(item => item, new PropertyComparer<T>(orderBy)))
        {
            _comparer = new PropertyComparer<T>(orderBy);
        }

        protected override void InsertItem(int index, T item)
        {
            // Since this method is used by the normal Add method, we'll help it find a valid index.
            if (!IsValidIndexForItem(index, item))
                index = GetValidIndexForItem(item);
            base.InsertItem(index, item);
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex) return;
            var item = this[oldIndex];
            if (IsValidIndexForItem(newIndex, item))
                base.MoveItem(oldIndex, newIndex);
        }

        protected override void SetItem(int index, T item)
        {
            // Check and store if the current index is a valid index for the item
            var isValidIndex = IsValidIndexForItem(index, item);
            if (isValidIndex)
            {
                // Replace the item.
                base.SetItem(index, item);
                return;
            }

            // It wasn't a valid index, move the item to a valid index.
            var newIndex = GetValidIndexForItem(item);
            base.Move(index, newIndex);
        }

        private bool IsValidIndexForItem(int index, T item)
        {
            // No other items so return true.
            if (Count == 0) return true;

            T itemAboveIndex;
            T itemAtIndex;

            if (index > 0 && index < Count)
            {
                // Compare against both
                itemAboveIndex = this[index - 1];
                itemAtIndex = this[index];

                // If itemAboveIndex is greater than item or itemAtIndex is less than item, return false.
                if (_comparer.Compare(itemAboveIndex, item) > 0 ||
                    _comparer.Compare(itemAtIndex, item) < 0)
                    return false;
                return true;
            }

            if (index == 0 && index < Count)
            {
                // Compare against itemAtIndex
                itemAtIndex = this[index];

                // If itemAtIndex is greater than or equal to item, return true.
                return _comparer.Compare(itemAtIndex, item) > 0;
            }

            if (index > 0 && index == Count)
            {
                // Compare against itemAboveIndex
                itemAboveIndex = this[index - 1];

                // If itemAboveIndex is less than or equal to item, return true;
                return _comparer.Compare(itemAboveIndex, item) < 1;
            }

            // Should never reach this part.
            return false;
        }

        private int GetValidIndexForItem(T item)
        {
            for (int i = 0; i < Count; ++i)
            {
                // If stored item at index is greater than the passed in item.
                if (_comparer.Compare(this[i], item) > 0)
                    return i;
            }

            // Insert last because all other items are less or equal.
            return Count;
        }
    }
}
