using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Grouping.Test.ObservableOrderedCollection
{
    [TestClass]
    public class ByteTests : DefaultImplTests<byte>
    {
        public override void PrepareData()
        {
            _data = new List<byte>(100);
            for (int i = 0; i < 100; ++i)
            {
                var number = (byte)(_random.Next() % 100);
                _data.Add(number);
            }
            _sortedData = new List<byte>(_data);
            _sortedData.Sort();
        }
    }
}
