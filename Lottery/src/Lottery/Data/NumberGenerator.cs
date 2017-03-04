using System;

namespace Lottery.Data
{
    public class RangeNumberGenerator
    {   
        private readonly IRandom _random;

        public RangeNumberGenerator(IRandom random)
        {
            _random = random;
        }

        public IRangeNumber Generate(byte minValue, byte maxValue)
        {
            return Generate(minValue, maxValue, minValue, maxValue);
        }

        public IRangeNumber Generate(byte minValue, byte maxValue, byte minRangeValue, byte maxRangeValue)
        {
            Assert(minValue, maxValue, minRangeValue, maxRangeValue);

            var value = (byte)_random.Next(minValue, maxValue);

            var normalizedRangeMaxValue = maxRangeValue - minRangeValue;
            var normalizedValue = value - minRangeValue;
            var normalizedMiddleValue = (byte)Math.Round(normalizedRangeMaxValue / 2D);

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

        private void Assert(byte minValue, byte maxValue, byte minRangeValue, byte maxRangeValue)
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