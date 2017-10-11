using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Grouping
{
    public class ObservableOrderedGroupingCollection<T> : ObservableCollection<T>, IDisposable
    {
        private string _groupBy;
        private string _orderBy;
        private PropertyInfo _groupByProperty;
        private PropertyInfo _orderByProperty;
        private ObservableOrderedCollection<ObservableOrderedGroupCollection<string, T>> _groups;

        public string GroupBy
        {
            get { return _groupBy; }
            set
            {
                if (_groupBy == value) return;
                _groupBy = value;
                _groupByProperty = GetProperty(_groupBy);
                CreateGroups();
            }
        }
        public string OrderBy
        {
            get { return _orderBy; }
            set
            {
                if (_orderBy == value) return;
                _orderBy = value;
                _orderByProperty = GetProperty(_orderBy);
                CreateGroups();
            }
        }
        public ObservableOrderedCollection<ObservableOrderedGroupCollection<string, T>> Groups => _groups;

        public ObservableOrderedGroupingCollection() : base()
        {
        }
        public ObservableOrderedGroupingCollection(List<T> list) : base(list)
        {
            SubscribeToItems(list);
        }
        public ObservableOrderedGroupingCollection(IEnumerable<T> collection) :base(collection)
        {
            SubscribeToItems(collection);
        }
        public ObservableOrderedGroupingCollection(string groupBy, string orderBy)
        {
            _groupBy = groupBy;
            _groupByProperty = GetProperty(_groupBy);
            _orderBy = orderBy;
            _orderByProperty = GetProperty(_orderBy);
            CreateGroups(false);
        }
        public ObservableOrderedGroupingCollection(List<T> list, string groupBy, string orderBy) : base(list)
        {
            _groupBy = groupBy;
            _groupByProperty = GetProperty(_groupBy);
            _orderBy = orderBy;
            _orderByProperty = GetProperty(_orderBy);
            CreateGroups(false);
            SubscribeToItems(list);
        }
        public ObservableOrderedGroupingCollection(IEnumerable<T> collection, string groupBy, string orderBy) : base(collection)
        {
            _groupBy = groupBy;
            _groupByProperty = GetProperty(_groupBy);
            _orderBy = orderBy;
            _orderByProperty = GetProperty(_orderBy);
            CreateGroups(false);
            SubscribeToItems(collection);
        }

        public event NotifyCollectionChangedEventHandler CollectionGroupingChanged;

        protected virtual void OnCollectionGroupingChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionGroupingChanged?.Invoke(this, e);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_groupBy) || string.IsNullOrWhiteSpace(_orderBy) || _groups == null)
            {
                base.OnCollectionChanged(e);
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        // Get or create a group for item, add item to the group.
                        var item = (T)e.NewItems[0];
                        var group = GetOrCreateGroupForItem(item);
                        group.Add(item);

                        // Subscribe to item
                        SubscribeToItem(item);

                        // Get all the relevant indexes.
                        var indexOfGroup = _groups.IndexOf(group);
                        var indexOfItemInGroup = group.IndexOf(item);

                        // Create a new event args and append the extra data.
                        var args = CreateArgsFrom(e);
                        args.NewGroupIndex = indexOfGroup;
                        args.NewGroupItemIndex = indexOfItemInGroup;

                        // Pass it along.
                        base.OnCollectionChanged(args);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        // Find which group the item resides in.
                        var item = (T)e.OldItems[0];
                        var group = GetGroupForItem(item);

                        // Unsubscribe from item
                        UnsubscribeFromItem(item);

                        // Get relevant indexes.
                        var indexOfGroup = _groups.IndexOf(group);
                        var indexOfItemInGroup = group.IndexOf(item);

                        // Remove item from group.
                        group.Remove(item);

                        // Create a new event args and append the extra data.
                        var args = CreateArgsFrom(e);
                        args.OldGroupIndex = indexOfGroup;
                        args.OldGroupItemIndex = indexOfItemInGroup;

                        // Pass it along.
                        base.OnCollectionChanged(args);

                        // Check if the group is empty, and if so, remove it and call OnCollectionGroupingChanged.
                        if (group.Count == 0)
                        {
                            _groups.Remove(group);
                            OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, group, indexOfGroup));
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        // Remove old item...
                        var oldItem = (T)e.OldItems[0];
                        var group = GetGroupForItem(oldItem);

                        // Unsubscribe from item.
                        UnsubscribeFromItem(oldItem);

                        // Get indexes...
                        var indexOfGroup = _groups.IndexOf(group);
                        var indexOfItemInGroup = group.IndexOf(oldItem);

                        // Remove item from group.
                        group.Remove(oldItem);

                        // Create a remove action (removed from one group and added to another group).
                        var args = CreateArgsFrom(new NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, e.OldStartingIndex));
                        args.OldGroupIndex = indexOfGroup;
                        args.OldGroupItemIndex = indexOfItemInGroup;

                        // Pass it along.
                        base.OnCollectionChanged(args);

                        // Check if the group is empty, and if so, remove it and call OnCollectionGroupingChanged.
                        if (group.Count == 0)
                        {
                            _groups.Remove(group);
                            OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, group, indexOfGroup));
                        }
                        
                        // Create a add action.
                        var newItem = (T)e.NewItems[0];
                        group = GetOrCreateGroupForItem(newItem);
                        group.Add(newItem);

                        // Subscribe to item.
                        SubscribeToItem(newItem);

                        // Get indexes...
                        indexOfGroup = _groups.IndexOf(group);
                        indexOfItemInGroup = group.IndexOf(newItem);

                        // Create a new event args...
                        args = CreateArgsFrom(new NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, e.NewStartingIndex));
                        args.NewGroupIndex = indexOfGroup;
                        args.NewGroupItemIndex = indexOfItemInGroup;

                        // Pass it along.
                        base.OnCollectionChanged(args);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    // The placement of an item in the source list is of no interest when grouping and sorting, ignore it.
                    break;
                case NotifyCollectionChangedAction.Reset:
                    {   
                        // Item subscriptions has already been taken care of in the ClearItems override.
                        // Clear the groups and pass it on.
                        _groups?.Clear();
                        base.OnCollectionChanged(e);
                        // I won't be doing a group notification since we have the original args.
                        //OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }
                    break;
            }
        }

        protected override void ClearItems()
        {
            // Unsubscribe from items.
            UnsubscribeFromItems(this);

            // Continue as usual.
            base.ClearItems();
        }

        private void CreateGroups(bool notifyCollectionChanged = true)
        {
            if (_groups != null)
            {
                _groups.Clear();
                OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            if (string.IsNullOrWhiteSpace(_groupBy) || string.IsNullOrWhiteSpace(_orderBy)) return;
            _groups = new ObservableOrderedCollection<ObservableOrderedGroupCollection<string, T>>(nameof(ObservableOrderedGroupCollection<string, T>.Key));

            if (Count == 0) return;
            var result = this.GroupBy(item => _groupByProperty.GetValue(item).ToString());
            foreach (var item in result)
            {
                var group = new ObservableOrderedGroupCollection<string, T>(item.Key, item, _orderBy);
                _groups.Add(group);
                OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, group, _groups.IndexOf(group)));
            }
        }

        private void SubscribeToItems(IEnumerable<T> items)
        {
            if (items == null) return;
            foreach (var item in items)
                SubscribeToItem(item);
        }

        private void SubscribeToItem(T item) => SubscribeToItem(item as INotifyPropertyChanged);

        private void SubscribeToItem(INotifyPropertyChanged item)
        {
            if (item == null) return;
            item.PropertyChanged += Item_PropertyChanged;
        }

        private void UnsubscribeFromItems(IEnumerable<T> items)
        {
            if (items == null) return;
            foreach (var item in items)
                UnsubscribeFromItem(item);
        }

        private void UnsubscribeFromItem(T item) => UnsubscribeFromItem(item as INotifyPropertyChanged);

        private void UnsubscribeFromItem(INotifyPropertyChanged item)
        {
            if (item == null) return;
            item.PropertyChanged -= Item_PropertyChanged;
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_groupBy) || _groupBy != e.PropertyName) return;

            // Get the group containing the item (brute-force lookup since the key has changed) and which group it should be moved to.
            var item = (T)sender;
            var group = _groups.FirstOrDefault(g => g.Contains(item));
            if (group == null) return;

            // Get group for item.
            var newGroup = GetOrCreateGroupForItem(item);
            newGroup.Add(item);

            // Get relevant indexes.
            var sourceIndex = IndexOf(item);
            var indexOfGroup = _groups.IndexOf(group);
            var indexOfItemInGroup = group.IndexOf(item);
            var indexOfNewGroup = _groups.IndexOf(newGroup);
            var indexOfItemInNewGroup = newGroup.IndexOf(item);

            // Remove item from old group.
            group.Remove(item);

            // Create args...
            var args = CreateArgsFrom(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, sourceIndex, sourceIndex));
            args.NewGroupIndex = indexOfNewGroup;
            args.OldGroupIndex = indexOfGroup;
            args.NewGroupItemIndex = indexOfItemInNewGroup;
            args.OldGroupItemIndex = indexOfItemInGroup;

            // Pass it to base.
            base.OnCollectionChanged(args);

            // Check if the group is empty, and if so, remove it and call OnCollectionGroupingChanged.
            if (group.Count == 0)
            {
                _groups.Remove(group);
                OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, group, indexOfGroup));
            }
        }

        private ObservableOrderedGroupCollection<string, T> GetOrCreateGroupForItem(T item)
        {
            var groupKey = _groupByProperty.GetValue(item).ToString();
            var group = _groups.FirstOrDefault(g => g.Key == groupKey);
            if (group == null)
            {
                group = new ObservableOrderedGroupCollection<string, T>(groupKey, _orderBy);
                _groups.Add(group);
                OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, group, _groups.IndexOf(group)));
            }
            return group;
        }

        private ObservableOrderedGroupCollection<string, T> GetGroupForItem(T item)
        {
            var groupKey = _groupByProperty.GetValue(item).ToString();
            return _groups.FirstOrDefault(g => g.Key == groupKey);
        }

        private static NotifyGroupCollectionChangedEventArgs CreateArgsFrom(NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    return new NotifyGroupCollectionChangedEventArgs(args.Action, args.NewItems[0], args.NewStartingIndex);
                case NotifyCollectionChangedAction.Remove:
                    return new NotifyGroupCollectionChangedEventArgs(args.Action, args.OldItems[0], args.OldStartingIndex);
                case NotifyCollectionChangedAction.Replace:
                    return new NotifyGroupCollectionChangedEventArgs(args.Action, args.OldItems, args.NewItems, args.NewStartingIndex);
                case NotifyCollectionChangedAction.Move:
                    return new NotifyGroupCollectionChangedEventArgs(args.Action, args.OldItems, args.NewStartingIndex, args.OldStartingIndex);
                case NotifyCollectionChangedAction.Reset:
                default:
                    return new NotifyGroupCollectionChangedEventArgs(args.Action);
            }
        }

        private static PropertyInfo GetProperty(string propertyName) => typeof(T).GetPropertyOrDefault(propertyName);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UnsubscribeFromItems(this);
                    if (_groups != null)
                    {
                        foreach (var group in _groups)
                            UnsubscribeFromItems(group);
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
