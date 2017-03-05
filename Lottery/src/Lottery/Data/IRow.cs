using System.Collections.Generic;

namespace Lottery.Data
{
    public interface IRow
    {
         IReadOnlyList<IRangeNumber> Numbers { get; }
    }
}