using Grouping.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grouping.Test.Grouping
{
    [TestClass]
    public class ItemTests
    {
        private Random _random = new Random();
        private List<Item> _data;

        public ItemTests()
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
            // Act
            var result = new ObservableOrderedGroupingCollection<Item>(_data, nameof(Item.Name), nameof(Item.Index));

            // Assert
            Assert.AreNotEqual(0, result.Groups.Count);

            var groupKeys = result.Groups.Select(g => g.Key).ToList();
            var sortedGroupKeys = new List<string>(groupKeys);
            sortedGroupKeys.Sort();
            AssertEx.AreEqual(sortedGroupKeys, result.Groups.Select(g => g.Key).ToList());

            foreach (var group in result.Groups)
            {
                var sortedItems = group.OrderBy(g => g.Index).ToList();
                AssertEx.AreEqual(sortedItems, group, nameof(Item.Index));
            }
        }

        [TestMethod]
        public void Constructor_as_enumerable()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedGroupingCollection<Item>(_data as IEnumerable<Item>, nameof(Item.Name), nameof(Item.Index));

            // Assert
            Assert.AreNotEqual(0, result.Groups.Count);

            var groupKeys = result.Groups.Select(g => g.Key).ToList();
            var sortedGroupKeys = new List<string>(groupKeys);
            sortedGroupKeys.Sort();
            AssertEx.AreEqual(sortedGroupKeys, result.Groups.Select(g => g.Key).ToList());

            foreach (var group in result.Groups)
            {
                var sortedItems = group.OrderBy(g => g.Index).ToList();
                AssertEx.AreEqual(sortedItems, group, nameof(Item.Index));
            }
        }

        [TestMethod]
        public void Add()
        {
            // Arrange
            var result = new ObservableOrderedGroupingCollection<Item>(nameof(Item.Name), nameof(Item.Index));

            // Act
            foreach (var item in _data)
                result.Add(item);

            // Assert
            Assert.AreNotEqual(0, result.Groups.Count);

            var groupKeys = result.Groups.Select(g => g.Key).ToList();
            var sortedGroupKeys = new List<string>(groupKeys);
            sortedGroupKeys.Sort();
            AssertEx.AreEqual(sortedGroupKeys, result.Groups.Select(g => g.Key).ToList());

            foreach (var group in result.Groups)
            {
                var sortedItems = group.OrderBy(g => g.Index).ToList();
                AssertEx.AreEqual(sortedItems, group, nameof(Item.Index));
            }
        }

        [TestMethod]
        public void Change_items_group_by_property_value()
        {
            // Arrange
            var result = new ObservableOrderedGroupingCollection<Item>(nameof(Item.Name), nameof(Item.Index));
            for (int i = 0; i < 25; ++i)
            {
                result.Add(new Item
                {
                    Index = i,
                    Name = "0",
                    Timestamp = new DateTime(1900, 1, 1),
                });
            }

            // Act
            for (int i = 0; i < 25; ++i)
            {
                var item = result[i];
                item.Name = i.ToString();
            }

            // Assert
            Assert.AreEqual(25, result.Groups.Count);
            Assert.IsTrue(result.Groups.All(g => g.Count == 1));
        }
    }
}
