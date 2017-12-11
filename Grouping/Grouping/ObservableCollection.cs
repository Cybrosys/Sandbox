using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grouping
{
    public interface INotifyItemPropertyChanged
    {
        event PropertyChangedEventHandler ItemPropertyChanged;
    }

    public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>, INotifyItemPropertyChanged, IDisposable
    {
        public ObservableCollection() : base() { }
        public ObservableCollection(List<T> list) : base(list) { SubscribeToItems(list); }
        public ObservableCollection(IEnumerable<T> collection) : base(collection) { SubscribeToItems(collection); }

        public event PropertyChangedEventHandler ItemPropertyChanged;

        protected virtual void SubscribeToItems(IEnumerable<T> items)
        {
            if (items == null) return;
            foreach (var item in items)
                SubscribeToItem(item);
        }

        protected virtual void UnsubscribeFromItems(IEnumerable<T> items)
        {
            if (items == null) return;
            foreach (var item in items)
                UnsubscribeFromItem(item);
        }

        protected virtual void SubscribeToItem(T item) => SubscribeToItem(item as INotifyPropertyChanged);

        protected virtual void SubscribeToItem(INotifyPropertyChanged item)
        {
            if (item != null)
                item.PropertyChanged += OnItemPropertyChanged;
        }

        protected virtual void UnsubscribeFromItem(T item) => UnsubscribeFromItem(item as INotifyPropertyChanged);

        protected virtual void UnsubscribeFromItem(INotifyPropertyChanged item)
        {
            if (item != null)
                item.PropertyChanged -= OnItemPropertyChanged;
        }

        protected virtual void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e) => ItemPropertyChanged?.Invoke(sender, e);

        protected override void ClearItems()
        {
            UnsubscribeFromItems(this);
            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            SubscribeToItem(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            UnsubscribeFromItem(this[index]);
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            UnsubscribeFromItem(this[index]);
            SubscribeToItem(item);
            base.SetItem(index, item);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ClearItems();
                }

                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
