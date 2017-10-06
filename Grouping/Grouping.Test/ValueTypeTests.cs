using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Grouping.Test
{
    [TestClass]
    public class ValueTypeTests
    {
        private Random _random = new Random();
        private List<int> _data;
        private List<int> _sortedData;

        public ValueTypeTests()
        {
            _data = new List<int>(100);
            for (int i = 0; i < 100; ++i)
                _data.Add(_random.Next(i) % 100);
            _sortedData = new List<int>(_data);
            _sortedData.Sort();
        }

        [TestMethod]
        public void Integers_passed_to_constructor_as_list()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<int>(_data);

            // Assert
            
            AssertEx.AreEqual(_sortedData, result);
        }

        [TestMethod]
        public void Integers_passed_to_constructor_as_enumerable()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<int>(_data as IEnumerable<int>);

            // Assert
            AssertEx.AreEqual(_sortedData, result);
        }

        [TestMethod]
        public void Integers_passed_to_add()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<int>();
            for (int i = 0; i < _data.Count; ++i)
                result.Add(_data[i]);

            // Assert
            AssertEx.AreEqual(_sortedData, result);
        }
    }
}
