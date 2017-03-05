using System;

namespace Lottery.Data
{
    public class RangeNumberGenerator : IRangeNumberGenerator
    {   
        private readonly IRandom _random;

        public RangeNumberGenerator(IRandom random)
        {
            _random = random;
        }

        public IRangeNumber Generate(int minValue, int maxValue)
        {
            return Generate(minValue, maxValue, minValue, maxValue);
        }

        public IRangeNumber Generate(int minValue, int maxValue, int minRangeValue, int maxRangeValue)
        {
            Assert(minValue, maxValue, minRangeValue, maxRangeValue);

            var value = (int)_random.Next(minValue, maxValue);

            var normalizedRangeMaxValue = maxRangeValue - minRangeValue;
            var normalizedValue = value - minRangeValue;
            var normalizedMiddleValue = (int)Math.Floor(normalizedRangeMaxValue / 2D);

            var isOdd = value % 2 != 0;
            var canBeMiddle = normalizedRangeMaxValue % 2 == 0;
            var isMiddle = canBeMiddle && normalizedValue == normalizedMiddleValue;
            var isUpper = !isMiddle && normalizedValue > normalizedMiddleValue;

            var number = new RangeNumber
            {
                Value = value,
                IsOdd = isOdd,
                IsEven = !isOdd,
                IsUpper = isUpper,
                IsMiddle = isMiddle,
                IsLower = !isMiddle && !isUpper
            };

            return number;
        }

        private void Assert(int minValue, int maxValue, int minRangeValue, int maxRangeValue)
        {
            if (minValue < 0) throw new ArgumentOutOfRangeException(nameof(minValue));
            if (maxValue < minValue) throw new ArgumentOutOfRangeException(nameof(maxValue));
            
            if (minRangeValue < 0) throw new ArgumentOutOfRangeException(nameof(minRangeValue));
            if (maxRangeValue < minRangeValue) throw new ArgumentOutOfRangeException(nameof(maxRangeValue));

            if (minValue < minRangeValue) throw new ArgumentOutOfRangeException(nameof(minValue));
            if (maxValue > maxRangeValue) throw new ArgumentOutOfRangeException(nameof(maxValue));
        }
    }
}