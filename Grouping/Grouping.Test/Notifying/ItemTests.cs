using Grouping.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Grouping.Test.Notifying
{
    [TestClass]
    public class ItemTests
    {
        class SubscriptionCountingObservableCollection : ObservableCollection<Item>
        {
            public int SubscriptionCount { get; set; }

            public SubscriptionCountingObservableCollection()
            {
            }

            public SubscriptionCountingObservableCollection(IEnumerable<Item> collection) : base(collection)
            {
            }

            public SubscriptionCountingObservableCollection(List<Item> list) : base(list)
            {
            }

            protected override void SubscribeToItem(INotifyPropertyChanged item)
            {
                base.SubscribeToItem(item);

                if (item != null)
                    ++SubscriptionCount;
            }

            protected override void UnsubscribeFromItem(INotifyPropertyChanged item)
            {
                base.UnsubscribeFromItem(item);

                if (item != null)
                    --SubscriptionCount;
            }
        }

        private Random _random = new Random();
        private List<Item> _data;

        [TestInitialize]
        public void Init()
        {
            _data = new List<Item>(25);
            for (int i = 0; i < 25; ++i)
            {
                _data.Add(new Item
                {
                    Index = _random.Next(),
                    Name = (i % 5).ToString(),
                    Timestamp = new DateTime(_random.Next(2000, 2018), _random.Next(1, 13), _random.Next(1, 28))
                });
            }
        }

        [TestMethod]
        public void Constructor_as_list()
        {
            // Arrange
            var result = new SubscriptionCountingObservableCollection(_data);
            var itemPropertyChangedCount = 0;
            result.ItemPropertyChanged += (s, e) => ++itemPropertyChangedCount;

            // Act
            foreach (var item in result)
                item.Index = -1;

            // Assert
            Assert.AreEqual(result.SubscriptionCount, result.Count);
            Assert.AreEqual(result.SubscriptionCount, itemPropertyChangedCount);
        }

        [TestMethod]
        public void Constructor_as_enumerable()
        {
            // Arrange
            var result = new SubscriptionCountingObservableCollection(_data as IEnumerable<Item>);
            var itemPropertyChangedCount = 0;
            result.ItemPropertyChanged += (s, e) => ++itemPropertyChangedCount;

            // Act
            foreach (var item in result)
                item.Index = -1;

            // Assert
            Assert.AreEqual(result.SubscriptionCount, result.Count);
            Assert.AreEqual(result.SubscriptionCount, itemPropertyChangedCount);
        }

        [TestMethod]
        public void Add()
        {
            // Arrange
            var result = new SubscriptionCountingObservableCollection();
            var itemPropertyChangedCount = 0;
            result.ItemPropertyChanged += (s, e) => ++itemPropertyChangedCount;

            // Act
            foreach (var item in _data)
            {
                result.Add(item);
                item.Index = -1;
            }

            // Assert
            Assert.AreEqual(result.SubscriptionCount, result.Count);
            Assert.AreEqual(result.SubscriptionCount, itemPropertyChangedCount);
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            var result = new SubscriptionCountingObservableCollection(_data);
            var itemPropertyChangedCount = 0;
            result.ItemPropertyChanged += (s, e) => ++itemPropertyChangedCount;

            // Act
            while (result.Count > 0)
                result.RemoveAt(0);

            foreach (var item in _data)
                item.Index = -1;

            // Assert
            Assert.AreEqual(result.SubscriptionCount, result.Count);
            Assert.AreEqual(result.SubscriptionCount, itemPropertyChangedCount);
        }

        [TestMethod]
        public void Set()
        {
            // Arrange
            var result = new SubscriptionCountingObservableCollection(_data);
            var itemPropertyChangedCount = 0;
            result.ItemPropertyChanged += (s, e) => ++itemPropertyChangedCount;

            // Act
            var newItem = new Item();
            result[0] = newItem;

            foreach (var item in _data)
                item.Index = -1;
            newItem.Index = -1;

            // Assert
            Assert.AreEqual(result.SubscriptionCount, result.Count);
            Assert.AreEqual(result.SubscriptionCount, itemPropertyChangedCount);
        }

        [TestMethod]
        public void Clear()
        {
            // Arrange
            var result = new SubscriptionCountingObservableCollection(_data);
            var itemPropertyChangedCount = 0;
            result.ItemPropertyChanged += (s, e) => ++itemPropertyChangedCount;

            // Act
            result.Clear();

            foreach (var item in _data)
                item.Index = -1;

            // Assert
            Assert.AreEqual(result.SubscriptionCount, result.Count);
            Assert.AreEqual(result.SubscriptionCount, itemPropertyChangedCount);
        }

        [TestMethod]
        public void Dispose()
        {
            // Arrange
            var result = new SubscriptionCountingObservableCollection(_data);
            var itemPropertyChangedCount = 0;
            result.ItemPropertyChanged += (s, e) => ++itemPropertyChangedCount;
            result.Dispose();

            // Act
            foreach (var item in _data)
                item.Timestamp = DateTime.Now;

            // Assert
            Assert.AreEqual(result.SubscriptionCount, result.Count);
            Assert.AreEqual(result.SubscriptionCount, itemPropertyChangedCount);
        }
    }
}
