using System;

namespace Grouping.Test.Models
{
    public class Item : ObservableObject
    {
        private int _index;
        private string _name;
        private DateTime _timestamp;

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { SetProperty(ref _timestamp, value); }
        }
    }
}
