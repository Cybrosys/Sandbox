namespace Lottery.Data
{
    public interface IRandom
    {
         int Next(int minValue, int maxValue, bool excludeMaxValue = false);
    }
}