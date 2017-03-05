namespace Lottery.Data
{
    public interface IRangeNumberGenerator
    {
        IRangeNumber Generate(int minValue, int maxValue);
        IRangeNumber Generate(int minValue, int maxValue, int minRangeValue, int maxRangeValue);
    }
}