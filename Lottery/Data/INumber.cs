namespace Lottery
{
    public interface INumber
    {
        byte Value { get; }
        bool IsOdd { get; }
        bool IsEven { get; }
        bool IsUpper { get; }
        bool IsLower { get; }
    }
}