using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grouping
{
    public class NotifyGroupCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
    {
        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action) : base(action)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem) : base(action, changedItem)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index) : base(action, changedItem, index)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems) : base(action, changedItems)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex) : base(action, changedItems, startingIndex)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem) : base(action, newItem, oldItem)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index) : base(action, newItem, oldItem, index)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems) : base(action, newItems, oldItems)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex)
        {
        }

        public NotifyGroupCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex)
        {
        }

        public int NewGroupIndex { get; set; } = -1;
        public int OldGroupIndex { get; set; } = -1;
        public int NewGroupItemIndex { get; set; } = -1;
        public int OldGroupItemIndex { get; set; } = -1;
    }
}
