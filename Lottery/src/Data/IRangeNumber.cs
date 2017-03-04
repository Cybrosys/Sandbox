namespace Lottery
{
    public interface IRangeNumber
    {
        byte Value { get; }
        bool IsOdd { get; }
        bool IsEven { get; }
        bool IsUpper { get; }
        bool IsMiddle { get; }
        bool IsLower { get; }
    }
}