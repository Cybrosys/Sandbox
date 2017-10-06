using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Grouping.Test
{
    [TestClass]
    public class ValueTypeTests
    {
        private Random _random = new Random();
        private List<int> _intData;
        private List<int> _sortedIntData;
        private List<byte> _byteData;
        private List<byte> _sortedByteData;

        public ValueTypeTests()
        {
            _intData = new List<int>(100);
            _byteData = new List<byte>(100);
            for (int i = 0; i < 100; ++i)
            {
                var number = (byte)(_random.Next() % 100);
                _intData.Add(number);
                _byteData.Add(number);
            }
            _sortedIntData = new List<int>(_intData);
            _sortedIntData.Sort();
            _sortedByteData = new List<byte>(_byteData);
            _sortedByteData.Sort();
        }

        [TestMethod]
        public void Integers_passed_to_constructor_as_list()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<int>(_intData);

            // Assert
            
            AssertEx.AreEqual(_sortedIntData, result);
        }

        [TestMethod]
        public void Integers_passed_to_constructor_as_enumerable()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<int>(_intData as IEnumerable<int>);

            // Assert
            AssertEx.AreEqual(_sortedIntData, result);
        }

        [TestMethod]
        public void Integers_passed_to_add()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<int>();
            for (int i = 0; i < _intData.Count; ++i)
                result.Add(_intData[i]);

            // Assert
            AssertEx.AreEqual(_sortedIntData, result);
        }

        [TestMethod]
        public void Integers_passed_to_insert()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<int>();
            result.Insert(0, _intData[0]);
            for (int i = 1; i < _intData.Count; ++i)
            {
                var index = _random.Next() % result.Count;
                result.Insert(index, _intData[i]);
            }

            // Assert
            AssertEx.AreEqual(_sortedIntData, result);
        }

        [TestMethod]
        public void Bytes_passed_to_constructor_as_list()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<byte>(_byteData);

            // Assert

            AssertEx.AreEqual(_sortedByteData, result);
        }

        [TestMethod]
        public void Bytes_passed_to_constructor_as_enumerable()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<byte>(_byteData as IEnumerable<byte>);

            // Assert

            AssertEx.AreEqual(_sortedByteData, result);
        }

        [TestMethod]
        public void Bytes_passed_to_add()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<byte>();
            for (int i = 0; i < _byteData.Count; ++i)
                result.Add(_byteData[i]);

            // Assert
            AssertEx.AreEqual(_sortedByteData, result);
        }

        [TestMethod]
        public void Bytes_passed_to_insert()
        {
            // Arrange
            // Act
            var result = new ObservableOrderedCollection<byte>();
            result.Insert(0, _byteData[0]);
            for (int i = 1; i < _byteData.Count; ++i)
            {
                var index = _random.Next() % result.Count;
                result.Insert(index, _byteData[i]);
            }

            // Assert
            AssertEx.AreEqual(_sortedByteData, result);
        }
    }
}
