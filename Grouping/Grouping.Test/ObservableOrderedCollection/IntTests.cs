using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Grouping.Test.ObservableOrderedCollection
{
    [TestClass]
    public class IntTests : DefaultImplTests<int>
    {
        public override void PrepareData()
        {
            _data = new List<int>(100);
            for (int i = 0; i < 100; ++i)
            {
                var number = (byte)(_random.Next() % 100);
                _data.Add(number);
            }
            _sortedData = new List<int>(_data);
            _sortedData.Sort();
        }
    }
}
