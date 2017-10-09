using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Grouping.Test
{
    [TestClass]
    public class StringTests : DefaultImplTests<string>
    {
        public override void PrepareData()
        {
            _data = new List<string>(100);
            for (int i = 0; i < 100; ++i)
            {
                var number = _random.Next();
                _data.Add(number.ToString());
            }
            _sortedData = new List<string>(_data);
            _sortedData.Sort();
        }
    }
}
