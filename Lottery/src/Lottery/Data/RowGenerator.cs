using System;
using System.Collections.Generic;

namespace Lottery.Data
{
    public class RowGenerator
    {
        private readonly IRangeNumberGenerator _numberGenerator;

        public RowGenerator(IRangeNumberGenerator numberGenerator)
        {
            if (numberGenerator == null)
                throw new ArgumentNullException(nameof(numberGenerator));

            _numberGenerator = numberGenerator;
        }

        public IRow Generate(int size, int minValue, int maxValue)
        {
            var list = new List<IRangeNumber>(size);
            for (int i = 0; i < size; ++i)
            {
                list.Add(_numberGenerator.Generate(minValue, maxValue));
            }

            return new Row
            {
                Numbers = list
            };
        }
    }
}