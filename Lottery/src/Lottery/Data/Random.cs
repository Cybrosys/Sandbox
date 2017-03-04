using System;

namespace Lottery.Data
{
    public class Random : IRandom
    {
        private static System.Random _random = new System.Random();

        public int Next(int minValue, int maxValue, bool excludeMaxValue = false)
        {
            // The maxValue in System.Random.Next is exclusive, which means Int32.MaxValue can never be returned; I'm compensating.
            if (maxValue != Int32.MaxValue)
                return _random.Next(minValue, maxValue + 1);
            else
            {
                // I can't add one to maxValue (Int32.MaxValue), I would get a overflow exception, I'll instead add one to the value, unless it's 0.
                var value = _random.Next(minValue, maxValue);
                if (value > minValue)
                    return ++value;
                return value;
            }
        }
    }
}