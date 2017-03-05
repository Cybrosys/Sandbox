using System;
using System.Collections.Generic;

namespace Lottery.Data
{
    public class Row : IRow
    {
        public List<IRangeNumber> Numbers { get; set; }

        IReadOnlyList<IRangeNumber> IRow.Numbers => Numbers;
    }
}