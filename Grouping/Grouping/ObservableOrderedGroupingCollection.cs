﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grouping
{
    public class ObservableOrderedGroupingCollection<T> : ObservableCollection<T>
    {
        public class Group<TKey> : ObservableOrderedCollection<T>
        {
            private readonly TKey _key;

            public Group(TKey key)
            {
                _key = key;
            }

            public Group(TKey key, Comparer<T> comparer) : base(comparer)
            {
                _key = key;
            }

            public Group(TKey key, string orderBy) : base(orderBy)
            {
                _key = key;
            }

            public Group(TKey key, List<T> list) : base(list)
            {
                _key = key;
            }

            public Group(TKey key, IEnumerable<T> collection) : base(collection)
            {
                _key = key;
            }

            public Group(TKey key, List<T> list, Comparer<T> comparer) : base(list, comparer)
            {
                _key = key;
            }

            public Group(TKey key, IEnumerable<T> collection, Comparer<T> comparer) : base(collection, comparer)
            {
                _key = key;
            }

            public Group(TKey key, List<T> list, string orderBy) : base(list, orderBy)
            {
                _key = key;
            }

            public Group(TKey key, IEnumerable<T> collection, string orderBy) : base(collection, orderBy)
            {
                _key = key;
            }

            public TKey Key => _key;
        }

        private string _groupBy;
        private string _orderBy;
        private PropertyInfo _groupByProperty;
        private PropertyInfo _orderByProperty;
        private ObservableOrderedCollection<Group<string>> _groups;

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
        public ObservableOrderedCollection<Group<string>> Groups => _groups;

        public ObservableOrderedGroupingCollection() : base()
        {
        }
        public ObservableOrderedGroupingCollection(List<T> list) : base(list)
        {
        }
        public ObservableOrderedGroupingCollection(IEnumerable<T> collection) :base(collection)
        {
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
        }
        public ObservableOrderedGroupingCollection(IEnumerable<T> collection, string groupBy, string orderBy) : base(collection)
        {
            _groupBy = groupBy;
            _groupByProperty = GetProperty(_groupBy);
            _orderBy = orderBy;
            _orderByProperty = GetProperty(_orderBy);
            CreateGroups(false);
        }

        public event NotifyCollectionChangedEventHandler CollectionGroupingChanged;

        protected virtual void OnCollectionGroupingChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionGroupingChanged?.Invoke(this, e);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_groupBy) || string.IsNullOrWhiteSpace(_orderBy) || _groups == null || _groups.Count == 0)
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
                        // Clear the groups and pass it on.
                        _groups.Clear();
                        base.OnCollectionChanged(e);
                        // I won't be doing a group notification since we have the original args.
                        //OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }
                    break;
            }
        }

        private void CreateGroups(bool notifyCollectionChanged = true)
        {
            if (_groups != null)
            {
                _groups.Clear();
                OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            if (string.IsNullOrWhiteSpace(_groupBy) || string.IsNullOrWhiteSpace(_orderBy)) return;
            if (Count == 0) return;

            _groups = new ObservableOrderedCollection<Group<string>>(nameof(Group<string>.Key));

            var result = this.GroupBy(item => _groupByProperty.GetValue(item).ToString());
            foreach (var item in result)
            {
                var group = new Group<string>(item.Key, item, _orderBy);
                _groups.Add(group);
                OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, group, _groups.IndexOf(group)));
            }
        }

        private Group<string> GetOrCreateGroupForItem(T item)
        {
            var groupKey = _groupByProperty.GetValue(item).ToString();
            var group = _groups.FirstOrDefault(g => g.Key == groupKey);
            if (group == null)
            {
                group = new Group<string>(groupKey);
                _groups.Add(group);
                OnCollectionGroupingChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, group, _groups.IndexOf(group)));
            }
            return group;
        }

        private Group<string> GetGroupForItem(T item)
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
    }
}
