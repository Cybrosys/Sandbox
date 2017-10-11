using System.Collections.Generic;

namespace Grouping
{
    public class ObservableOrderedGroupCollection<TKey, TItem> : ObservableOrderedCollection<TItem>
    {
        private readonly TKey _key;

        public ObservableOrderedGroupCollection(TKey key)
        {
            _key = key;
        }

        public ObservableOrderedGroupCollection(TKey key, Comparer<TItem> comparer) : base(comparer)
        {
            _key = key;
        }

        public ObservableOrderedGroupCollection(TKey key, string orderBy) : base(orderBy)
        {
            _key = key;
        }

        public ObservableOrderedGroupCollection(TKey key, List<TItem> list) : base(list)
        {
            _key = key;
        }

        public ObservableOrderedGroupCollection(TKey key, IEnumerable<TItem> collection) : base(collection)
        {
            _key = key;
        }

        public ObservableOrderedGroupCollection(TKey key, List<TItem> list, Comparer<TItem> comparer) : base(list, comparer)
        {
            _key = key;
        }

        public ObservableOrderedGroupCollection(TKey key, IEnumerable<TItem> collection, Comparer<TItem> comparer) : base(collection, comparer)
        {
            _key = key;
        }

        public ObservableOrderedGroupCollection(TKey key, List<TItem> list, string orderBy) : base(list, orderBy)
        {
            _key = key;
        }

        public ObservableOrderedGroupCollection(TKey key, IEnumerable<TItem> collection, string orderBy) : base(collection, orderBy)
        {
            _key = key;
        }

        public TKey Key => _key;
    }
}
