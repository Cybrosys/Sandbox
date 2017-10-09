using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grouping.Test.Ordering
{
    public abstract class DefaultImplTests<T> : Tests<T>
    {
        protected string _orderBy;

        [TestMethod]
        public override void Constructor_as_list()
        {
            // Arrange
            // Act
            var result = string.IsNullOrWhiteSpace(_orderBy) ? new ObservableOrderedCollection<T>(_data) : new ObservableOrderedCollection<T>(_data, _orderBy);

            // Assert
            AssertEx.AreEqual(_sortedData, result, _orderBy);
        }

        [TestMethod]
        public override void Constructor_as_enumerable()
        {
            // Arrange
            // Act
            var result = string.IsNullOrWhiteSpace(_orderBy) ? new ObservableOrderedCollection<T>(_data as IEnumerable<T>) : new ObservableOrderedCollection<T>(_data as IEnumerable<T>, _orderBy);

            // Assert

            AssertEx.AreEqual(_sortedData, result, _orderBy);
        }

        [TestMethod]
        public override void Add()
        {
            // Arrange
            // Act
            var result = string.IsNullOrWhiteSpace(_orderBy) ? new ObservableOrderedCollection<T>() : new ObservableOrderedCollection<T>(_orderBy);
            for (int i = 0; i < _data.Count; ++i)
                result.Add(_data[i]);

            // Assert
            AssertEx.AreEqual(_sortedData, result, _orderBy);
        }

        [TestMethod]
        public override void Insert()
        {
            // Arrange
            // Act
            var result = string.IsNullOrWhiteSpace(_orderBy) ? new ObservableOrderedCollection<T>() : new ObservableOrderedCollection<T>(_orderBy);
            result.Insert(0, _data[0]);
            for (int i = 1; i < _data.Count; ++i)
            {
                var index = _random.Next() % result.Count;
                result.Insert(index, _data[i]);
            }

            // Assert
            AssertEx.AreEqual(_sortedData, result, _orderBy);
        }

        [TestMethod]
        public override void Move()
        {
            // Arrange
            var result = string.IsNullOrWhiteSpace(_orderBy) ? new ObservableOrderedCollection<T>() : new ObservableOrderedCollection<T>(_orderBy);
            
            // Act
            for (int i = 0; i < result.Count / 2; ++i)
            {
                var randomFromIndex = _random.Next() % result.Count;
                var randomToIndex = _random.Next() % result.Count;
                result.Move(randomFromIndex, randomToIndex);
            }
            var sortedResult = new List<T>(result);
            sortedResult.Sort();

            // Assert
            AssertEx.AreEqual(sortedResult, result, _orderBy);
        }

        [TestMethod]
        public override void Remove()
        {
            // Arrange
            var result = string.IsNullOrWhiteSpace(_orderBy) ? new ObservableOrderedCollection<T>(_data) : new ObservableOrderedCollection<T>(_data, _orderBy);
            var sortedResult = new List<T>(_sortedData);

            // Act
            for (int i = 0; i < result.Count / 2; ++i)
            {
                var randomIndex = _random.Next() % result.Count;
                result.RemoveAt(i);
                sortedResult.RemoveAt(i);
            }

            // Assert
            AssertEx.AreEqual(sortedResult, result, _orderBy);
        }

        [TestMethod]
        public override void Set()
        {
            // Arrange
            var result = string.IsNullOrWhiteSpace(_orderBy) ? new ObservableOrderedCollection<T>(_data) : new ObservableOrderedCollection<T>(_data, _orderBy);

            // Act
            for (int i = 0; i < result.Count / 2; ++i)
            {
                var randomIndex = _random.Next() % result.Count;
                var randomValue = _data[_random.Next() % _data.Count];
                result[randomIndex] = randomValue;
            }
            var sortedResult = string.IsNullOrWhiteSpace(_orderBy) ? new ObservableOrderedCollection<T>(result) : new ObservableOrderedCollection<T>(result, _orderBy);

            // Assert
            AssertEx.AreEqual(sortedResult, result, _orderBy);
        }
    }
}
