using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Grouping
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (ReferenceEquals(property, value) || EqualityComparer<T>.Default.Equals(property, value))
                return false;
            property = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
