using Grouping.Test.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grouping.Test
{
    [TestClass]
    public class ItemTests : DefaultImplTests<Item>
    {
        public override void PrepareData()
        {
            _data = new List<Item>(100);
            _orderBy = nameof(Item.Index);
            for (int i = 0; i < 100; ++i)
            {
                _data.Add(new Item
                {
                    Index = _random.Next() % 100,
                    Name = _random.Next().ToString(),
                    Timestamp = new DateTime(_random.Next(2000, 2018), _random.Next(1, 13), _random.Next(1, 28))
                });
            }
            _sortedData = _data.OrderBy(item => item.Index).ToList();
        }
    }
}
